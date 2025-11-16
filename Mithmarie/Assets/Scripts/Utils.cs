using System.IO;
using System.Text;
using UnityEngine;

namespace Mithmarie
{
    /// <summary>
    ///  THIS MUST BE MADE UTILS REPO
    /// </summary>
    public static class Utils
    {
        public static Vector3Int ApplyGrid(Vector3 pos)
        {
            return new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
        }

        public static bool IsStringValid(string str) => !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);

        public static void ApplyGrid(ref Vector3 pos) => pos = ApplyGrid(pos);

        /// <summary>
        /// We are using ref here to be verbose.
        /// https://discussions.unity.com/t/export-unity-mesh-to-obj-or-fbx-format/525773/14
        /// </summary>
        public static void ExportToOBJ(Mesh mesh, ref BinaryWriter writer)
        {
            StringBuilder builder = new StringBuilder();

            foreach (Vector3 vert in mesh.vertices)
            {
                builder.Append(string.Format("v {0} {1} {2}\n", vert.x, vert.y, vert.z));
            }

            foreach (Vector3 norm in mesh.normals)
            {
                builder.Append(string.Format("vn {0} {1} {2}\n", norm.x, norm.y, norm.z));
            }

            for (int material = 0; material < mesh.subMeshCount; material++)
            {
                Debug.Log(mesh.name);
                builder.Append(string.Format("\ng {0}\n", mesh.name));
                int[] triangles = mesh.GetTriangles(material);
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    builder.Append(string.Format("f {0}/{0} {1}/{1} {2}/{2}\n",
                    triangles[i] + 1,
                    triangles[i + 1] + 1,
                    triangles[i + 2] + 1));
                }
            }

            writer.Write(builder.ToString());
        }
    }
}
