using System;
using System.Collections.Generic;
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
        /// /// <summary>
        /// https://github.com/samhogan/Minecraft-Unity3D/blob/master/Assets/Scripts/TerrainChunk.cs
        /// </summary>
        public static void GenerateCulledMesh(HashSet<Vector3Int> blocks, Predicate<Vector3Int> hasBlock, List<Vector3> verts, List<int> tris, List<Vector2> uvs)
        {
            // MAKE SURE EVERYTHING IS CLEARED.
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


    }
}
