using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    /// <summary>
    /// https://github.com/samhogan/Minecraft-Unity3D/blob/master/Assets/Scripts/TerrainChunk.cs
    /// </summary>
    public class CulledMeshGenerator : IMeshingStrategy
    {
        private Vector3 upForward = new Vector3(0, 1, 1);
        private Vector3 upRight = new Vector3(1, 1, 0);
        private Vector3 forwardRight = new Vector3(1, 0, 1);

        public Mesh GenerateMesh(HashSet<Vector3Int> blocks)
        {
            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();
            List<Vector2> uvs = new List<Vector2>();
            int[] tempTris = new int[6];

            Vector2[] faceUvs = new Vector2[]
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0)
            };

            Mesh mesh = new Mesh();
            
            foreach (Vector3Int blockPos in blocks)
            {
                int faceCount = 0;
                int offset = verts.Count;
                
                if (!blocks.Contains(blockPos + Vector3Int.up))
                {
                    verts.Add(blockPos + Vector3Int.up);
                    verts.Add(blockPos + upForward);
                    verts.Add(blockPos + Vector3Int.one);
                    verts.Add(blockPos + upRight);
                    faceCount++;
                }

                if (!blocks.Contains(blockPos + Vector3Int.down))
                {
                    verts.Add(blockPos + Vector3Int.zero);
                    verts.Add(blockPos + Vector3Int.right);
                    verts.Add(blockPos + forwardRight);
                    verts.Add(blockPos + Vector3Int.forward);
                    faceCount++;
                }

                if (!blocks.Contains(blockPos + Vector3Int.forward))
                {
                    verts.Add(blockPos + forwardRight);
                    verts.Add(blockPos + Vector3Int.one);
                    verts.Add(blockPos + upForward);
                    verts.Add(blockPos + Vector3Int.forward);
                    faceCount++;
                }

                if (!blocks.Contains(blockPos + Vector3Int.right))
                {
                    verts.Add(blockPos + Vector3Int.right);
                    verts.Add(blockPos + upRight);
                    verts.Add(blockPos + Vector3Int.one);
                    verts.Add(blockPos + forwardRight);
                    faceCount++;
                }

                if (!blocks.Contains(blockPos + Vector3Int.back))
                {
                    verts.Add(blockPos + Vector3Int.zero);
                    verts.Add(blockPos + Vector3Int.up);
                    verts.Add(blockPos + upRight);
                    verts.Add(blockPos + Vector3Int.right);
                    faceCount++;
                }

                if (!blocks.Contains(blockPos + Vector3Int.left))
                {
                    verts.Add(blockPos + Vector3Int.forward);
                    verts.Add(blockPos + upForward);
                    verts.Add(blockPos + Vector3Int.up);
                    verts.Add(blockPos + Vector3Int.zero);
                    faceCount++;
                }
                
                for (int i = 0; i < faceCount; i++)
                {
                    tempTris[0] = offset + i * 4;
                    tempTris[1] = offset + i * 4 + 1;
                    tempTris[2] = offset + i * 4 + 2;

                    tempTris[3] = offset + i * 4;
                    tempTris[4] = offset + i * 4 + 2;
                    tempTris[5] = offset + i * 4 + 3;

                    tris.AddRange(tempTris);
                    uvs.AddRange(faceUvs);
                }
            }

            mesh.vertices = verts.ToArray();
            mesh.triangles = tris.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}