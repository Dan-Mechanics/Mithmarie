using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mithmarie
{
    public static class MeshUtils
    {
        private static readonly Vector3 upForward = new Vector3(0f, 1f, 1f);
        private static readonly Vector3 upRight = new Vector3(1f, 1f, 0f);
        private static readonly Vector3 forwardRight = new Vector3(1f, 0f, 1f);

        private static readonly Vector2[] faceUvs = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0)
        };

        // TODO: ADD PROPER GREEDY MESHER.

        /// <summary>
        /// https://github.com/samhogan/Minecraft-Unity3D/blob/master/Assets/Scripts/TerrainChunk.cs
        /// Is this hasBlocks smart ??
        /// </summary>
        public static void GenerateCulledMesh(HashSet<Vector3Int> blocks, Predicate<Vector3Int> hasBlock, List<Vector3> verts, List<int> tris, List<Vector2> uvs)
        {
            verts.Clear();
            tris.Clear();
            uvs.Clear();

            foreach (Vector3Int pos in blocks)
            {
                int faceCount = 0;
                int vertIndexOffset = verts.Count;

                if (!hasBlock(pos + Vector3Int.up))
                {
                    verts.Add(pos + Vector3Int.up);
                    verts.Add(pos + upForward);
                    verts.Add(pos + Vector3Int.one);
                    verts.Add(pos + upRight);
                    faceCount++;
                }

                if (!hasBlock(pos + Vector3Int.down))
                {
                    verts.Add(pos + Vector3Int.zero);
                    verts.Add(pos + Vector3Int.right);
                    verts.Add(pos + forwardRight);
                    verts.Add(pos + Vector3Int.forward);
                    faceCount++;
                }

                if (!hasBlock(pos + Vector3Int.forward))
                {
                    verts.Add(pos + forwardRight);
                    verts.Add(pos + Vector3Int.one);
                    verts.Add(pos + upForward);
                    verts.Add(pos + Vector3Int.forward);
                    faceCount++;
                }

                if (!hasBlock(pos + Vector3Int.right))
                {
                    verts.Add(pos + Vector3Int.right);
                    verts.Add(pos + upRight);
                    verts.Add(pos + Vector3Int.one);
                    verts.Add(pos + forwardRight);
                    faceCount++;
                }

                if (!hasBlock(pos + Vector3Int.back))
                {
                    verts.Add(pos + Vector3Int.zero);
                    verts.Add(pos + Vector3Int.up);
                    verts.Add(pos + upRight);
                    verts.Add(pos + Vector3Int.right);
                    faceCount++;
                }

                if (!hasBlock(pos + Vector3Int.left))
                {
                    verts.Add(pos + Vector3Int.forward);
                    verts.Add(pos + upForward);
                    verts.Add(pos + Vector3Int.up);
                    verts.Add(pos + Vector3Int.zero);
                    faceCount++;
                }

                for (int i = 0; i < faceCount; i++)
                {
                    tris.Add(vertIndexOffset + i * 4);
                    tris.Add(vertIndexOffset + i * 4 + 1);
                    tris.Add(vertIndexOffset + i * 4 + 2);
                    tris.Add(vertIndexOffset + i * 4);
                    tris.Add(vertIndexOffset + i * 4 + 2);
                    tris.Add(vertIndexOffset + i * 4 + 3);

                    uvs.AddRange(faceUvs);
                }
            }
        }

        /// <summary>
        /// https://www.reddit.com/r/VoxelGameDev/comments/cmwqwy/whats_the_simplest_greedy_meshing_example_with/
        /// https://github.com/VictorGordan/opengl-tutorials/blob/main/YoutubeOpenGL%209%20-%20Lighting/Main.cpp
        /// https://pastebin.com/DXKEmvap
        /// </summary>
        public static void GenerateExpandingCubesMesh(HashSet<Vector3Int> blocks, List<Vector3> verts, List<int> tris, List<Vector2> uvs)
        {
            verts.Clear();
            tris.Clear();
            uvs.Clear();

            List<ExpandingCubeMesh> expandingCubes = ExtractExpandingCubes(blocks.ToList());
            for (int i = 0; i < expandingCubes.Count; i++)
            {
                expandingCubes[i].AddSelfToMesh(blocks, verts, tris, uvs);
            }
        }

        private static List<ExpandingCubeMesh> ExtractExpandingCubes(List<Vector3Int> blocksLeft)
        {
            List<ExpandingCubeMesh> result = new List<ExpandingCubeMesh>();
            while (blocksLeft.Count > 0)
            {
                ExpandingCubeMesh expandingCube = new ExpandingCubeMesh(blocksLeft[0]);
                blocksLeft.RemoveAt(0);

                expandingCube.ExpandUp(blocksLeft);
                expandingCube.ExpandDown(blocksLeft);

                expandingCube.ExandRight(blocksLeft);
                expandingCube.ExpandLeft(blocksLeft);

                expandingCube.ExpandForward(blocksLeft);
                expandingCube.ExpandBack(blocksLeft);

                result.Add(expandingCube);
            }

            return result;
        }

        /// <summary>
        /// Here be dragons.
        /// </summary>
        private class ExpandingCubeMesh
        {
            private int minX;
            private int maxX;
            private int minY;
            private int maxY;
            private int minZ;
            private int maxZ;

            public ExpandingCubeMesh(Vector3Int center)
            {
                minX = maxX = center.x;
                minY = maxY = center.y;
                minZ = maxZ = center.z;
            }

            public void AddSelfToMesh(HashSet<Vector3Int> blocks, List<Vector3> verts, List<int> tris, List<Vector2> uvs)
            {
                maxX++;
                maxY++;
                maxZ++;

                int width = Mathf.Abs(maxX - minX);
                int depth = Mathf.Abs(maxZ - minZ);
                int height = Mathf.Abs(maxY - minY);

                int faceCount = 0;
                int vertIndexOffset = verts.Count;

                var xSlice = blocks.Where(block => block.y >= minY && block.y < maxY).Where(block => block.z >= minZ && block.z < maxZ);
                var ySlice = blocks.Where(block => block.x >= minX && block.x < maxX).Where(block => block.z >= minZ && block.z < maxZ);
                var zSlice = blocks.Where(block => block.x >= minX && block.x < maxX).Where(block => block.y >= minY && block.y < maxY);

                // Y =======================

                int blocksAbove = ySlice.Where(block => block.y == maxY).Count();
                if (blocksAbove < width * depth)
                {
                    verts.Add(new Vector3(minX, maxY, minZ));
                    verts.Add(new Vector3(minX, maxY, maxZ));
                    verts.Add(new Vector3(maxX, maxY, maxZ));
                    verts.Add(new Vector3(maxX, maxY, minZ));

                    uvs.AddRange(new Vector2[]
                    {
                        new Vector2(0, 0),
                        new Vector2(0, depth),
                        new Vector2(width, depth),
                        new Vector2(width, 0)
                    });

                    faceCount++;
                }

                int blocksBelow = ySlice.Where(block => block.y == minY - 1).Count();
                if (blocksBelow < width * depth)
                {
                    verts.Add(new Vector3(minX, minY, minZ));
                    verts.Add(new Vector3(maxX, minY, minZ));
                    verts.Add(new Vector3(maxX, minY, maxZ));
                    verts.Add(new Vector3(minX, minY, maxZ));

                    uvs.AddRange(new Vector2[]
                    {
                        new Vector2(0, 0),
                        new Vector2(0, width),
                        new Vector2(depth, width),
                        new Vector2(depth, 0)
                    });

                    faceCount++;
                }

                // Z =======================

                int blocksInFront = zSlice.Where(block => block.z == maxZ).Count();
                if (blocksInFront < width * height)
                {
                    verts.Add(new Vector3(maxX, minY, maxZ));
                    verts.Add(new Vector3(maxX, maxY, maxZ));
                    verts.Add(new Vector3(minX, maxY, maxZ));
                    verts.Add(new Vector3(minX, minY, maxZ));

                    uvs.AddRange(new Vector2[]
                    {
                        new Vector2(0, 0),
                        new Vector2(0, height),
                        new Vector2(width, height),
                        new Vector2(width, 0)
                    });

                    faceCount++;
                }

                int blockBehind = zSlice.Where(block => block.z == minZ - 1).Count();
                if (blockBehind < width * height)
                {
                    verts.Add(new Vector3(minX, minY, minZ));
                    verts.Add(new Vector3(minX, maxY, minZ));
                    verts.Add(new Vector3(maxX, maxY, minZ));
                    verts.Add(new Vector3(maxX, minY, minZ));

                    uvs.AddRange(new Vector2[]
                    {
                        new Vector2(0, 0),
                        new Vector2(0, height),
                        new Vector2(width, height),
                        new Vector2(width, 0)
                    });

                    faceCount++;
                }

                // X =======================

                int blocksRight = xSlice.Where(block => block.x == maxX).Count();
                if (blocksRight < depth * height)
                {
                    verts.Add(new Vector3(maxX, minY, minZ));
                    verts.Add(new Vector3(maxX, maxY, minZ));
                    verts.Add(new Vector3(maxX, maxY, maxZ));
                    verts.Add(new Vector3(maxX, minY, maxZ));

                    uvs.AddRange(new Vector2[]
                    {
                        new Vector2(0, 0),
                        new Vector2(0, height),
                        new Vector2(depth, height),
                        new Vector2(depth, 0)
                    });

                    faceCount++;
                }

                int blockLeft = xSlice.Where(block => block.x == minX - 1).Count();
                if (blockLeft < depth * height)
                {
                    verts.Add(new Vector3(minX, minY, maxZ));
                    verts.Add(new Vector3(minX, maxY, maxZ));
                    verts.Add(new Vector3(minX, maxY, minZ));
                    verts.Add(new Vector3(minX, minY, minZ));

                    faceCount++;
                    uvs.AddRange(new Vector2[]
                    {
                        new Vector2(0, 0),
                        new Vector2(0, height),
                        new Vector2(depth, height),
                        new Vector2(depth, 0)
                    });

                }

                // =======================

                for (int i = 0; i < faceCount; i++)
                {
                    tris.Add(vertIndexOffset + i * 4);
                    tris.Add(vertIndexOffset + i * 4 + 1);
                    tris.Add(vertIndexOffset + i * 4 + 2);
                    tris.Add(vertIndexOffset + i * 4);
                    tris.Add(vertIndexOffset + i * 4 + 2);
                    tris.Add(vertIndexOffset + i * 4 + 3);
                }
            }

            public void ExpandUp(List<Vector3Int> blocksLeft)
            {
                Vector3Int current = new Vector3Int(0, maxY + 1, 0);
                for (int x = minX; x <= maxX; x++)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        current.x = x;
                        current.z = z;
                        if (!blocksLeft.Contains(current))
                            return;
                    }
                }

                // WE CAN EXPAND, REMOVE ALL IN PATH.
                for (int x = minX; x <= maxX; x++)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        current.x = x;
                        current.z = z;
                        blocksLeft.Remove(current);
                    }
                }

                maxY = current.y;
                ExpandUp(blocksLeft);
            }

            public void ExpandDown(List<Vector3Int> blocksLeft)
            {
                Vector3Int current = new Vector3Int(0, minY - 1, 0);
                for (int x = minX; x <= maxX; x++)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        current.x = x;
                        current.z = z;
                        if (!blocksLeft.Contains(current))
                            return;
                    }
                }

                // WE CAN EXPAND, REMOVE ALL IN PATH.
                for (int x = minX; x <= maxX; x++)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        current.x = x;
                        current.z = z;
                        blocksLeft.Remove(current);
                    }
                }

                minY = current.y;
                ExpandDown(blocksLeft);
            }

            public void ExpandLeft(List<Vector3Int> blocksLeft)
            {
                Vector3Int current = new Vector3Int(minX - 1, 0, 0);
                for (int y = minY; y <= maxY; y++)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        current.y = y;
                        current.z = z;
                        if (!blocksLeft.Contains(current))
                            return;
                    }
                }

                // WE CAN EXPAND, REMOVE ALL IN PATH.
                for (int y = minY; y <= maxY; y++)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        current.y = y;
                        current.z = z;
                        blocksLeft.Remove(current);
                    }
                }

                minX = current.x;
                ExpandLeft(blocksLeft);
            }

            public void ExandRight(List<Vector3Int> blocksLeft)
            {
                Vector3Int current = new Vector3Int(maxX + 1, 0, 0);
                for (int y = minY; y <= maxY; y++)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        current.y = y;
                        current.z = z;
                        if (!blocksLeft.Contains(current))
                            return;
                    }
                }

                // WE CAN EXPAND, REMOVE ALL IN PATH.
                for (int y = minY; y <= maxY; y++)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        current.y = y;
                        current.z = z;
                        blocksLeft.Remove(current);
                    }
                }

                maxX = current.x;
                ExandRight(blocksLeft);
            }

            public void ExpandForward(List<Vector3Int> blocksLeft)
            {
                Vector3Int current = new Vector3Int(0, 0, maxZ + 1);
                for (int x = minX; x <= maxX; x++)
                {
                    for (int y = minY; y <= maxY; y++)
                    {
                        current.x = x;
                        current.y = y;
                        if (!blocksLeft.Contains(current))
                            return;
                    }
                }

                // WE CAN EXPAND, REMOVE ALL IN PATH.
                for (int x = minX; x <= maxX; x++)
                {
                    for (int y = minY; y <= maxY; y++)
                    {
                        current.x = x;
                        current.y = y;
                        blocksLeft.Remove(current);
                    }
                }

                maxZ = current.z;
                ExpandForward(blocksLeft);
            }

            public void ExpandBack(List<Vector3Int> blocksLeft)
            {
                Vector3Int current = new Vector3Int(0, 0, minZ - 1);
                for (int x = minX; x <= maxX; x++)
                {
                    for (int y = minY; y <= maxY; y++)
                    {
                        current.x = x;
                        current.y = y;
                        if (!blocksLeft.Contains(current))
                            return;
                    }
                }

                // WE CAN EXPAND, REMOVE ALL IN PATH.
                for (int x = minX; x <= maxX; x++)
                {
                    for (int y = minY; y <= maxY; y++)
                    {
                        current.x = x;
                        current.y = y;
                        blocksLeft.Remove(current);
                    }
                }

                minZ = current.z;
                ExpandBack(blocksLeft);
            }
        }
    }
}
