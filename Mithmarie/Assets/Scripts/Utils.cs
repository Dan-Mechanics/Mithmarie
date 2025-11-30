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
            return new Vector3Int(
                Mathf.FloorToInt(blockPos.x / (float)chunkSize),
                Mathf.FloorToInt(blockPos.y / (float)chunkSize),
                Mathf.FloorToInt(blockPos.z / (float)chunkSize)
                );
        }
    }
}
