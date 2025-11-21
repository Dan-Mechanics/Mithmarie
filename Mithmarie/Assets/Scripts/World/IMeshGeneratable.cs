using UnityEngine;
using System.Collections.Generic;

namespace Mithmarie
{
    public interface IMeshGeneratable
    {
        void Create();
        void GenerateMesh(HashSet<Vector3Int> blocks);
        void Destroy();
    }
}
