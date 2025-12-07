using System;
using UnityEngine;

namespace Mithmarie
{
    public class Sphere : IBrush
    {
        private World world;

        public void Setup(World world) => this.world = world;
        public void Add(Vector3Int a, Vector3Int b) => Apply(a, b, true);
        public void Remove(Vector3Int a, Vector3Int b) => Apply(a, b, true);

        public void Apply(Vector3Int a, Vector3Int b, bool add)
        {
            throw new NotImplementedException();
        }
    }
}
