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
    /// https://en.wikipedia.org/wiki/Wavefront_.obj_file
    /// </summary>
    public class OBJ : IExportStrategy
    {
        public void Export(string path, MeshData mesh, IMessageService message)
        {
            try
            {
                using StreamWriter writer = new StreamWriter(path);
                writer.Write(FormatObj(mesh));
            }
            catch (Exception exception)
            {
                message.Send(exception.Message, Color.red);
            }
        }

        public void ExportAsChunks(string path, List<MeshData> meshes, IMessageService message)
        {
            try
            {
                using StreamWriter writer = new StreamWriter(path);
                writer.Write(FormatMultipleObjs(meshes));
            }
            catch (Exception exception)
            {
                message.Send(exception.Message, Color.red);
            }
        }

        public string GetShortName() => "obj";
        public string GetWholeName() => "Wavefront";

        private string FormatObj(MeshData mesh)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"o {mesh.name}");

            foreach (Vector3 vert in mesh.verts)
            {
                builder.AppendLine(string.Format("v {0} {1} {2}", vert.x, vert.y, vert.z));
            }

            // I DON'T INCLUDE NORMALS ON PURPOSE HERE 
            // BECAUSE BLENDER AND UNITY CALCULATE THEM AUTOMATICALLY.

            foreach (Vector2 uv in mesh.uvs)
            {
                builder.AppendLine(string.Format("vt {0} {1}", uv.x, uv.y));
            }

            for (int j = 0; j < mesh.tris.Count; j += 3)
            {
                builder.AppendLine(
                    string.Format("f {0}/{0} {1}/{1} {2}/{2}",
                    mesh.tris[j] + 1,
                    mesh.tris[j + 1] + 1,
                    mesh.tris[j + 2] + 1)
                );
            }

            return builder.ToString();
        }

        private string FormatMultipleObjs(List<MeshData> meshes)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            StringBuilder builder = new StringBuilder();

            int vertOffset = 0;
            List<int> triangles = new List<int>();
            Dictionary<int, MeshData> subMeshStarts = new Dictionary<int, MeshData>();
            builder.AppendLine($"o obj_level");

            foreach (MeshData mesh in meshes)
            {
                foreach (Vector3 vert in mesh.verts)
                {
                    builder.AppendLine(string.Format("v {0} {1} {2}", vert.x, vert.y, vert.z));
                }
                Debug.Log(mesh.verts.Count);
                Debug.Log(mesh.tris.Count);
                subMeshStarts.Add(triangles.Count, mesh);
                foreach (int tri in mesh.tris)
                {
                    triangles.Add(vertOffset + tri);
                }

                vertOffset += mesh.verts.Count;
            }

            foreach (MeshData mesh in meshes)
            {
                foreach (Vector2 uv in mesh.uvs)
                {
                    builder.AppendLine(string.Format("vt {0} {1}", uv.x, uv.y));
                }
            }

            for (int i = 0; i < triangles.Count; i += 3)
            {
                if(subMeshStarts.ContainsKey(i))
                    builder.AppendLine().AppendLine(string.Format("g {0}", subMeshStarts[i].name));

                builder.AppendLine(string.Format (
                    "f {0}/{0} {1}/{1} {2}/{2}",
                    triangles[i] + 1,
                    triangles[i + 1] + 1,
                    triangles[i + 2] + 1)
                );
            }

            return builder.ToString();
        }
    }
}