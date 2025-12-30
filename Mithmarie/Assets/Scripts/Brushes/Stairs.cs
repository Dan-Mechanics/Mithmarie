using System;
using UnityEngine;

namespace Mithmarie
{
    public class Stairs : IBrushable
    {
        private readonly Transform player;
        private readonly World world;

        private delegate bool IsBlockWithinStair(int width, int height, int depth, int x, int y, int z);

        public Stairs(World world, Transform player)
        {
            this.world = world;
            this.player = player;
        }

        public void Add(Vector3Int a, Vector3Int b) => Apply(a, b, true);
        public void Remove(Vector3Int a, Vector3Int b) => Apply(a, b, false);

        private void Apply(Vector3Int a, Vector3Int b, bool add)
        {
            float rot = player.rotation.eulerAngles.y;
            float angle = 22.5f;
            CardinalDirection direction = CardinalDirection.North;
            if (rot > angle)
                direction = CardinalDirection.East;

            if (rot > 90f + angle)
                direction = CardinalDirection.South;

            if (rot > 180f + angle)
                direction = CardinalDirection.West;

            if (rot > 270f + angle)
                direction = CardinalDirection.North;

            int width = Mathf.Abs(b.x - a.x) + 1;
            int height = Mathf.Abs(b.y - a.y) + 1;
            int depth = Mathf.Abs(b.z - a.z) + 1;

            int xDirection = a.x <= b.x ? 1 : -1;
            int yDirection = a.y <= b.y ? 1 : -1;
            int zDirection = a.z <= b.z ? 1 : -1;

            IsBlockWithinStair isBlockWithinStair = IsBlockWithinStairNorth;
            switch (direction)
            {
                case CardinalDirection.East:
                    //isBlockWithinStair =
                    break;
                case CardinalDirection.South:
                    break;
                case CardinalDirection.West:
                    break;
            }

            Vector3Int temp = Vector3Int.zero;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        if (!isBlockWithinStair(width, height, depth, x, y, z))
                            continue;
                        
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

        private bool IsBlockWithinStairNorth(int width, int height, int depth, int x, int y, int z)
        {
            float slope = (float)height / depth;
            int max = Mathf.CeilToInt(z * slope);
            //Debug.Log($"max{max}, y{y}");
            return y <= max;
        }
    }
}
