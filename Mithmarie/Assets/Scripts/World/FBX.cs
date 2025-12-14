using System;
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

        public string GetShortName() => "fbx";
        public string GetWholeName() => GetShortName().ToUpperInvariant();
    }
}