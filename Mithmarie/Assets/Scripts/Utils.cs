using UnityEngine;

namespace Mithmarie
{
    /// <summary>
    ///  THIS MUST BE MADE UTILS REPO !!
    /// </summary>
    public static class Utils
    {
        public static Vector3Int ApplyGrid(Vector3 pos) => new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
        public static bool IsStringValid(string str) => !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
        public static void ApplyGrid(ref Vector3 pos) => pos = ApplyGrid(pos);
    }
}
