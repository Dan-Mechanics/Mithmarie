using UnityEngine;

namespace Mithmarie
{
    public enum CardinalDirection
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }
    
    /// <summary>
    ///  THIS MUST BE MADE UTILS REPO !!
    /// </summary>
    public static class Utils
    {
        
        public static Vector3Int GetBlockPos(Vector3 pos) => new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
        public static bool IsStringValid(string str) => !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
        public static Vector3Int GetChunkPos(Vector3Int blockPos, int chunkSize)
        {
            return new Vector3Int(blockPos.x / chunkSize,
                blockPos.y / chunkSize,
                blockPos.z / chunkSize
            );
        }

        public static bool IsInElipse(Vector3 direction, float width, float height)
        {
            float relativeHeight = height / width;

            direction.z /= relativeHeight;
            return direction.magnitude <= width;
        }

        public static void IntroduceTool(World world, IMessageService message)
        {
            world.Add(Vector3Int.zero);
            world.Flush();
            message.Send("[WASD] for movement and [MOUSE] for looking.\n" +
                "Use [RMB] to place blocks, [LMB] to destroy.", Color.black, 4f);
        }
        // possibly add things like:
        // snap to grid or round to decimal or like string formatting for unity debug color
        // or like array to 3D space with the width of the plane as index type beat
        // or useful memes like that.
    }
}
