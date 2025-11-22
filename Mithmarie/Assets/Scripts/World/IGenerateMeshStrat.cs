using UnityEngine;
using System.Collections.Generic;

namespace Mithmarie
{
    public interface IGenerateMeshStrat
    {
        Mesh GenerateMesh(HashSet<Vector3Int> blocks);
    }
}
