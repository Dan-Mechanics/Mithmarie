using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mithmarie
{
    public class ExpandingCubesWorldMesh : IWorldMeshStrategy
    {
        public Mesh GenerateMesh(HashSet<Vector3Int> blocks)
        {
            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            Mesh mesh = new Mesh();

            MeshUtils.GenerateExpandingCubesMesh(blocks, verts, tris, uvs);

            mesh.vertices = verts.ToArray();
            mesh.triangles = tris.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}