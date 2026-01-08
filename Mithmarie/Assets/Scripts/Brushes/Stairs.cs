using UnityEngine;

namespace Mithmarie
{
    public class Stairs : IBrushable
    {
        private readonly Transform player;
        private readonly World world;

        private delegate bool IsBlockWithinStairs(int width, int height, int depth, int x, int y, int z);

        public Stairs(World world, Transform player)
        {
            this.world = world;
            this.player = player;
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

            IsBlockWithinStairs isBlockWithinStairs = IsBlockWithinStairsNorth;
            CardinalDirection cardinal = Utils.GetCardinal(player.rotation.eulerAngles.y);
            switch (cardinal)
            {
                case CardinalDirection.East:
                    isBlockWithinStairs = IsBlockWithinStairsEast;
                    break;
                case CardinalDirection.South:
                    isBlockWithinStairs = IsBlockWithinStairsSouth;
                    break;
                case CardinalDirection.West:
                    isBlockWithinStairs = IsBlockWithinStairsWest;
                    break;
            }

            Vector3Int temp = Vector3Int.zero;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        if (!isBlockWithinStairs(width, height, depth, x, y, z))
                            continue;

                        temp.x = x;
                        temp.y = y;
                        temp.z = z;
                        temp += a;

                        world.ChangeBlock(temp, add);
                    }
                }
            }
        }
        
        private bool IsBlockWithinStairsNorth(int width, int height, int depth, int x, int y, int z)
        {
            float slope = (float)height / depth;
            int max = Mathf.CeilToInt(z * slope);
            return y < max;
        }

        private bool IsBlockWithinStairsEast(int width, int height, int depth, int x, int y, int z)
        {
            float slope = (float)height / width;
            int max = Mathf.CeilToInt(x * slope);
            return y < max;
        }

        private bool IsBlockWithinStairsSouth(int width, int height, int depth, int x, int y, int z)
        {
            // INVERT.
            z = depth - 1 - z;
            return IsBlockWithinStairsNorth(width, height, depth, x, y, z);
        }

        private bool IsBlockWithinStairsWest(int width, int height, int depth, int x, int y, int z)
        {
            // INVERT.
            x = width - 1 - x;
            return IsBlockWithinStairsEast(width, height, depth, x, y, z);
        }
    }
}
