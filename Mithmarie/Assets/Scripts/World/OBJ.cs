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
                writer.Write(GetMeshesOBJ(meshes));
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

            foreach (Vector2 uv in mesh.uv)
            {
                builder.AppendLine(string.Format("vt {0} {1}", uv.x, uv.y));
            }

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

        private string GetMeshesOBJ(List<Mesh> meshes)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            StringBuilder builder = new StringBuilder();

            int offset = 0;
            List<int> triangles = new List<int>();
            Dictionary<int, Mesh> subMeshPoints = new Dictionary<int, Mesh>();
            builder.AppendLine($"o obj_level");

            foreach (Mesh mesh in meshes)
            {
                foreach (Vector3 vert in mesh.vertices)
                {
                    builder.AppendLine(string.Format("v {0} {1} {2}", vert.x, vert.y, vert.z));
                }

                subMeshPoints.Add(triangles.Count, mesh);
                foreach (int tri in mesh.triangles)
                {
                    triangles.Add(offset + tri);
                }

                offset += mesh.vertices.Length;
            }

            foreach (Mesh mesh in meshes)
            {
                foreach (Vector2 uv in mesh.uv)
                {
                    builder.AppendLine(string.Format("vt {0} {1}", uv.x, uv.y));
                }
            }

            for (int j = 0; j < triangles.Count; j += 3)
            {
                if(subMeshPoints.ContainsKey(j))
                    builder.Append(string.Format("\ng {0}\n", subMeshPoints[j].name));

                builder.AppendLine(string.Format (
                    "f {0}/{0} {1}/{1} {2}/{2}",
                    triangles[j] + 1,
                    triangles[j + 1] + 1,
                    triangles[j + 2] + 1)
                );
            }

            return builder.ToString();
        }
    }
}