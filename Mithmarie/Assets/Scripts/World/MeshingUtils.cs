using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mithmarie
{
    public static class MeshingUtils
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

        /// <summary>
        /// https://github.com/samhogan/Minecraft-Unity3D/blob/master/Assets/Scripts/TerrainChunk.cs
        /// </summary>
        public static void GenerateCulledMesh(HashSet<Vector3Int> blocks, Predicate<Vector3Int> hasBlock, List<Vector3> verts, List<int> tris, List<Vector2> uvs)
        {
            verts.Clear();
            tris.Clear();
            uvs.Clear();

            foreach (Vector3Int blockPos in blocks)
            {
                int faceCount = 0;
                int vertIndexOffset = verts.Count;

                // UP. ===
                if (!hasBlock(blockPos + Vector3Int.up))
                {
                    verts.Add(blockPos + Vector3Int.up);
                    verts.Add(blockPos + upForward);
                    verts.Add(blockPos + Vector3Int.one);
                    verts.Add(blockPos + upRight);
                    faceCount++;
                }

                // DOWN. ===
                if (!hasBlock(blockPos + Vector3Int.down))
                {
                    verts.Add(blockPos + Vector3Int.zero);
                    verts.Add(blockPos + Vector3Int.right);
                    verts.Add(blockPos + forwardRight);
                    verts.Add(blockPos + Vector3Int.forward);
                    faceCount++;
                }

                // FORWARD. ===
                if (!hasBlock(blockPos + Vector3Int.forward))
                {
                    verts.Add(blockPos + forwardRight);
                    verts.Add(blockPos + Vector3Int.one);
                    verts.Add(blockPos + upForward);
                    verts.Add(blockPos + Vector3Int.forward);
                    faceCount++;
                }

                // RIGHT. ===
                if (!hasBlock(blockPos + Vector3Int.right))
                {
                    verts.Add(blockPos + Vector3Int.right);
                    verts.Add(blockPos + upRight);
                    verts.Add(blockPos + Vector3Int.one);
                    verts.Add(blockPos + forwardRight);
                    faceCount++;
                }

                // BACK. ===
                if (!hasBlock(blockPos + Vector3Int.back))
                {
                    verts.Add(blockPos + Vector3Int.zero);
                    verts.Add(blockPos + Vector3Int.up);
                    verts.Add(blockPos + upRight);
                    verts.Add(blockPos + Vector3Int.right);
                    faceCount++;
                }

                // LEFT. ===
                if (!hasBlock(blockPos + Vector3Int.left))
                {
                    verts.Add(blockPos + Vector3Int.forward);
                    verts.Add(blockPos + upForward);
                    verts.Add(blockPos + Vector3Int.up);
                    verts.Add(blockPos + Vector3Int.zero);
                    faceCount++;
                }

                // GENERATE TRIANGLES. ===
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
        public static void GenerateGreedyMesh(HashSet<Vector3Int> blocks, Predicate<Vector3Int> hasBlock, List<Vector3> verts, List<int> tris, List<Vector2> uvs)
        {
            verts.Clear();
            tris.Clear();
            uvs.Clear();

            Vector3 globalOffset = new Vector3(-0.5f, 0f, -0.5f);
            Dictionary<int, HashSet<Vector2Int>> upFaces = new Dictionary<int, HashSet<Vector2Int>>();
            Dictionary<int, HashSet<Vector2Int>> downFaces = new Dictionary<int, HashSet<Vector2Int>>();
            Dictionary<int, HashSet<Vector2Int>> forwardFaces = new Dictionary<int, HashSet<Vector2Int>>();
            Dictionary<int, HashSet<Vector2Int>> backwardFaces = new Dictionary<int, HashSet<Vector2Int>>();
            Dictionary<int, HashSet<Vector2Int>> leftFaces = new Dictionary<int, HashSet<Vector2Int>>();
            Dictionary<int, HashSet<Vector2Int>> rightFaces = new Dictionary<int, HashSet<Vector2Int>>();

            // GET THE FACES OF THE MESH IN LAYERS. ===
            foreach (Vector3Int blockPos in blocks)
            {
                if (!hasBlock(blockPos + Vector3Int.up))
                {
                    if (!upFaces.ContainsKey(blockPos.y))
                        upFaces.Add(blockPos.y, new HashSet<Vector2Int>());

                    upFaces[blockPos.y].Add(new Vector2Int(blockPos.x, blockPos.z));
                }

                if (!hasBlock(blockPos + Vector3Int.down))
                {
                    if (!downFaces.ContainsKey(blockPos.y))
                        downFaces.Add(blockPos.y, new HashSet<Vector2Int>());

                    downFaces[blockPos.y].Add(new Vector2Int(blockPos.x, blockPos.z));
                }

                if (!hasBlock(blockPos + Vector3Int.forward))
                {
                    if (!forwardFaces.ContainsKey(blockPos.z))
                        forwardFaces.Add(blockPos.z, new HashSet<Vector2Int>());

                    forwardFaces[blockPos.z].Add(new Vector2Int(blockPos.x, blockPos.y));
                }

                if (!hasBlock(blockPos + Vector3Int.right))
                {
                    if (!rightFaces.ContainsKey(blockPos.x))
                        rightFaces.Add(blockPos.x, new HashSet<Vector2Int>());

                    rightFaces[blockPos.x].Add(new Vector2Int(blockPos.y, blockPos.z));
                }

                if (!hasBlock(blockPos + Vector3Int.back))
                {
                    if (!backwardFaces.ContainsKey(blockPos.z))
                        backwardFaces.Add(blockPos.z, new HashSet<Vector2Int>());

                    backwardFaces[blockPos.z].Add(new Vector2Int(blockPos.x, blockPos.y));
                }

                if (!hasBlock(blockPos + Vector3Int.left))
                {
                    if (!leftFaces.ContainsKey(blockPos.x))
                        leftFaces.Add(blockPos.x, new HashSet<Vector2Int>());

                    leftFaces[blockPos.x].Add(new Vector2Int(blockPos.y, blockPos.z));
                }
            }

            // FORWARD. ===
            foreach (KeyValuePair<int, HashSet<Vector2Int>> slice in forwardFaces)
            {
                float z = slice.Key + 1f;
                while (slice.Value.Count > 0)
                {
                    GreedyQuad quad = new GreedyQuad(slice.Value.First(), slice.Value);
                    quad.ExpandRight(slice.Value);
                    quad.ExpandLeft(slice.Value);
                    quad.ExpandUp(slice.Value);
                    quad.ExpandDown(slice.Value);

                    int width = Mathf.Abs(quad.maxX - quad.minX);
                    int height = Mathf.Abs(quad.maxY - quad.minY);

                    int vertIndexOffset = verts.Count;
                    verts.Add(new Vector3(quad.maxX, quad.minY, z));
                    verts.Add(new Vector3(quad.maxX, quad.maxY, z));
                    verts.Add(new Vector3(quad.minX, quad.maxY, z));
                    verts.Add(new Vector3(quad.minX, quad.minY, z));

                    uvs.AddRange(new Vector2[]
                    {
                        new Vector2(0, 0),
                        new Vector2(0, height),
                        new Vector2(width, height),
                        new Vector2(width, 0)
                    });

                    // FIRST TRIANGLE.
                    tris.Add(vertIndexOffset + 0);
                    tris.Add(vertIndexOffset + 1);
                    tris.Add(vertIndexOffset + 2);
                    
                    // SECOND TRIANGLE.
                    tris.Add(vertIndexOffset + 0);
                    tris.Add(vertIndexOffset + 2);
                    tris.Add(vertIndexOffset + 3);
                }
            }

            // BACKWARD. ===
            foreach (KeyValuePair<int, HashSet<Vector2Int>> slice in backwardFaces)
            {
                float z = slice.Key;
                while (slice.Value.Count > 0)
                {
                    GreedyQuad quad = new GreedyQuad(slice.Value.First(), slice.Value);
                    quad.ExpandRight(slice.Value);
                    quad.ExpandLeft(slice.Value);
                    quad.ExpandUp(slice.Value);
                    quad.ExpandDown(slice.Value);

                    int width = Mathf.Abs(quad.maxX - quad.minX);
                    int height = Mathf.Abs(quad.maxY - quad.minY);

                    int vertIndexOffset = verts.Count;
                    verts.Add(new Vector3(quad.minX, quad.minY, z));
                    verts.Add(new Vector3(quad.minX, quad.maxY, z));
                    verts.Add(new Vector3(quad.maxX, quad.maxY, z));
                    verts.Add(new Vector3(quad.maxX, quad.minY, z));

                    uvs.AddRange(new Vector2[]
                    {
                        new Vector2(0, 0),
                        new Vector2(0, height),
                        new Vector2(width, height),
                        new Vector2(width, 0)
                    });

                    // FIRST TRIANGLE.
                    tris.Add(vertIndexOffset + 0);
                    tris.Add(vertIndexOffset + 1);
                    tris.Add(vertIndexOffset + 2);

                    // SECOND TRIANGLE.
                    tris.Add(vertIndexOffset + 0);
                    tris.Add(vertIndexOffset + 2);
                    tris.Add(vertIndexOffset + 3);
                }
            }

            // RIGHT. ===
            foreach (KeyValuePair<int, HashSet<Vector2Int>> slice in rightFaces)
            {
                float x = slice.Key + 1f;
                while (slice.Value.Count > 0)
                {
                    GreedyQuad quad = new GreedyQuad(slice.Value.First(), slice.Value);
                    quad.ExpandRight(slice.Value);
                    quad.ExpandLeft(slice.Value);
                    quad.ExpandUp(slice.Value);
                    quad.ExpandDown(slice.Value);

                    int width = Mathf.Abs(quad.maxX - quad.minX);
                    int height = Mathf.Abs(quad.maxY - quad.minY);

                    int vertIndexOffset = verts.Count;
                    verts.Add(new Vector3(x, quad.minX, quad.minY));
                    verts.Add(new Vector3(x, quad.maxX, quad.minY));
                    verts.Add(new Vector3(x, quad.maxX, quad.maxY));
                    verts.Add(new Vector3(x, quad.minX, quad.maxY));

                    uvs.AddRange(new Vector2[]
                    {
                        new Vector2(0, 0),
                        new Vector2(0, width),
                        new Vector2(height, width),
                        new Vector2(height, 0)
                    });

                    // FIRST TRIANGLE.
                    tris.Add(vertIndexOffset + 0);
                    tris.Add(vertIndexOffset + 1);
                    tris.Add(vertIndexOffset + 2);

                    // SECOND TRIANGLE.
                    tris.Add(vertIndexOffset + 0);
                    tris.Add(vertIndexOffset + 2);
                    tris.Add(vertIndexOffset + 3);
                }
            }

            // LEFT. ===
            foreach (KeyValuePair<int, HashSet<Vector2Int>> slice in leftFaces)
            {
                float x = slice.Key;
                while (slice.Value.Count > 0)
                {
                    GreedyQuad quad = new GreedyQuad(slice.Value.First(), slice.Value);
                    quad.ExpandRight(slice.Value);
                    quad.ExpandLeft(slice.Value);
                    quad.ExpandUp(slice.Value);
                    quad.ExpandDown(slice.Value);

                    int width = Mathf.Abs(quad.maxX - quad.minX);
                    int height = Mathf.Abs(quad.maxY - quad.minY);

                    int vertIndexOffset = verts.Count;
                    verts.Add(new Vector3(x, quad.minX, quad.maxY));
                    verts.Add(new Vector3(x, quad.maxX, quad.maxY));
                    verts.Add(new Vector3(x, quad.maxX, quad.minY));
                    verts.Add(new Vector3(x, quad.minX, quad.minY));

                    uvs.AddRange(new Vector2[]
                    {
                        new Vector2(0, 0),
                        new Vector2(0, width),
                        new Vector2(height, width),
                        new Vector2(height, 0)
                    });

                    // FIRST TRIANGLE.
                    tris.Add(vertIndexOffset + 0);
                    tris.Add(vertIndexOffset + 1);
                    tris.Add(vertIndexOffset + 2);

                    // SECOND TRIANGLE.
                    tris.Add(vertIndexOffset + 0);
                    tris.Add(vertIndexOffset + 2);
                    tris.Add(vertIndexOffset + 3);
                }
            }

            // UP. ===
            foreach (KeyValuePair<int, HashSet<Vector2Int>> slice in upFaces)
            {
                float y = slice.Key + 1f;
                while (slice.Value.Count > 0)
                {
                    GreedyQuad quad = new GreedyQuad(slice.Value.First(), slice.Value);
                    quad.ExpandRight(slice.Value);
                    quad.ExpandLeft(slice.Value);
                    quad.ExpandUp(slice.Value);
                    quad.ExpandDown(slice.Value);

                    int width = Mathf.Abs(quad.maxX - quad.minX);
                    int height = Mathf.Abs(quad.maxY - quad.minY);

                    int vertIndexOffset = verts.Count;
                    verts.Add(new Vector3(quad.minX, y, quad.minY));
                    verts.Add(new Vector3(quad.minX, y, quad.maxY));
                    verts.Add(new Vector3(quad.maxX, y, quad.maxY));
                    verts.Add(new Vector3(quad.maxX, y, quad.minY));

                    uvs.AddRange(new Vector2[]
                    {
                        new Vector2(0, 0),
                        new Vector2(0, height),
                        new Vector2(width, height),
                        new Vector2(width, 0)
                    });

                    // FIRST TRIANGLE.
                    tris.Add(vertIndexOffset + 0);
                    tris.Add(vertIndexOffset + 1);
                    tris.Add(vertIndexOffset + 2);

                    // SECOND TRIANGLE.
                    tris.Add(vertIndexOffset + 0);
                    tris.Add(vertIndexOffset + 2);
                    tris.Add(vertIndexOffset + 3);
                }
            }

            // DOWN. ===
            foreach (KeyValuePair<int, HashSet<Vector2Int>> slice in downFaces)
            {
                float y = slice.Key;
                while (slice.Value.Count > 0)
                {
                    GreedyQuad quad = new GreedyQuad(slice.Value.First(), slice.Value);
                    quad.ExpandRight(slice.Value);
                    quad.ExpandLeft(slice.Value);
                    quad.ExpandUp(slice.Value);
                    quad.ExpandDown(slice.Value);

                    int width = Mathf.Abs(quad.maxX - quad.minX);
                    int height = Mathf.Abs(quad.maxY - quad.minY);

                    int vertIndexOffset = verts.Count;
                    verts.Add(new Vector3(quad.minX, y, quad.minY));
                    verts.Add(new Vector3(quad.maxX, y, quad.minY));
                    verts.Add(new Vector3(quad.maxX, y, quad.maxY));
                    verts.Add(new Vector3(quad.minX, y, quad.maxY));

                    uvs.AddRange(new Vector2[]
                    {
                        new Vector2(0, 0),
                        new Vector2(0, width),
                        new Vector2(height, width),
                        new Vector2(height, 0)
                    });

                    // FIRST TRIANGLE.
                    tris.Add(vertIndexOffset + 0);
                    tris.Add(vertIndexOffset + 1);
                    tris.Add(vertIndexOffset + 2);

                    // SECOND TRIANGLE.
                    tris.Add(vertIndexOffset + 0);
                    tris.Add(vertIndexOffset + 2);
                    tris.Add(vertIndexOffset + 3);
                }
            }

            // APPLY GLOBAL OFFSET. ===
            for (int i = 0; i < verts.Count; i++)
            {
                verts[i] += globalOffset;
            }
        }

        public static void GenerateExpandingCubesMesh(HashSet<Vector3Int> blocks, List<Vector3> verts, List<int> tris, List<Vector2> uvs)
        {
            verts.Clear();
            tris.Clear();
            uvs.Clear();

            List<ExpandingCube> expandingCubes = new List<ExpandingCube>();

            List<Vector3Int> blocksLeft = blocks.ToList();
            while (blocksLeft.Count > 0)
            {
                ExpandingCube current = new ExpandingCube(blocksLeft[0]);
                blocksLeft.RemoveAt(0);

                current.ExpandUp(blocksLeft);
                current.ExpandDown(blocksLeft);

                current.ExandRight(blocksLeft);
                current.ExpandLeft(blocksLeft);

                current.ExpandForward(blocksLeft);
                current.ExpandBack(blocksLeft);

                expandingCubes.Add(current);
            }

            for (int i = 0; i < expandingCubes.Count; i++)
            {
                expandingCubes[i].AddSelfToMesh(blocks, verts, tris, uvs);
            }
        }
    }
}
