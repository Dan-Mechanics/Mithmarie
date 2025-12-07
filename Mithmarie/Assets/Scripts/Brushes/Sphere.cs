using System;
using UnityEngine;

namespace Mithmarie
{
    public class Sphere : IBrush
    {
        private World world;

        public void Setup(World world) => this.world = world;
        public void Add(Vector3Int a, Vector3Int b) => Apply(a, b, true);
        public void Remove(Vector3Int a, Vector3Int b) => Apply(a, b, false);

        public void Apply(Vector3Int a, Vector3Int b, bool add)
        {
            int height = Mathf.Abs(b.y - a.y) + 1;

            Vector3 center = new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
            center /= 2f;

            float radius = height / 2f;


            int width = Mathf.Abs(b.x - a.x) + 1;
            int depth = Mathf.Abs(b.z - a.z) + 1;

            float xRadius = width / 2f;
            float yRadius = height / 2f;
            float zRadius = depth / 2f;

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

                        if (Vector3.Distance(center, temp) > radius)
                            continue;

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
