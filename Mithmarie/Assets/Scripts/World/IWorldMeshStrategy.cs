using UnityEngine;
using System.Collections.Generic;

namespace Mithmarie
{
    public interface IWorldMeshStrategy
    {
        Mesh GenerateMesh(HashSet<Vector3Int> blocks);
    }
}
