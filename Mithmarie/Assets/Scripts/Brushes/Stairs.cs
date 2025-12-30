using System;
using UnityEngine;

namespace Mithmarie
{
    public class Stairs : IBrushable
    {
        private readonly Transform player;
        private readonly World world;

        // I KNOW GLOBAL VARIABLES ARE NOT
        // IDEAL BUT IT WOULD BE WORSE OTHERWISE.
        private float slope;
        private Vector3Int prev;

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

            Debug.Log(direction);
            slope = (float)height / depth;

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    switch (direction)
                    {
                        case CardinalDirection.North:
                            MakeNorthFacingStairs(a, add, x, z, xDirection, yDirection, zDirection);
                            break;
                        case CardinalDirection.East:
                            MakeEastFacingStairs(a, add, x, z, xDirection, yDirection, zDirection);
                            break;
                        case CardinalDirection.South:
                            MakeSouthFacingStairs(a, add, x, z, xDirection, yDirection, zDirection);
                            break;
                        case CardinalDirection.West:
                            MakeWestFacingStairs(a, add, x, z, xDirection, yDirection, zDirection);
                            break;
                    }
                }
            }
        }

        private void MakeNorthFacingStairs(Vector3Int a, bool add, int x, int z, int xDirection, int yDirection, int zDirection)
        {
            int max = z + 1;
            for (int y = 0; y < max; y++)
            {
                Place(a, add, xDirection, yDirection, zDirection, x, y, z);
            }
        }

        private void MakeEastFacingStairs(Vector3Int a, bool add, int x, int z, int xDirection, int yDirection, int zDirection)
        {
            int max = x;
            for (int y = max; y >= 0; y--)
            {
                Place(a, add, xDirection, yDirection, zDirection, x, y, z);
            }
        }

        private void MakeSouthFacingStairs(Vector3Int a, bool add, int x, int z, int xDirection, int yDirection, int zDirection)
        {
            int max = z;
            for (int y = max; y >= 0; y--)
            {
                Place(a, add, xDirection, yDirection, zDirection, x, y, z);
            }
        }

        private void MakeWestFacingStairs(Vector3Int a, bool add, int x, int z, int xDirection, int yDirection, int zDirection)
        {
            int max = x + 1;
            for (int y = 0; y < max; y++)
            {
                Place(a, add, xDirection, yDirection, zDirection, x, y, z);
            }
        }

        private void Place(Vector3Int blockPos, bool add, int xDirection, int yDirection, int zDirection, int x, int y, int z)
        {
            blockPos += new Vector3Int(x * xDirection, (int)(y * yDirection * slope), z * zDirection);
            int height = Mathf.Abs(blockPos.y - prev.y);

            for (int i = 0; i < height; i++)
            {
                if (add)
                {
                    world.Add(blockPos);
                }
                else
                {
                    world.Remove(blockPos);
                }
                blockPos.y++;
            }

            prev = blockPos;
        }
    }
}
