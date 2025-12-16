using System;
using System.Collections.Generic;
using UnityEditor.Formats.Fbx.Exporter;
using UnityEngine;
using Autodesk.Fbx;

namespace Mithmarie
{

    public static class Test
    {
        /*public static void ExportScene(string fileName)
        {
            using (FbxManager fbxManager = FbxManager.Create())
            {
                // configure IO settings.
                fbxManager.SetIOSettings(FbxIOSettings.Create(fbxManager, Globals.IOSROOT));

                // Export the scene
                using (FbxExporter exporter = FbxExporter.Create(fbxManager, "myExporter"))
                {

                    // Initialize the exporter.
                    bool status = exporter.Initialize(fileName, -1, fbxManager.GetIOSettings());

                    // Create a new scene to export
                    *//*FbxScene scene = FbxScene.Create(fbxManager, "myScene");
                    FbxGeometry fbx = FbxGeometry.Create(fbxManager, "wd");
                    fbx.
                    FbxMesh mesh = FbxMesh.Create(fbxManager, "mesh");
                    FbxImplementation implementation = FbxImplementation.Create();
                    implementation
                    mesh.add();
                    FbxCluster wd = FbxCluster.Create(fbxManager, "");
                    wd.poly*//*

                    scene.AddMember(mesh);

                    // Export the scene to the file.
                    exporter.Export(scene);
                }
            }
        }*/
    }
}