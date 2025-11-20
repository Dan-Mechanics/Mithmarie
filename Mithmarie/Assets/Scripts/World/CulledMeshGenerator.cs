using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    /// <summary>
    /// This class is responsible for generating the mesh, i could make another script that actually generates the perinl
    /// and such. Cool idea: classes talk tuah eachother via interfaces.
    /// </summary>
    public class CulledMeshGenerator : MonoBehaviour, IMeshGeneratable
    {
        [SerializeField] private MeshFilter filter = default;
        [SerializeField] private MeshCollider coll = default;

        private readonly MeshColliderCookingOptions cookingOptions = MeshColliderCookingOptions.UseFastMidphase;
        private Mesh mesh;
        private int[] triangles;

        private readonly List<Vector3> verts = new List<Vector3>();
        private readonly List<int> tris = new List<int>();
        private readonly List<Vector2> uvs = new List<Vector2>();


        /// <summary>
        /// https://github.com/samhogan/Minecraft-Unity3D/blob/master/Assets/Scripts/TerrainChunk.cs
        /// </summary>
        private void Start()
        {
            // INIT THE MESH.
            mesh = new Mesh();
            filter.mesh = mesh;
            coll.sharedMesh = mesh;

            coll.cookingOptions = cookingOptions;
            mesh.MarkDynamic();
        }

        private void GenerateMesh()
        {
            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            for (int z = 0; z <= terrainable.GetSize(); z++)
            {
                for (int x = 0; x <= terrainable.GetSize(); x++)
                {
                    terrainable.SetHeightStartup(x, ref height, z);
                    verticies[i] = new Vector3(x, height, z);

                    height = 0f;
                    i++;
                }
            }

            triangles = new int[terrainable.GetSize() * terrainable.GetSize() * 6];

            int vert = 0;
            int tris = 0;

            for (int z = 0; z < terrainable.GetSize(); z++)
            {
                for (int x = 0; x < terrainable.GetSize(); x++)
                {
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + terrainable.GetSize() + 1;
                    triangles[tris + 2] = vert + 1;

                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + terrainable.GetSize() + 1;
                    triangles[tris + 5] = vert + terrainable.GetSize() + 2;

                    vert++;
                    tris += 6;
                }

                vert++;
            }

            mesh.Clear();

            mesh.vertices = verts.ToArray();
            mesh.triangles = tris.ToArray();
            mesh.uv = uvs.ToArray();

            mesh.RecalculateNormals();
            Physics.BakeMesh(mesh.GetInstanceID(), false, cookingOptions);
           // coll.sharedMesh = mesh;
        }

        public void GenerateMesh(HashSet<Vector3Int> hash)
        {
            verts.Clear();
            tris.Clear();
            uvs.Clear();

            for (int z = 0; z <= terrainable.GetSize(); z++)
            {
                for (int x = 0; x <= terrainable.GetSize(); x++)
                {
                    terrainable.SetHeightStartup(x, ref height, z);
                    verticies[i] = new Vector3(x, height, z);

                    height = 0f;
                    i++;
                }
            }

            triangles = new int[terrainable.GetSize() * terrainable.GetSize() * 6];

            int vert = 0;
            int tris = 0;

            for (int z = 0; z < terrainable.GetSize(); z++)
            {
                for (int x = 0; x < terrainable.GetSize(); x++)
                {
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + terrainable.GetSize() + 1;
                    triangles[tris + 2] = vert + 1;

                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + terrainable.GetSize() + 1;
                    triangles[tris + 5] = vert + terrainable.GetSize() + 2;

                    vert++;
                    tris += 6;
                }

                vert++;
            }

            mesh.Clear();

            mesh.vertices = verts.ToArray();
            mesh.triangles = tris.ToArray();
            mesh.uv = uvs.ToArray();

            mesh.RecalculateNormals();
            Physics.BakeMesh(mesh.GetInstanceID(), false, cookingOptions);
            // coll.sharedMesh = mesh;
        }
    }
}