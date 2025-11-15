using UnityEngine;
using System.Collections.Generic;

namespace Mitholca
{
    public interface IMeshGeneratable
    {
        void GenerateMesh(HashSet<Vector3Int> hash);
    }
}
