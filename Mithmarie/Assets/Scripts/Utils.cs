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

    public enum Axis
    {
        X = 0,
        Y = 1,
        Z = 2
    }

    public static class Utils
    {
        public static Vector3Int GetBlockPos(Vector3 pos) => new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
        public static bool IsStringValid(string str) => !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);

        public static Vector3Int GetChunkPos(Vector3Int blockPos, int chunkSize)
        {
            return new Vector3Int(
                Mathf.FloorToInt((float)blockPos.x / chunkSize),
                Mathf.FloorToInt((float)blockPos.y / chunkSize),
                Mathf.FloorToInt((float)blockPos.z / chunkSize));
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
                RoundToDecimalPlaces(vector.z, decimalPlaces));
        }

        public static void SwapAxis(ref Vector3Int a, ref Vector3Int b, Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    int aX = a.x;
                    int bX = b.x;
                    a.x = bX;
                    b.x = aX;
                    break;
                case Axis.Y:
                    int aY = a.y;
                    int bY = b.y;
                    a.y = bY;
                    b.y = aY;
                    break;
                case Axis.Z:
                    int aZ = a.z;
                    int bZ = b.z;
                    a.z = bZ;
                    b.z = aZ;
                    break;
                default:
                    break;
            }
        }

        public static string MakeRed(string str) => SetRichTextColor(str, "red");
        public static string MakeGreen(string str) => SetRichTextColor(str, "green");
        public static string MakeBlue(string str) => SetRichTextColor(str, "blue");
        public static string SetRichTextColor(string str, string color) => $"<color={color}>{str}</color>";

        public static CardinalDirection GetCardinal(float rot)
        {
            CardinalDirection direction = CardinalDirection.North;
            float angle = 22.5f;
            if (rot > angle)
                direction = CardinalDirection.East;

            if (rot > 90f + angle)
                direction = CardinalDirection.South;

            if (rot > 180f + angle)
                direction = CardinalDirection.West;

            if (rot > 270f + angle)
                direction = CardinalDirection.North;

            return direction;
        }
    }
}
