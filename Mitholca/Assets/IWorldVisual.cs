using UnityEngine;
using System.Collections.Generic;

namespace Mitholca
{
    public interface IWorldVisual
    {
        void Draw(HashSet<Vector3Int> hash);
    }
}
