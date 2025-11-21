using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    /// <summary>
    /// Source: https://github.com/samhogan/Minecraft-Unity3D/blob/master/Assets/Scripts/TerrainChunk.cs
    /// </summary>
    public class CulledMeshGenerator : MonoBehaviour, IMeshGeneratable
    {
        [SerializeField] private MeshFilter filter = default;
        [SerializeField] private MeshCollider coll = default;
        [SerializeField] private MeshColliderCookingOptions cookingOptions = default;

        private Mesh mesh;
        private readonly Vector3 upForward = new Vector3(0, 1, 1);
        private readonly Vector3 upRight = new Vector3(1, 1, 0);
        private readonly Vector3 forwardRight = new Vector3(1, 0, 1);
        private readonly List<Vector3> verts = new List<Vector3>();
        private readonly List<int> tris = new List<int>();
        private readonly List<Vector2> uvs = new List<Vector2>();
        private readonly int[] newTris = new int[6];
        private readonly Vector2[] faceUvs = new Vector2[]
        { 
            new Vector2(0, 0), 
            new Vector2(0, 1), 
            new Vector2(1, 1),
            new Vector2(1, 0)
        };

        public void Create()
        {
            mesh = new Mesh();
            mesh.MarkDynamic();
            filter.mesh = mesh;
            coll.cookingOptions = cookingOptions;
        }

        public void GenerateMesh(HashSet<Vector3Int> blocks)
        {
            verts.Clear();
            tris.Clear();
            uvs.Clear();
            
            mesh.Clear();
            
            foreach (Vector3Int blockPos in blocks)
            {
                int faceCount = 0;
                int offset = verts.Count;

                if (!blocks.Contains(blockPos + Vector3Int.up))
                {
                    verts.Add(blockPos + Vector3Int.up);
                    verts.Add(blockPos + upForward);
                    verts.Add(blockPos + Vector3Int.one);
                    verts.Add(blockPos + upRight);
                    faceCount++;
                }

                if (!blocks.Contains(blockPos + Vector3Int.down))
                {
                    verts.Add(blockPos + Vector3Int.zero);
                    verts.Add(blockPos + Vector3Int.right);
                    verts.Add(blockPos + forwardRight);
                    verts.Add(blockPos + Vector3Int.forward);
                    faceCount++;
                }

                if (!blocks.Contains(blockPos + Vector3Int.forward))
                {
                    verts.Add(blockPos + forwardRight);
                    verts.Add(blockPos + Vector3Int.one);
                    verts.Add(blockPos + upForward);
                    verts.Add(blockPos + Vector3Int.forward);
                    faceCount++;
                }

                if (!blocks.Contains(blockPos + Vector3Int.right))
                {
                    verts.Add(blockPos + Vector3Int.right);
                    verts.Add(blockPos + upRight);
                    verts.Add(blockPos + Vector3Int.one);
                    verts.Add(blockPos + forwardRight);
                    faceCount++;
                }

                if (!blocks.Contains(blockPos + Vector3Int.back))
                {
                    verts.Add(blockPos + Vector3Int.zero);
                    verts.Add(blockPos + Vector3Int.up);
                    verts.Add(blockPos + upRight);
                    verts.Add(blockPos + Vector3Int.right);
                    faceCount++;
                }

                if (!blocks.Contains(blockPos + Vector3Int.left))
                {
                    verts.Add(blockPos + Vector3Int.forward);
                    verts.Add(blockPos + upForward);
                    verts.Add(blockPos + Vector3Int.up);
                    verts.Add(blockPos + Vector3Int.zero);
                    faceCount++;
                }
                
                for (int i = 0; i < faceCount; i++)
                {
                    newTris[0] = offset + i * 4;
                    newTris[1] = offset + i * 4 + 1;
                    newTris[2] = offset + i * 4 + 2;

                    newTris[3] = offset + i * 4;
                    newTris[4] = offset + i * 4 + 2;
                    newTris[5] = offset + i * 4 + 3;

                    tris.AddRange(newTris);
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

        public void Destroy()
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