using System;
using System.Collections.Generic;
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
                writer.Write(GetMeshOBJ(mesh));
            }
            catch (Exception exception)
            {
                message.Send(exception.Message, Color.red);
            }
        }

        public void ExportAsChunks(string path, List<Mesh> meshes, IMessageService message)
        {
            try
            {
                using StreamWriter writer = new StreamWriter(path);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < meshes.Count; i++)
                {
                    builder.AppendLine(GetMeshOBJ(meshes[i]));
                }

                writer.Write(builder.ToString());
            }
            catch (Exception exception)
            {
                message.Send(exception.Message, Color.red);
            }
        }

        public string GetShortName() => "obj";
        public string GetWholeName() => "Wavefront";

        private string GetMeshOBJ(Mesh mesh)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"o {mesh.name}");

            foreach (Vector3 vert in mesh.vertices)
            {
                builder.AppendLine(string.Format("v {0} {1} {2}", vert.x, vert.y, vert.z));
            }

            /*foreach (Vector3 normal in mesh.normals)
            {
                builder.AppendLine(string.Format("vn {0} {1} {2}", normal.z, normal.y, normal.x));
            }*/

            foreach (Vector2 uv in mesh.uv)
            {
                builder.AppendLine(string.Format("vt {0} {1}", uv.x, uv.y));
            }

            //builder.AppendLine("s 1");
            //builder.AppendLine("s off");

            /*for (int i = 0; i < mesh.subMeshCount; i++)
            {
                builder.Append(string.Format("\ng {0}\n", name));
                int[] triangles = mesh.GetTriangles(i);
                for (int j = 0; j < triangles.Length; j += 3)
                {
                    builder.AppendLine(string.Format("f {0}/{0} {1}/{1} {2}/{2}",
                    triangles[j] + 1,
                    triangles[j + 1] + 1,
                    triangles[j + 2] + 1));
                }
            }*/

            for (int j = 0; j < mesh.triangles.Length; j += 3)
            {
                builder.AppendLine (
                    string.Format("f {0}/{0} {1}/{1} {2}/{2}",
                    mesh.triangles[j] + 1,
                    mesh.triangles[j + 1] + 1,
                    mesh.triangles[j + 2] + 1)
                );
            }

            return builder.ToString();
        }
    }
}