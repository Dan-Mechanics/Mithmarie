using System;
using UnityEngine;

namespace Mithmarie
{
    public class Stairs : IBrushable
    {
        private readonly Transform player;
        private readonly World world;

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
            int depth = Mathf.Abs(b.z - a.z) + 1;

            int xDirection = a.x <= b.x ? 1 : -1;
            int yDirection = a.y <= b.y ? 1 : -1;
            int zDirection = a.z <= b.z ? 1 : -1;

            for (int y = 0; y < x + 1; y++)
            {
                for (int z = 0; z < depth; z++)
                {

                }
            }


                    switch (direction)
            {
                case CardinalDirection.North:
                    MakeNorthFacingStairs(a, add, width, depth, xDirection, yDirection, zDirection);
                    break;
                case CardinalDirection.East:
                    MakeEastFacingStairs(a, add, width, depth, xDirection, yDirection, zDirection);
                    break;
                case CardinalDirection.South:
                    MakeSouthFacingStairs(a, add, width, depth, xDirection, yDirection, zDirection);
                    break;
                case CardinalDirection.West:
                    MakeWestFacingStairs(a, add, width, depth, xDirection, yDirection, zDirection);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void MakeNorthFacingStairs(Vector3Int a, bool add, int x, int z, int xDirection, int yDirection, int zDirection)
        {
            for (int y = 0; y < z + 1; y++)
            {
                Place(a, add, xDirection, yDirection, zDirection, x, z, y);
            }
        }

        private void MakeEastFacingStairs(Vector3Int a, bool add, int x, int z, int xDirection, int yDirection, int zDirection)
        {
            for (int y = x; y >= 0; y--)
            {
                Place(a, add, xDirection, yDirection, zDirection, x, z, y);
            }
        }

        private void MakeSouthFacingStairs(Vector3Int a, bool add, int x, int z, int xDirection, int yDirection, int zDirection)
        {
            for (int y = z; y >= 0; y--)
            {
                Place(a, add, xDirection, yDirection, zDirection, x, z, y);
            }
        }

        private void MakeWestFacingStairs(Vector3Int a, bool add, int x, int z, int xDirection, int yDirection, int zDirection)
        {
            for (int y = 0; y < x + 1; y++)
            {
                Place(a, add, xDirection, yDirection, zDirection, x, z, y);
            }
        }

        private void Place(Vector3Int blockPos, bool add, int xDirection, int yDirection, int zDirection, int x, int z, int y)
        {
            blockPos += new Vector3Int(x * xDirection, y * yDirection, z * zDirection);
            if (add)
            {
                world.Add(blockPos);
            }
            else
            {
                world.Remove(blockPos);
            }
        }
    }
}
