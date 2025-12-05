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

            foreach (Vector3Int pos in blocks)
            {
                int faceCount = 0;
                int vertIndexOffset = verts.Count;

                // UP. ===
                if (!hasBlock(pos + Vector3Int.up))
                {
                    verts.Add(pos + Vector3Int.up);
                    verts.Add(pos + upForward);
                    verts.Add(pos + Vector3Int.one);
                    verts.Add(pos + upRight);
                    faceCount++;
                }

                // DOWN. ===
                if (!hasBlock(pos + Vector3Int.down))
                {
                    verts.Add(pos + Vector3Int.zero);
                    verts.Add(pos + Vector3Int.right);
                    verts.Add(pos + forwardRight);
                    verts.Add(pos + Vector3Int.forward);
                    faceCount++;
                }

                // FORWARD. ===
                if (!hasBlock(pos + Vector3Int.forward))
                {
                    verts.Add(pos + forwardRight);
                    verts.Add(pos + Vector3Int.one);
                    verts.Add(pos + upForward);
                    verts.Add(pos + Vector3Int.forward);
                    faceCount++;
                }

                // RIGHT. ===
                if (!hasBlock(pos + Vector3Int.right))
                {
                    verts.Add(pos + Vector3Int.right);
                    verts.Add(pos + upRight);
                    verts.Add(pos + Vector3Int.one);
                    verts.Add(pos + forwardRight);
                    faceCount++;
                }

                // BACK. ===
                if (!hasBlock(pos + Vector3Int.back))
                {
                    verts.Add(pos + Vector3Int.zero);
                    verts.Add(pos + Vector3Int.up);
                    verts.Add(pos + upRight);
                    verts.Add(pos + Vector3Int.right);
                    faceCount++;
                }

                // lEFT. ===
                if (!hasBlock(pos + Vector3Int.left))
                {
                    verts.Add(pos + Vector3Int.forward);
                    verts.Add(pos + upForward);
                    verts.Add(pos + Vector3Int.up);
                    verts.Add(pos + Vector3Int.zero);
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

        public static void GenerateGreedyMesh(HashSet<Vector3Int> blocks, Predicate<Vector3Int> hasBlock, List<Vector3> verts, List<int> tris, List<Vector2> uvs)
        {
            verts.Clear();
            tris.Clear();
            uvs.Clear();

            // GET THE FACES OF THE MESH IN LAYERS. ===

            // CONSIDER MAKING USE OF STATIC HERE ??
            Dictionary<int, HashSet<Vector2Int>> upFaces = new Dictionary<int, HashSet<Vector2Int>>();
            Dictionary<int, HashSet<Vector2Int>> downFaces = new Dictionary<int, HashSet<Vector2Int>>();
            Dictionary<int, HashSet<Vector2Int>> forwardFaces = new Dictionary<int, HashSet<Vector2Int>>();
            Dictionary<int, HashSet<Vector2Int>> backFaces = new Dictionary<int, HashSet<Vector2Int>>();
            Dictionary<int, HashSet<Vector2Int>> leftFaces = new Dictionary<int, HashSet<Vector2Int>>();
            Dictionary<int, HashSet<Vector2Int>> rightFaces = new Dictionary<int, HashSet<Vector2Int>>();

            foreach (Vector3Int blockPos in blocks)
            {
                if (!hasBlock(blockPos + Vector3Int.up))
                {
                    if (upFaces[blockPos.y] == null)
                        upFaces[blockPos.y] = new HashSet<Vector2Int>();

                    upFaces[blockPos.y].Add(new Vector2Int(blockPos.x, blockPos.z));
                }

                if (!hasBlock(blockPos + Vector3Int.down))
                {
                    if (downFaces[blockPos.y] == null)
                        downFaces[blockPos.y] = new HashSet<Vector2Int>();

                    downFaces[blockPos.y].Add(new Vector2Int(blockPos.x, blockPos.z));
                }

                if (!hasBlock(blockPos + Vector3Int.forward))
                {
                    if (forwardFaces[blockPos.z] == null)
                        forwardFaces[blockPos.z] = new HashSet<Vector2Int>();

                    forwardFaces[blockPos.z].Add(new Vector2Int(blockPos.x, blockPos.y));
                }

                if (!hasBlock(blockPos + Vector3Int.right))
                {
                    if (rightFaces[blockPos.x] == null)
                        rightFaces[blockPos.x] = new HashSet<Vector2Int>();

                    rightFaces[blockPos.x].Add(new Vector2Int(blockPos.y, blockPos.z));
                }

                if (!hasBlock(blockPos + Vector3Int.back))
                {
                    if (backFaces[blockPos.z] == null)
                        backFaces[blockPos.z] = new HashSet<Vector2Int>();

                    backFaces[blockPos.z].Add(new Vector2Int(blockPos.x, blockPos.y));
                }

                if (!hasBlock(blockPos + Vector3Int.left))
                {
                    if (leftFaces[blockPos.x] == null)
                        leftFaces[blockPos.x] = new HashSet<Vector2Int>();

                    leftFaces[blockPos.x].Add(new Vector2Int(blockPos.y, blockPos.z));
                }
            }

            // CONGLOMERATE UP FACES INTO GREEDY QUADS. ===

            // try to make this work first and then generalize.
            // just see if above facing show up at all,
            // it prolly still needs some tweaking but idk.
            foreach (KeyValuePair<int, HashSet<Vector2Int>> slice in forwardFaces)
            {
                int z = slice.Key; // THIS VALUE MIGHT NEED TO CHANGE.
                while (slice.Value.Count > 0)
                {
                    GreedyQuad quad = new GreedyQuad(slice.Value.First(), slice.Value);
                    quad.ExpandRight(slice.Value);
                    quad.ExpandLeft(slice.Value);
                    quad.ExpandUp(slice.Value);
                    quad.ExpandDown(slice.Value);

                    int width = Mathf.Abs(quad.maxX - quad.minX);
                    int height = Mathf.Abs(quad.maxY - quad.minY);

                    // i mean, you COULD implement this in the greedyQued itself, but this seems
                    // more efficient.
                    // in general, i think it better to make a 
                    // function more like c++ style than use classes for "AddSelfToMesh"
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
