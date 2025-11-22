using System.Collections.Generic;
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

        public int _maxX;
        public int _maxY;
        public int _maxZ;

        public ExpandingCubeMesh(Vector3Int center)
        {
            minX = maxX = center.x;
            minY = maxY = center.y;
            minZ = maxZ = center.z;
        }
    
        public void ExandUp(List<Vector3Int> blocksLeft)
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
            ExandUp(blocksLeft);
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

        public bool CanDrawUpFace(HashSet<Vector3Int> blocks)
        {
            return true;
            Vector3Int current = new Vector3Int(0, _maxY + 1, 0);
            for (int x = minX; x <= _maxY; x++)
            {
                for (int z = minZ; z <= _maxY; z++)
                {
                    current.x = x;
                    current.z = z;
                    if (!blocks.Contains(current))
                        return true;
                }
            }

            // HERE, WE WILL NOT DRAW BECAUSE THERE IS NO
            // AIR BLOCK TO SEE IT THROUGH.
            return false;
        }

        public bool CanDrawBottomFace(HashSet<Vector3Int> blocks)
        {
            return true;
            Vector3Int current = new Vector3Int(0, minY - 1, 0);
            for (int x = minX; x <= _maxX; x++)
            {
                for (int z = minZ; z <= _maxZ; z++)
                {
                    current.x = x;
                    current.z = z;
                    if (!blocks.Contains(current))
                        return true;
                }
            }

            return false;
        }

        public bool CanDrawLeftFace(HashSet<Vector3Int> blocks)
        {
            return true;
            Vector3Int current = new Vector3Int(minX - 1, 0, 0);
            for (int y = minY; y <= _maxY; y++)
            {
                for (int z = minZ; z <= _maxZ; z++)
                {
                    current.y = y;
                    current.z = z;
                    if (!blocks.Contains(current))
                        return true;
                }
            }

            return false;
        }

        public bool CanDrawRightFace(HashSet<Vector3Int> blocks)
        {
            return true;
            Vector3Int current = new Vector3Int(_maxX + 1, 0, 0);
            for (int y = minY; y <= _maxY; y++)
            {
                for (int z = minZ; z <= _maxZ; z++)
                {
                    current.y = y;
                    current.z = z;
                    if (!blocks.Contains(current))
                        return true;
                }
            }

            return false;
        }

        public bool CanDrawFrontFace(HashSet<Vector3Int> blocks)
        {
            return true;
            Vector3Int current = new Vector3Int(0, 0, _maxZ + 1);
            for (int x = minX; x <= _maxX; x++)
            {
                for (int y = minY; y <= _maxY; y++)
                {
                    current.x = x;
                    current.y = y;
                    if (!blocks.Contains(current))
                        return true;
                }
            }

            return false;
        }

        public bool CanDrawBackFace(HashSet<Vector3Int> blocks)
        {
            return true;
            Vector3Int current = new Vector3Int(0, 0, minZ - 1);
            for (int x = minX; x <= _maxX; x++)
            {
                for (int y = minY; y <= _maxX; y++)
                {
                    current.x = x;
                    current.y = y;
                    if (!blocks.Contains(current))
                        return true;
                }
            }

            return false;
        }

        public void AddSelfToMesh(List<Vector3> verts, List<int> tris, List<Vector2> uvs, HashSet<Vector3Int> blocks)
        {
            SaveMax();
            
            maxX++;
            maxY++;
            maxZ++;

            int width = Mathf.Abs(maxX - minX);
            int depth = Mathf.Abs(maxZ - minZ);
            int height = Mathf.Abs(maxY - minY);

            int[] tempTris = new int[6];
            int faceCount = 0;
            int offset = verts.Count;

            // if all the blcoks in the upwards direction are not blocks.
            if (CanDrawUpFace(blocks))
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

            if (CanDrawBottomFace(blocks))
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

            if (CanDrawBackFace(blocks))
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

            if (CanDrawRightFace(blocks))
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

            if (CanDrawFrontFace(blocks))
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

            if (CanDrawLeftFace(blocks))
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

            for (int i = 0; i < faceCount; i++)
            {
                tempTris[0] = offset + i * 4;
                tempTris[1] = offset + i * 4 + 1;
                tempTris[2] = offset + i * 4 + 2;
    
                tempTris[3] = offset + i * 4;
                tempTris[4] = offset + i * 4 + 2;
                tempTris[5] = offset + i * 4 + 3;
    
                tris.AddRange(tempTris);
                /*uvs.AddRange(new Vector2[]
                {
                    new Vector2(0, 0),
                    new Vector2(0, Mathf.Abs(maxY-minY)),
                    new Vector2(Mathf.Abs(maxX-minX), Mathf.Abs(maxY-minY)),
                    new Vector2(Mathf.Abs(maxX-minX), 0)
                });*/
            }
        }

        private void SaveMax()
        {
            _maxX = maxX;
            _maxY = maxY;
            _maxZ = maxZ;
        }
    }
}