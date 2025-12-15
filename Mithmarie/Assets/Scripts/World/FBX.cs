using System;
using System.Collections.Generic;
using UnityEditor.Formats.Fbx.Exporter;
using UnityEngine;

namespace Mithmarie
{
    /// <summary>
    /// https://docs.unity3d.com/Packages/com.unity.formats.fbx@2.0/manual/devguide.html
    /// </summary>
    public class FBX : IExportStrategy
    {
        public void Export(string path, Mesh mesh, IMessageService message)
        {
            try
            {
                GameObject go = new GameObject(mesh.name);
                go.AddComponent<MeshRenderer>();
                go.AddComponent<MeshFilter>().sharedMesh = mesh;

                ExportModelOptions options = new ExportModelOptions();
                options.ExportFormat = ExportFormat.Binary;

                ModelExporter.ExportObject(path, go, options);

                UnityEngine.Object.Destroy(go);
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
                GameObject parent = new GameObject("fbx_level");
                for (int i = 0; i < meshes.Count; i++)
                {
                    Mesh mesh = meshes[i];
                    GameObject submesh = new GameObject(mesh.name);
                    submesh.AddComponent<MeshFilter>().sharedMesh = mesh;
                    submesh.transform.SetParent(parent.transform);
                }

                ExportModelOptions options = new ExportModelOptions();
                options.ExportFormat = ExportFormat.Binary;

                ModelExporter.ExportObject(path, parent, options);

                UnityEngine.Object.Destroy(parent);
            }
            catch (Exception exception)
            {
                message.Send(exception.Message, Color.red);
            }
        }

        public string GetShortName() => "fbx";
        public string GetWholeName() => GetShortName().ToUpperInvariant();
    }
}