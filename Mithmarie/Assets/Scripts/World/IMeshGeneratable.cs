using UnityEngine;
using System.Collections.Generic;

namespace Mithmarie
{
    public interface IMeshGeneratable
    {
        void GenerateMesh(HashSet<Vector3Int> hash);
    }
}
