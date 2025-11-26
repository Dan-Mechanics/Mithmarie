using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Mithmarie
{
    /// <summary>
    /// https://discussions.unity.com/t/export-unity-mesh-to-obj-or-fbx-format/525773/14
    /// </summary>
    public class OBJ : IExportStrategy
    {
        public void Export(string path, Mesh mesh, IMessageService message)
        {
            try
            {
                using StreamWriter writer = new StreamWriter(path);
                writer.Write(GetMeshOBJ(mesh.name, mesh, Matrix4x4.identity));
            }
            catch (Exception exception)
            {
                //Debug.LogError(exception.Message);
                message.Send(exception.Message, Color.red);
            }
        }

        private string GetMeshOBJ(string name, Mesh mesh, Matrix4x4 objTransform)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            StringBuilder builder = new StringBuilder();

            foreach (Vector3 v in mesh.vertices)
            {
                Vector3 writeV = (objTransform != Matrix4x4.identity && objTransform != default) ? objTransform.MultiplyPoint(v) : v;
                builder.Append(string.Format("v {0} {1} {2}\n", writeV.x, writeV.y, writeV.z));
            }

            // Also export UV's
            foreach (Vector3 v in mesh.uv)
            {
                builder.Append(string.Format("vt {0} {1} {2}\n", v.x, v.y, v.z));
            }

            foreach (Vector3 v in mesh.normals)
            {
                builder.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z));
            }

            for (int material = 0; material < mesh.subMeshCount; material++)
            {
                builder.Append(string.Format("\ng {0}\n", name));
                int[] triangles = mesh.GetTriangles(material);
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    builder.Append(string.Format("f {0}/{0} {1}/{1} {2}/{2}\n",
                    triangles[i] + 1,
                    triangles[i + 1] + 1,
                    triangles[i + 2] + 1));
                }
            }

            return builder.ToString();
        }
    }
}