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
    
    public static class Utils
    {
        public static Vector3Int GetBlockPos(Vector3 pos) => new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
        public static bool IsStringValid(string str) => !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);

        public static Vector3Int GetChunkPos(Vector3Int blockPos, int chunkSize)
        {
            return new Vector3Int(
                blockPos.x / chunkSize,
                blockPos.y / chunkSize,
                blockPos.z / chunkSize
            );
        }

        public static bool IsInEllipse(Vector3 direction, float width, float height)
        {
            return GetEllipseMagnitude(direction, width, height) <= width;
        }

        public static float GetEllipseMagnitude(Vector3 direction, float width, float height)
        {
            float relativeHeight = height / width;

            direction.z /= relativeHeight;
            return direction.magnitude;
        }

        public static int PosToIndex(int x, int z, int width)
        {
            if (width < 1)
                return 0;

            return x + z * width;
        }

        public static float RoundToDecimalPlaces(float value, int decimalPlaces)
        {
            if (decimalPlaces < 1)
                return Mathf.Round(value);

            float precision = 10f * decimalPlaces;
            return Mathf.Round(value * precision) / precision;
        }

        public static Vector3 RoundVector(Vector3 vector)
        {
            return new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
        }

        public static Vector3 RoundVector(Vector3 vector, int decimalPlaces)
        {
            return new Vector3(
                RoundToDecimalPlaces(vector.x, decimalPlaces),
                RoundToDecimalPlaces(vector.y, decimalPlaces),
                RoundToDecimalPlaces(vector.z, decimalPlaces)
            );
        }

        public static string SetRichTextColor(string str, string color) => $"<color={color}>{str}</color>";
        public static string MakeGreen(string str) => SetRichTextColor(str, "green");
        public static string MakeRed(string str) => SetRichTextColor(str, "red");
        public static string MakeBlue(string str) => SetRichTextColor(str, "blue");
    }
}
