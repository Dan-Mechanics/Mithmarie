using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class ChunkVisualizationManager : MonoBehaviour
    {
        [SerializeField] private GameObject chunkPrefab = default;
        private readonly Dictionary<Vector3Int, IChunkMeshable> chunkMeshes = new Dictionary<Vector3Int, IChunkMeshable>();
        private Transform eyes;

        public void Setup(Transform eyes) => this.eyes = eyes;

        public void DrawChunk(Vector3Int chunkPos, Dictionary<Vector3Int, HashSet<Vector3Int>> chunks)
        {
            if (!chunks.ContainsKey(chunkPos) || chunks[chunkPos] == null || chunks[chunkPos].Count <= 0)
            {
                if (chunkMeshes.ContainsKey(chunkPos) && chunkMeshes[chunkPos] != null)
                {
                    chunkMeshes[chunkPos].Dispose();
                    chunkMeshes[chunkPos] = null;
                }

                chunkMeshes.Remove(chunkPos);
                return;
            }

            if (!chunkMeshes.ContainsKey(chunkPos))
                AddChunk(chunkPos);

            chunkMeshes[chunkPos].GenerateMesh(chunks[chunkPos], chunks);
        }

        private void FixedUpdate()
        {
            foreach (KeyValuePair<Vector3Int, IChunkMeshable> chunkMesh in chunkMeshes)
            {
                chunkMesh.Value.Tick();
            }
        }

        private void AddChunk(Vector3Int chunkPos)
        {
            IChunkMeshable chunk = Instantiate(chunkPrefab, transform.position,
                Quaternion.identity).GetComponent<IChunkMeshable>();

            chunkMeshes.Add(chunkPos, chunk);
            chunkMeshes[chunkPos].Setup(chunkPos, eyes);
        }
    }
}
