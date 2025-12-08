using System;
using UnityEngine;

namespace Mithmarie
{
    public class Cylinder : IBrushable
    {
        private World world;

        public void Setup(World world) => this.world = world;
        public void Add(Vector3Int a, Vector3Int b) => Summarize(a, b, true);
        public void Remove(Vector3Int a, Vector3Int b) => Summarize(a, b, true);

        private void Summarize(Vector3Int a, Vector3Int b, bool add)
        {
            int height = Mathf.Abs(b.y - a.y) + 1;
            float radius = Vector2.Distance(new Vector2(b.x, b.z), new Vector2(a.x, a.z)) + 1f;
            radius /= 2f;

            Vector3 center = a + b;
            center /= 2f;
            Apply(height, Mathf.CeilToInt(radius), Utils.GetBlockPos(center), add);
        }

        private void Apply(int height, int radius, Vector3Int center, bool add)
        {
            int halfHeight = Mathf.CeilToInt(height / 2f);

            for (int x = -radius; x <= radius; x++)
            {
                for (int z = -radius; z <= radius; z++)
                {
                    float mag = new Vector2(x, z).magnitude;
                    if (mag > radius || mag < radius - 1f)
                        continue;

                    for (int y = -halfHeight; y < halfHeight; y++)
                    {
                        if (add)
                        {
                            world.Add(center + new Vector3Int(x, y, z));
                        }
                        else
                        {
                            world.Remove(center + new Vector3Int(x, y, z));
                        }
                    }
                }
            }
        }
    }
}
