using UnityEngine;
using System.Collections.Generic;

namespace Mithmarie
{
    public interface IMeshGeneratable
    {
        Mesh GenerateMesh(HashSet<Vector3Int> blocks);
    }
}
