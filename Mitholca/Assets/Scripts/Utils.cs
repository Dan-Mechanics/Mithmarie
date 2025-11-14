using UnityEngine;

namespace Mitholca
{
    /// <summary>
    ///  THIS MUST BE MADE UTILS REPO
    /// </summary>
    public static class Utils
    {
        public static Vector3 ApplyGrid(Vector3 pos)
        {
            return new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
        }

        public static void ApplyGrid(ref Vector3 pos)
        {
            pos = ApplyGrid(pos);
        }
    }
}
