using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    /// <summary>
    /// You could for example implement a DemoChunk.cs whereby 
    /// very many cubes are spawned one by one.
    /// Alternatively, you could implement a more optimized GreedyChunk.cs here.
    /// </summary>
    public interface IChunkMeshable
    {
        public void Setup(Vector3Int chunkPos, Transform eyes);
        public void Tick();
        public void GenerateMesh(HashSet<Vector3Int> blocks, Dictionary<Vector3Int, HashSet<Vector3Int>> chunks);
        public void Dispose();
    }
}