using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class ChunkManager : MonoBehaviour
    {
        [SerializeField] private GameObject chunkPrefab = default;
        private readonly Dictionary<Vector3Int, IChunk> meshes = new Dictionary<Vector3Int, IChunk>();
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
            foreach (KeyValuePair<Vector3Int, IChunk> chunkMesh in meshes)
            {
                chunkMesh.Value.Tick();
            }
        }

        private void AddChunk(Vector3Int chunkPos)
        {
            IChunk chunk = Instantiate(chunkPrefab, transform.position,
                Quaternion.identity).GetComponent<IChunk>();

            meshes.Add(chunkPos, chunk);
            meshes[chunkPos].Setup(chunkPos, eyes);
        }
    }
}
