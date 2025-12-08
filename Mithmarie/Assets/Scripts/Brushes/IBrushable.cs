using System;
using UnityEngine;

namespace Mithmarie
{
    public interface IBrushable 
    {
        void Add(Vector3Int a, Vector3Int b);
        void Remove(Vector3Int a, Vector3Int b);
    }
}
