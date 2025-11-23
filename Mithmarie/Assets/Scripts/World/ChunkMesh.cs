using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    /// <summary>
    /// https://github.com/samhogan/Minecraft-Unity3D/blob/master/Assets/Scripts/TerrainChunk.cs
    /// </summary>
    public class ChunkMesh : MonoBehaviour
    {
        [SerializeField] private MeshFilter filter = default;
        [SerializeField] private MeshCollider coll = default;
        [SerializeField, Min(0f)] private float maxViewingRange = default;
        [SerializeField] private MeshColliderCookingOptions cookingOptions = default;

        private Transform eyes;
        private Mesh mesh;
        private Vector3 center;

        private static readonly Vector3 upForward = new Vector3(0, 1, 1);
        private static readonly Vector3 upRight = new Vector3(1, 1, 0);
        private static readonly Vector3 forwardRight = new Vector3(1, 0, 1);
        private static readonly List<Vector3> verts = new List<Vector3>();
        private static readonly List<int> tris = new List<int>();
        private static readonly List<Vector2> uvs = new List<Vector2>();
        private static readonly int[] tempTris = new int[6];
        private static readonly Vector2[] faceUvs = new Vector2[]
        { 
            new Vector2(0, 0), 
            new Vector2(0, 1), 
            new Vector2(1, 1),
            new Vector2(1, 0)
        };

        public void Setup(Vector3Int chunkPos, Transform eyes)
        {
            mesh = new Mesh();
            mesh.MarkDynamic();
            filter.mesh = mesh;
            coll.cookingOptions = cookingOptions;

            center = chunkPos * World.CHUNK_SIZE;
            center += 0.5f * World.CHUNK_SIZE * Vector3.one;
            this.eyes = eyes;
        }

        public void Tick()
        {
            gameObject.SetActive(Vector3.Distance(eyes.position, center) <= maxViewingRange);
        }

        private bool Has(Vector3Int blockPos, Dictionary<Vector3Int, HashSet<Vector3Int>> chunks)
        {
            Vector3Int chunkPos = Utils.GetChunkPos(blockPos, World.CHUNK_SIZE);
            if (!chunks.ContainsKey(chunkPos))
                return false;

            return chunks[chunkPos].Contains(blockPos);
        }

        public void GenerateMesh(HashSet<Vector3Int> blocks, Dictionary<Vector3Int, HashSet<Vector3Int>> chunks)
        {
            verts.Clear();
            tris.Clear();
            uvs.Clear();
            mesh.Clear();
            
            foreach (Vector3Int blockPos in blocks)
            {
                int faceCount = 0;
                int offset = verts.Count;

                if (!Has(blockPos + Vector3Int.up, chunks))
                {
                    verts.Add(blockPos + Vector3Int.up);
                    verts.Add(blockPos + upForward);
                    verts.Add(blockPos + Vector3Int.one);
                    verts.Add(blockPos + upRight);
                    faceCount++;
                }

                if (!Has(blockPos + Vector3Int.down, chunks))
                {
                    verts.Add(blockPos + Vector3Int.zero);
                    verts.Add(blockPos + Vector3Int.right);
                    verts.Add(blockPos + forwardRight);
                    verts.Add(blockPos + Vector3Int.forward);
                    faceCount++;
                }

                if (!Has(blockPos + Vector3Int.forward, chunks))
                {
                    verts.Add(blockPos + forwardRight);
                    verts.Add(blockPos + Vector3Int.one);
                    verts.Add(blockPos + upForward);
                    verts.Add(blockPos + Vector3Int.forward);
                    faceCount++;
                }

                if (!Has(blockPos + Vector3Int.right,  chunks))
                {
                    verts.Add(blockPos + Vector3Int.right);
                    verts.Add(blockPos + upRight);
                    verts.Add(blockPos + Vector3Int.one);
                    verts.Add(blockPos + forwardRight);
                    faceCount++;
                }

                if (!Has(blockPos + Vector3Int.back, chunks))
                {
                    verts.Add(blockPos + Vector3Int.zero);
                    verts.Add(blockPos + Vector3Int.up);
                    verts.Add(blockPos + upRight);
                    verts.Add(blockPos + Vector3Int.right);
                    faceCount++;
                }

                if (!Has(blockPos + Vector3Int.left, chunks))
                {
                    verts.Add(blockPos + Vector3Int.forward);
                    verts.Add(blockPos + upForward);
                    verts.Add(blockPos + Vector3Int.up);
                    verts.Add(blockPos + Vector3Int.zero);
                    faceCount++;
                }
                
                for (int i = 0; i < faceCount; i++)
                {
                    tempTris[0] = offset + i * 4;
                    tempTris[1] = offset + i * 4 + 1;
                    tempTris[2] = offset + i * 4 + 2;

                    tempTris[3] = offset + i * 4;
                    tempTris[4] = offset + i * 4 + 2;
                    tempTris[5] = offset + i * 4 + 3;

                    tris.AddRange(tempTris);
                    uvs.AddRange(faceUvs);
                }
            }

            mesh.vertices = verts.ToArray();
            mesh.triangles = tris.ToArray();
            mesh.uv = uvs.ToArray();

            verts.Clear();
            tris.Clear();
            uvs.Clear();

            mesh.RecalculateNormals();
            Physics.BakeMesh(mesh.GetInstanceID(), false, cookingOptions);
            coll.sharedMesh = mesh;
        }

        public void Dispose()
        {
            verts.Clear();
            tris.Clear();
            uvs.Clear();
            mesh.Clear();

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}