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
            List<Cube> cubes = ExtractCubes(blocks);

            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            for (int i = 0; i < cubes.Count; i++)
            {
                cubes[i].AddSelfToMesh(ref verts, ref tris, ref uvs);
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

        private List<Cube> ExtractCubes(HashSet<Vector3Int> blocksLeft)
        {
            List<Cube> result = new List<Cube>();

            Vector3Int[] blocks = blocksLeft.ToArray();
            Queue<Vector3Int> blocksToRemove = new Queue<Vector3Int>();

            for (int i = 0; i < blocks.Length; i++)
            {
                // THIS BLOCK DOESN'T EXIST ANYMORE ...
                if (!blocksLeft.Contains(blocks[i]))
                    continue;

                Cube cube = new Cube(blocks[i]);
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


        private class Cube
        {
            public int minX;
            public int maxX;
            public int minY;
            public int maxY;
            public int minZ;
            public int maxZ;

            public Cube(Vector3Int center)
            {
                minX = maxX = center.x;
                minY = maxY = center.y;
                minZ = maxZ = center.z;
            }

            public bool CanGoUp(HashSet<Vector3Int> blocksLeft, ref Queue<Vector3Int> blocksToRemove)
            {
                blocksToRemove.Clear();

                Vector3Int current = new Vector3Int(0, maxY + 1, 0);
                for (int x = minX; x <= maxX; x++)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        current.x = x;
                        current.z = z;
                        blocksToRemove.Enqueue(current);
                        if (!blocksLeft.Contains(current))
                            return false;
                    }
                }

                maxY = current.y;
                return true;
            }

            public bool CanGoDown(HashSet<Vector3Int> blocksLeft, ref Queue<Vector3Int> blocksToRemove)
            {
                blocksToRemove.Clear();

                Vector3Int current = new Vector3Int(0, minY - 1, 0);
                for (int x = minX; x <= maxX; x++)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        current.x = x;
                        current.z = z;
                        blocksToRemove.Enqueue(current);
                        if (!blocksLeft.Contains(current))
                            return false;
                    }
                }

                minY = current.y;
                return true;
            }

            public bool CanGoLeft(HashSet<Vector3Int> blocksLeft, ref Queue<Vector3Int> blocksToRemove)
            {
                blocksToRemove.Clear();

                Vector3Int current = new Vector3Int(minX - 1, 0, 0);
                for (int y = minY; y <= maxY; y++)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        current.y = y;
                        current.z = z;
                        blocksToRemove.Enqueue(current);
                        if (!blocksLeft.Contains(current))
                            return false;
                    }
                }

                minX = current.x;
                return true;
            }

            public bool CanGoRight(HashSet<Vector3Int> blocksLeft, ref Queue<Vector3Int> blocksToRemove)
            {
                blocksToRemove.Clear();

                Vector3Int current = new Vector3Int(maxX + 1, 0, 0);
                for (int y = minY; y <= maxY; y++)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        current.y = y;
                        current.z = z;
                        blocksToRemove.Enqueue(current);
                        if (!blocksLeft.Contains(current))
                            return false;
                    }
                }

                maxX = current.x;
                return true;
            }

            public bool CanGoForward(HashSet<Vector3Int> blocksLeft, ref Queue<Vector3Int> blocksToRemove)
            {
                blocksToRemove.Clear();

                Vector3Int current = new Vector3Int(0, 0, maxZ + 1);
                for (int x = minX; x <= maxX; x++)
                {
                    for (int y = minY; y <= maxY; y++)
                    {
                        current.y = y;
                        current.x = x;
                        blocksToRemove.Enqueue(current);
                        if (!blocksLeft.Contains(current))
                            return false;
                    }
                }

                maxZ = current.z;
                return true;
            }

            public bool CanGoBack(HashSet<Vector3Int> blocksLeft, ref Queue<Vector3Int> blocksToRemove)
            {
                blocksToRemove.Clear();

                Vector3Int current = new Vector3Int(0, 0, minZ - 1);
                for (int x = minX; x <= maxX; x++)
                {
                    for (int y = minY; y <= maxY; y++)
                    {
                        current.y = y;
                        current.x = x;
                        blocksToRemove.Enqueue(current);
                        if (!blocksLeft.Contains(current))
                            return false;
                    }
                }

                minZ = current.z;
                return true;
            }

            /*public void AddSelfToMesh(ref List<Vector3> verts, ref List<int> tris, ref List<Vector2> uvs, int offset)
            {
                minX--;
                minY--;
                minZ--;
                
                Vector3[] cubeVerts = new Vector3[]
                {
                    new Vector3(minX, minY, minZ),
                    new Vector3(maxX, minY, minZ),
                    new Vector3(maxX, maxY, minZ),
                    new Vector3(minX, maxY, minZ),
                    new Vector3(minX, minY, maxZ),
                    new Vector3(maxX, minY, maxZ),
                    new Vector3(maxX, maxY, maxZ),
                    new Vector3(minX, maxY, maxZ)
                };

                int[] cubeTris =
                {
                    0, 1, 3, 3, 1, 2,
                    1, 5, 2, 2, 5, 6,
                    5, 4, 6, 6, 4, 7,
                    4, 0, 7, 7, 0, 3,
                    3, 2, 7, 7, 2, 6,
                    4, 5, 0, 0, 5, 1
                };

                Vector2[] cubeUvs =
                {
                    new Vector2(0, 0),
                    new Vector2(1, 0),
                    new Vector2(1, 1),
                    new Vector2(0, 1)
                };

                verts.AddRange(cubeVerts);
                //tris.AddRange(cubeTris);

                for (int i = 0; i < cubeTris.Length; i++)
                {
                    tris.Add(cubeTris[i] + offset);
                }

                uvs.AddRange(cubeUvs);
                uvs.AddRange(cubeUvs);
            }*/

            public void AddSelfToMesh(ref List<Vector3> verts, ref List<int> tris, ref List<Vector2> uvs)
            {
                maxX++;
                maxY++;
                maxZ++;
                
                int[] tempTris = new int[6];
                int faceCount = 6;
                int offset = verts.Count;

                verts.Add(new Vector3(0, 1, 0));
                verts.Add(new Vector3(0, 1, 1));
                verts.Add(new Vector3(1, 1, 1));
                verts.Add(new Vector3(1, 1, 0));
                verts.Add(new Vector3(0, 0, 0));
                verts.Add(new Vector3(1, 0, 0));
                verts.Add(new Vector3(1, 0, 1));
                verts.Add(new Vector3(0, 0, 1));
                verts.Add(new Vector3(0, 0, 0));
                verts.Add(new Vector3(0, 1, 0));
                verts.Add(new Vector3(1, 1, 0));
                verts.Add(new Vector3(1, 0, 0));
                verts.Add(new Vector3(1, 0, 0));
                verts.Add(new Vector3(1, 1, 0));
                verts.Add(new Vector3(1, 1, 1));
                verts.Add(new Vector3(1, 0, 1));
                verts.Add(new Vector3(1, 0, 1));
                verts.Add(new Vector3(1, 1, 1));
                verts.Add(new Vector3(0, 1, 1));
                verts.Add(new Vector3(0, 0, 1));
                verts.Add(new Vector3(0, 0, 1));
                verts.Add(new Vector3(0, 1, 1));
                verts.Add(new Vector3(0, 1, 0));
                verts.Add(new Vector3(0, 0, 0));

                for (int i = 0; i < faceCount; i++)
                {
                    tempTris[0] = offset + i * 4;
                    tempTris[1] = offset + i * 4 + 1;
                    tempTris[2] = offset + i * 4 + 2;

                    tempTris[3] = offset + i * 4;
                    tempTris[4] = offset + i * 4 + 2;
                    tempTris[5] = offset + i * 4 + 3;

                    tris.AddRange(tempTris);
                    uvs.AddRange(CulledMeshGenerator.faceUvs);
                }
            }
        }
    }
}