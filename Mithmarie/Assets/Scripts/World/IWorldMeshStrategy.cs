using UnityEngine;
using System.Collections.Generic;

namespace Mithmarie
{
    public interface IWorldMeshStrategy
    {
        MeshData GenerateMesh(HashSet<Vector3Int> blocks);
        List<MeshData> GenerateAsChunks(Dictionary<Vector3Int, HashSet<Vector3Int>> chunks);
    }
}
