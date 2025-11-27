using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class CulledChunk : MonoBehaviour, IChunk
    {
        [SerializeField] private MeshFilter filter = default;
        [SerializeField] private MeshCollider coll = default;
        [SerializeField, Min(0f)] private float maxViewingRange = default;
        [SerializeField] private MeshColliderCookingOptions cookingOptions = default;

        private static readonly List<Vector3> verts = new List<Vector3>();
        private static readonly List<int> tris = new List<int>();
        private static readonly List<Vector2> uvs = new List<Vector2>();

        private Dictionary<Vector3Int, HashSet<Vector3Int>> allChunks;
        private Transform eyes;
        private Mesh mesh;
        private Vector3 center;

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

        private bool HasInChunks(Vector3Int blockPos)
        {
            Vector3Int chunkPos = Utils.GetChunkPos(blockPos, World.CHUNK_SIZE);
            if (!allChunks.ContainsKey(chunkPos))
                return false;

            return allChunks[chunkPos].Contains(blockPos);
        }

        public void GenerateMesh(HashSet<Vector3Int> blocks, Dictionary<Vector3Int, HashSet<Vector3Int>> allChunks)
        {
            if (!gameObject.activeSelf)
                return;

            this.allChunks = allChunks;
            MeshUtils.GenerateCulledMesh(blocks, HasInChunks, verts, tris, uvs);

            mesh.Clear();
            mesh.vertices = verts.ToArray();
            mesh.triangles = tris.ToArray();
            mesh.uv = uvs.ToArray();

            mesh.RecalculateNormals();
            Physics.BakeMesh(mesh.GetInstanceID(), false, cookingOptions);
            coll.sharedMesh = mesh;
        }

        public void Dispose()
        {
            mesh.Clear();
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}