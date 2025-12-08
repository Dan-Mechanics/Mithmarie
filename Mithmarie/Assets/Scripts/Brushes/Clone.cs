using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class Clone : IBrushable
    {
        private readonly HashSet<Vector3Int> example = new HashSet<Vector3Int>();
        private readonly World world;

        public Clone(World world)
        {
            this.world = world;
        }

        public void Add(Vector3Int a, Vector3Int b) => Copy(a, b);
        public void Remove(Vector3Int a, Vector3Int b) => Paste(a);

        private void Copy(Vector3Int a, Vector3Int b)
        {
            example.Clear();
            
            int width = Mathf.Abs(b.x - a.x) + 1;
            int height = Mathf.Abs(b.y - a.y) + 1;
            int depth = Mathf.Abs(b.z - a.z) + 1;

            int xDirection = a.x <= b.x ? 1 : -1;
            int yDirection = a.y <= b.y ? 1 : -1;
            int zDirection = a.z <= b.z ? 1 : -1;

            Vector3Int temp = Vector3Int.zero;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        temp.x = x * xDirection;
                        temp.y = y * yDirection;
                        temp.z = z * zDirection;
                        temp += a;

                        if (world.Has(temp))
                            example.Add(temp - a);
                    }
                }
            }
        }

        private void Paste(Vector3Int a)
        {
            foreach (Vector3Int blockPos in example)
            {
                world.Add(a + blockPos + Vector3Int.up);
            }
        }
    }
}
