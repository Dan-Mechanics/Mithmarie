using System;
using UnityEngine;

namespace Mithmarie
{
    public class Walls : IBrush
    {
        private World world;

        public void Setup(World world) => this.world = world;
        public void Add(Vector3Int a, Vector3Int b) => FillSection(a, b, true);
        public void Remove(Vector3Int a, Vector3Int b) => FillSection(a, b, false);

        public void FillSection(Vector3Int a, Vector3Int b, bool add)
        {
            throw new NotImplementedException();
        }
    }
}
