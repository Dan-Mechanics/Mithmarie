using System;
using UnityEngine;

namespace Mithmarie
{
    public class Fill : IBrushable
    {
        private readonly World world;

        public Fill(World world)
        {
            this.world = world;
        }

        public void Add(Vector3Int a, Vector3Int b) => Apply(a, b, true);
        public void Remove(Vector3Int a, Vector3Int b) => Apply(a, b, false);

        private void Apply(Vector3Int a, Vector3Int b, bool add)
        {
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

                        if (add) 
                        {
                            world.Add(a + temp);
                        }
                        else
                        {
                            world.Remove(a + temp);
                        }
                    }
                }
            }
        }
    }
}
