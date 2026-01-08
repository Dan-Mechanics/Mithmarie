using System;
using UnityEngine;

namespace Mithmarie
{
    public class Cylinder : IBrushable
    {
        private readonly World world;

        public Cylinder(World world)
        {
            this.world = world;
        }

        public void Add(Vector3Int a, Vector3Int b) => Apply(a, b, true);
        public void Remove(Vector3Int a, Vector3Int b) => Apply(a, b, false);

        private void Apply(Vector3Int a, Vector3Int b, bool add)
        {
            if (a.x > b.x)
                Utils.SwapAxis(ref a, ref b, Axis.X);

            if (a.y > b.y)
                Utils.SwapAxis(ref a, ref b, Axis.Y);

            if (a.z > b.z)
                Utils.SwapAxis(ref a, ref b, Axis.Z);

            int width = Mathf.Abs(b.x - a.x) + 1;
            int height = Mathf.Abs(b.y - a.y) + 1;
            int depth = Mathf.Abs(b.z - a.z) + 1;

            Vector3Int temp = Vector3Int.zero;
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if (!IsBlockWithin(x, z, width, depth, add))
                        continue;

                    // A CYLINDER IS A CIRCLE WITH HEIGHT HERE.
                    for (int y = 0; y < height; y++)
                    {
                        temp.x = x;
                        temp.y = y;
                        temp.z = z;
                        temp += a;

                        world.ChangeBlock(temp, add);
                    }
                }
            }
        }

        private bool IsBlockWithin(int x, int z, int width, int depth, bool circle)
        {
            float xRad = width / 2f;
            float zRad = depth / 2f;

            float xCenter = (width - 1) / 2f;
            float zCenter = (depth - 1) / 2f;

            float xDist = Mathf.Abs(x - xCenter);
            float zDist = Mathf.Abs(z - zCenter);

            float dist = Utils.GetEllipseMagnitude(new Vector3(xDist, 0f, zDist), xRad, zRad);
            if (circle)
            {
                return dist <= xRad && dist >= xRad - 1.125f;
            }
            else
            {
                // CYLINDER.
                return dist <= xRad;
            }
        }
    }
}
