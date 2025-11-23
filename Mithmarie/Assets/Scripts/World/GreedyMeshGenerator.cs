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
    public class GreedyMeshGenerator : IMeshingStrategy
    {
        public Mesh GenerateMesh(HashSet<Vector3Int> blocks)
        {
            List<ExpandingCubeMesh> cubes = ExtractCubes(blocks.ToList());

            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            cubes.ForEach(x => x.AddSelfToMesh(verts, tris, uvs, blocks));

            Mesh mesh = new Mesh
            {
                vertices = verts.ToArray(),
                triangles = tris.ToArray(),
                uv = uvs.ToArray()
            };

            mesh.RecalculateNormals();
            return mesh;
        }

        private List<ExpandingCubeMesh> ExtractCubes(List<Vector3Int> blocksLeft)
        {
            List<ExpandingCubeMesh> result = new List<ExpandingCubeMesh>();

            while(blocksLeft.Count > 0)
            { 
                ExpandingCubeMesh cube = new ExpandingCubeMesh(blocksLeft[0]);
                blocksLeft.RemoveAt(0);

                cube.ExpandForward(blocksLeft);
                cube.ExpandBack(blocksLeft);

                cube.ExandRight(blocksLeft);
                cube.ExpandLeft(blocksLeft);

                cube.ExpandUp(blocksLeft);
                cube.ExpandDown(blocksLeft);

                result.Add(cube);
            }

            return result;
        }
    }
}