using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class ChunkManager : MonoBehaviour
    {
        [SerializeField] private GameObject chunkPrefab = default;
        private readonly Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();

        public void DrawChunk(Vector3Int chunkPos, HashSet<Vector3Int> blocks)
        {
            if ((blocks == null || blocks.Count <= 0) && chunks.ContainsKey(chunkPos))
            {
                Destroy(chunks[chunkPos].gameObject);
                chunks.Remove(chunkPos);
                return;
            }

            if (!chunks.ContainsKey(chunkPos))
                AddChunk(chunkPos);

            chunks[chunkPos].Draw(blocks);
        }

        private void AddChunk(Vector3Int chunkPos)
        {
            Chunk chunk = Instantiate(chunkPrefab, chunkPos * World.CHUNK_SIZE, Quaternion.identity).GetComponent<Chunk>();
            chunks.Add(chunkPos, chunk);
            chunks[chunkPos].Setup();
        }
    }
}
