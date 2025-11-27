using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mithmarie
{
    /// <summary>
    /// https://www.reddit.com/r/VoxelGameDev/comments/cmwqwy/whats_the_simplest_greedy_meshing_example_with/
    /// https://github.com/VictorGordan/opengl-tutorials/blob/main/YoutubeOpenGL%209%20-%20Lighting/Main.cpp
    /// https://pastebin.com/DXKEmvap
    /// </summary>
    public class InflateWorldMesh : IWorldMeshStrategy
    {
        public Mesh GenerateMesh(HashSet<Vector3Int> blocks)
        {
            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            Mesh mesh = new Mesh();

            List<InflatableCube> cubes = ExtractCubes(blocks.ToList());
            cubes.ForEach(x => x.AddSelfToMesh(verts, tris, uvs, blocks));

            mesh.vertices = verts.ToArray();
            mesh.triangles = tris.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.RecalculateNormals();

            return mesh;
        }

        private List<InflatableCube> ExtractCubes(List<Vector3Int> blocksLeft)
        {
            List<InflatableCube> result = new List<InflatableCube>();

            while(blocksLeft.Count > 0)
            { 
                InflatableCube cube = new InflatableCube(blocksLeft[0]);
                blocksLeft.RemoveAt(0);

                cube.ExpandUp(blocksLeft);
                cube.ExpandDown(blocksLeft);

                cube.ExandRight(blocksLeft);
                cube.ExpandLeft(blocksLeft);

                cube.ExpandForward(blocksLeft);
                cube.ExpandBack(blocksLeft);

                result.Add(cube);
            }

            return result;
        }
    }
}