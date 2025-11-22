using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mithmarie
{
    /// <summary>
    /// Sources: https://www.reddit.com/r/VoxelGameDev/comments/cmwqwy/whats_the_simplest_greedy_meshing_example_with/
    /// https://github.com/VictorGordan/opengl-tutorials/blob/main/YoutubeOpenGL%209%20-%20Lighting/Main.cpp
    /// https://pastebin.com/DXKEmvap
    /// </summary>
    public class GreedyMeshGenerator : IGenerateMeshStrat
    {
        public Mesh GenerateMesh(HashSet<Vector3Int> blocks)
        {
            List<ExpandingCubeMesh> cubes = ExtractCubes(blocks);

            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            for (int i = 0; i < cubes.Count; i++)
            {
                cubes[i].AddSelfToMesh(verts, tris, uvs, HashSet<Vector3Int> blocks);
            }

            Mesh mesh = new Mesh
            {
                vertices = verts.ToArray(),
                triangles = tris.ToArray(),
                uv = uvs.ToArray()
            };

            mesh.RecalculateNormals();
            return mesh;
        }

        private List<ExpandingCubeMesh> ExtractCubes(HashSet<Vector3Int> blocks)
        {
            List<ExpandingCubeMesh> result = new List<ExpandingCubeMesh>();
            List<Vector3Int> blocksLeft = blocks.ToList();

            while(blocksLeft.Count > 0)
            { 
                ExpandingCubeMesh cube = new ExpandingCubeMesh(blocksLeft[0]);
                blocksLeft.RemoveAt(0);

                cube.ExandRight(blocksLeft);
                cube.ExpandLeft(blocksLeft);

                cube.ExandUp(blocksLeft);
                cube.ExpandDown(blocksLeft);

                cube.ExpandForward(blocksLeft);
                cube.ExpandBack(blocksLeft);

                result.Add(cube);
            }

            return result;
        }
    }
}