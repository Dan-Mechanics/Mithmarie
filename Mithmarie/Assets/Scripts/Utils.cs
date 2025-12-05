using UnityEngine;

namespace Mithmarie
{
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

        // possibly add things like:
        // snap to grid or round to decimal or like string formatting for unity debug color
        // or like array to 3D space with the width of the plane as index type beat
        // or useful memes like that.
    }
}
