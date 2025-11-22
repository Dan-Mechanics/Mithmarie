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
                cubes[i].AddSelfToMesh(verts, tris, uvs);
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

        private List<ExpandingCubeMesh> ExtractCubes(HashSet<Vector3Int> blocksLeft)
        {
            List<ExpandingCubeMesh> result = new List<ExpandingCubeMesh>();

            Vector3Int[] blocks = blocksLeft.ToArray();
            Queue<Vector3Int> blocksToRemove = new Queue<Vector3Int>();

            for (int i = 0; i < blocks.Length; i++)
            {
                // THIS BLOCK DOESN'T EXIST ANYMORE ...
                if (!blocksLeft.Contains(blocks[i]))
                    continue;

                ExpandingCubeMesh cube = new ExpandingCubeMesh(blocks[i]);
                while (cube.CanGoRight(blocksLeft, ref blocksToRemove))
                {
                    while (blocksToRemove.Count > 0)
                        blocksLeft.Remove(blocksToRemove.Dequeue());
                }

                while (cube.CanGoLeft(blocksLeft, ref blocksToRemove))
                {
                    while (blocksToRemove.Count > 0)
                        blocksLeft.Remove(blocksToRemove.Dequeue());
                }

                while (cube.CanGoForward(blocksLeft, ref blocksToRemove))
                {
                    while (blocksToRemove.Count > 0)
                        blocksLeft.Remove(blocksToRemove.Dequeue());
                }

                while (cube.CanGoBack(blocksLeft, ref blocksToRemove))
                {
                    while (blocksToRemove.Count > 0)
                        blocksLeft.Remove(blocksToRemove.Dequeue());
                }

                while (cube.CanGoUp(blocksLeft, ref blocksToRemove))
                {
                    while (blocksToRemove.Count > 0)
                        blocksLeft.Remove(blocksToRemove.Dequeue());
                }

                while (cube.CanGoDown(blocksLeft, ref blocksToRemove))
                {
                    while (blocksToRemove.Count > 0)
                        blocksLeft.Remove(blocksToRemove.Dequeue());
                }

              //  cube.SpawnDemoVersion(cubePrefab);
                result.Add(cube);
            }

            return result;
        }
    }
}