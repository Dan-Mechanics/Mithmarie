using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class WorldVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject chunkPrefab = default;
        private readonly Dictionary<Vector3Int, ChunkMesh> meshes = new Dictionary<Vector3Int, ChunkMesh>();

        private Transform eyes;

        private void Awake()
        {
            eyes = GameObject.FindWithTag("MainCamera").transform;
        }

        public void DrawChunk(Vector3Int chunkPos, Dictionary<Vector3Int, HashSet<Vector3Int>> allChunks)
        {
            if (!allChunks.ContainsKey(chunkPos) || allChunks[chunkPos] == null || allChunks[chunkPos].Count <= 0)
            {
                if (meshes.ContainsKey(chunkPos) && meshes[chunkPos] != null)
                {
                    meshes[chunkPos].Dispose();
                    meshes[chunkPos] = null;
                }

                meshes.Remove(chunkPos);
                return;
            }

            if (!meshes.ContainsKey(chunkPos))
                AddChunk(chunkPos);

            meshes[chunkPos].GenerateMesh(allChunks[chunkPos], allChunks);
        }

        private void FixedUpdate()
        {
            foreach (KeyValuePair<Vector3Int, ChunkMesh> chunkMesh in meshes)
            {
                chunkMesh.Value.Tick();
            }
        }

        private void AddChunk(Vector3Int chunkPos)
        {
            ChunkMesh chunk = Instantiate(chunkPrefab, transform.position,
                Quaternion.identity).GetComponent<ChunkMesh>();

            meshes.Add(chunkPos, chunk);
            meshes[chunkPos].Setup(chunkPos, eyes);
        }
    }
}
