using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    /// <summary>
    /// We don't use UnityEngine.Mesh here because
    /// Unity doesn't allow it with the multithreading.
    /// </summary>
    public class MeshData
    {
        public string name;
        public List<Vector3> verts;
        public List<int> tris;
        public List<Vector2> uvs;

        public Mesh GetMesh()
        {
            Mesh mesh = new Mesh();
            mesh.name = name;
            mesh.vertices = verts.ToArray();
            mesh.triangles = tris.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
