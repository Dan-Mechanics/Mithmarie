using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class ChunksVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject chunkPrefab = default;
        private readonly Dictionary<Vector3Int, IMeshGeneratable> generatables = new Dictionary<Vector3Int, IMeshGeneratable>();

        public void DrawChunk(Vector3Int chunkPos, HashSet<Vector3Int> blocks)
        {
            if ((blocks == null || blocks.Count <= 0) && generatables.ContainsKey(chunkPos))
            {
                generatables[chunkPos].Destroy();
                generatables.Remove(chunkPos);
                return;
            }

            if (!generatables.ContainsKey(chunkPos))
                AddChunk(chunkPos);

            generatables[chunkPos].GenerateMesh(blocks);
        }

        private void AddChunk(Vector3Int chunkPos)
        {
            IMeshGeneratable generatable = Instantiate(chunkPrefab, chunkPos * World.CHUNK_SIZE, Quaternion.identity).GetComponent<IMeshGeneratable>();
            generatables.Add(chunkPos, generatable);
            generatables[chunkPos].Create();
        }
    }
}
