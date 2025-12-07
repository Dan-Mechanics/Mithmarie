using System;
using UnityEngine;

namespace Mithmarie
{
    public interface IBrush 
    {
        void Setup(World world);
        void Add(Vector3Int a, Vector3Int b);
        void Remove(Vector3Int a, Vector3Int b);
    }
}
