using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mithmarie
{
    public class ExpandingCubeMesh
    {
        public int minX;
        public int maxX;
        public int minY;
        public int maxY;
        public int minZ;
        public int maxZ;
    
        public ExpandingCubeMesh(Vector3Int center)
        {
            minX = maxX = center.x;
            minY = maxY = center.y;
            minZ = maxZ = center.z;
        }
    
        public bool CanGoUp(HashSet<Vector3Int> blocksLeft, Queue<Vector3Int> blocksToRemove)
        {
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
    
        public bool CanGoDown(HashSet<Vector3Int> blocksLeft, Queue<Vector3Int> blocksToRemove)
        {
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
    
        public bool CanGoLeft(HashSet<Vector3Int> blocksLeft, Queue<Vector3Int> blocksToRemove)
        {
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
    
        public bool CanGoRight(HashSet<Vector3Int> blocksLeft, Queue<Vector3Int> blocksToRemove)
        {
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
    
        public bool CanGoForward(HashSet<Vector3Int> blocksLeft, Queue<Vector3Int> blocksToRemove)
        {
            Vector3Int current = new Vector3Int(0, 0, maxZ + 1);
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    current.x = x;
                    current.y = y;
                    blocksToRemove.Enqueue(current);
                    if (!blocksLeft.Contains(current))
                        return false;
                }
            }
    
            maxZ = current.z;
            return true;
        }
    
        public bool CanGoBack(HashSet<Vector3Int> blocksLeft, Queue<Vector3Int> blocksToRemove)
        {
            Vector3Int current = new Vector3Int(0, 0, minZ - 1);
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    current.x = x;
                    current.y = y;
                    blocksToRemove.Enqueue(current);
                    if (!blocksLeft.Contains(current))
                        return false;
                }
            }
    
            minZ = current.z;
            return true;
        }
    
        public void SpawnDemoCube(GameObject prefab)
        {
            Transform cube = Object.Instantiate(prefab, new Vector3(minX + maxX, minY + maxY, minZ + maxZ) / 2f, Quaternion.identity).transform;
            cube.localScale = new Vector3(Mathf.Abs(maxX - minX) + 1, Mathf.Abs(maxY - minY) + 1, Mathf.Abs(maxZ - minZ) + 1);
        }

        public void AddSelfToMesh(List<Vector3> verts, List<int> tris, List<Vector2> uvs)
        {
            maxX++;
            maxY++;
            maxZ++;

            int[] tempTris = new int[6];
            int faceCount = 6;
            int offset = verts.Count;

            verts.Add(new Vector3(minX, maxY, minZ));
            verts.Add(new Vector3(minX, maxY, maxZ));
            verts.Add(new Vector3(maxX, maxY, maxZ));
            verts.Add(new Vector3(maxX, maxY, minZ));

            verts.Add(new Vector3(minX, minY, minZ));
            verts.Add(new Vector3(maxX, minY, minZ));
            verts.Add(new Vector3(maxX, minY, maxZ));
            verts.Add(new Vector3(minX, minY, maxZ));

            verts.Add(new Vector3(minX, minY, minZ));
            verts.Add(new Vector3(minX, maxY, minZ));
            verts.Add(new Vector3(maxX, maxY, minZ));
            verts.Add(new Vector3(maxX, minY, minZ));

            verts.Add(new Vector3(maxX, minY, minZ));
            verts.Add(new Vector3(maxX, maxY, minZ));
            verts.Add(new Vector3(maxX, maxY, maxZ));
            verts.Add(new Vector3(maxX, minY, maxZ));

            verts.Add(new Vector3(maxX, minY, maxZ));
            verts.Add(new Vector3(maxX, maxY, maxZ));
            verts.Add(new Vector3(minX, maxY, maxZ));
            verts.Add(new Vector3(minX, minY, maxZ));

            verts.Add(new Vector3(minX, minY, maxZ));
            verts.Add(new Vector3(minX, maxY, maxZ));
            verts.Add(new Vector3(minX, maxY, minZ));
            verts.Add(new Vector3(minX, minY, minZ));
            
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