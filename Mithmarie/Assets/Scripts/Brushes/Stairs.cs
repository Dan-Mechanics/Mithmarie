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

        private void MakeNorthFacingStairs(Vector3Int a, bool add, int width, int depth, int xDirection, int yDirection, int zDirection)
        {
            Vector3Int temp = Vector3Int.zero;
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    for (int y = 0; y < z + 1; y++)
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

        private void MakeEastFacingStairs(Vector3Int a, bool add, int width, int depth, int xDirection, int yDirection, int zDirection)
        {
            Vector3Int temp = Vector3Int.zero;
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    for (int y = x; y >= 0; y--)
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

        private void MakeSouthFacingStairs(Vector3Int a, bool add, int width, int depth, int xDirection, int yDirection, int zDirection)
        {
            Vector3Int temp = Vector3Int.zero;
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    for (int y = z; y >= 0; y--)
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

        private void MakeWestFacingStairs(Vector3Int a, bool add, int width, int depth, int xDirection, int yDirection, int zDirection)
        {
            Vector3Int temp = Vector3Int.zero;
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    for (int y = 0; y < x + 1; y++)
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
