using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public interface IChunk
    {
        public void Setup(Vector3Int chunkPos, Transform eyes);
        public void Tick();
        public void GenerateMesh(HashSet<Vector3Int> blocks, Dictionary<Vector3Int, HashSet<Vector3Int>> allChunks);
        public void Dispose();
    }
}