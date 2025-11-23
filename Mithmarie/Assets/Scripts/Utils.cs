using UnityEngine;

namespace Mithmarie
{
    /// <summary>
    ///  THIS MUST BE MADE UTILS REPO !!
    /// </summary>
    public static class Utils
    {
        public static Vector3 UpForward { get; } = new Vector3(0, 1, 1);
        public static Vector3 UpRight { get; } = new Vector3(1, 1, 0);
        public static Vector3 ForwardRight { get; } = new Vector3(1, 0, 1);

        public static Vector3Int ConvertToBlockPos(Vector3 pos) => new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
        public static bool IsStringValid(string str) => !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
        public static void ConvertToBlockPos(ref Vector3 pos) => pos = ConvertToBlockPos(pos);
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
