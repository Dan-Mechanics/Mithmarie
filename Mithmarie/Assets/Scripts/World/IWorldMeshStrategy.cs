using UnityEngine;
using System.Collections.Generic;

namespace Mithmarie
{
    public interface IWorldMeshStrategy
    {
        Mesh GenerateMesh(HashSet<Vector3Int> blocks);
        List<Mesh> GenerateAsChunks(Dictionary<Vector3Int, HashSet<Vector3Int>> chunks);
    }
}
