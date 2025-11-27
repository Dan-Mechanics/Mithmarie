using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class ChunkManager : MonoBehaviour
    {
        [SerializeField] private GameObject chunkPrefab = default;
        private readonly Dictionary<Vector3Int, IChunkMeshable> chunkMeshes = new Dictionary<Vector3Int, IChunkMeshable>();
        private Transform eyes;

        private void Awake()
        {
            eyes = GameObject.FindWithTag("MainCamera").transform;
        }

        public void DrawChunk(Vector3Int chunkPos, Dictionary<Vector3Int, HashSet<Vector3Int>> allChunks)
        {
            if (!allChunks.ContainsKey(chunkPos) || allChunks[chunkPos] == null || allChunks[chunkPos].Count <= 0)
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

            chunkMeshes[chunkPos].GenerateMesh(allChunks[chunkPos], allChunks);
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
