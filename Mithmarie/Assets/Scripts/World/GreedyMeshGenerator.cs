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
    public class GreedyMeshGenerator : MonoBehaviour, IGenerateMeshStrat
    {
        public GameObject cubePrefab;
        
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
                while (cube.CanGoRight(blocksLeft, blocksToRemove))
                {
                    while (blocksToRemove.Count > 0)
                        blocksLeft.Remove(blocksToRemove.Dequeue());
                }

                blocksToRemove.Clear();
                while (cube.CanGoLeft(blocksLeft, blocksToRemove))
                {
                    while (blocksToRemove.Count > 0)
                        blocksLeft.Remove(blocksToRemove.Dequeue());
                }

                blocksToRemove.Clear();
                while (cube.CanGoForward(blocksLeft, blocksToRemove))
                {
                    while (blocksToRemove.Count > 0)
                        blocksLeft.Remove(blocksToRemove.Dequeue());
                }

                blocksToRemove.Clear();
                while (cube.CanGoBack(blocksLeft, blocksToRemove))
                {
                    while (blocksToRemove.Count > 0)
                        blocksLeft.Remove(blocksToRemove.Dequeue());
                }

                blocksToRemove.Clear();
                while (cube.CanGoUp(blocksLeft, blocksToRemove))
                {
                    while (blocksToRemove.Count > 0)
                        blocksLeft.Remove(blocksToRemove.Dequeue());
                }

                blocksToRemove.Clear();
                while (cube.CanGoDown(blocksLeft, blocksToRemove))
                {
                    while (blocksToRemove.Count > 0)
                        blocksLeft.Remove(blocksToRemove.Dequeue());
                }

                cube.SpawnDemoCube(cubePrefab);
                result.Add(cube);
            }

            return result;
        }
    }
}