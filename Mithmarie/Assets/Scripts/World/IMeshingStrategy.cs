using UnityEngine;
using System.Collections.Generic;

namespace Mithmarie
{
    public interface IMeshingStrategy
    {
        Mesh GenerateMesh(HashSet<Vector3Int> blocks);
    }
}
