using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mithmarie
{
    public class World : MonoBehaviour, IBinarySerializable
    {
        public const int CHUNK_SIZE = 16;
        
        //private readonly HashSet<Vector3Int> blocks = new HashSet<Vector3Int>();

        private readonly Dictionary<Vector3Int, HashSet<Vector3Int>> chunks = new Dictionary<Vector3Int, HashSet<Vector3Int>>();
        private readonly HashSet<Vector3Int> changedChunks;

        private IMeshGeneratable generatable;
        public ChunkManager chunkManager;
        private IMessageService message;

        private delegate void EditBlock(Vector3Int blockPos);
        private EditBlock editBlock;

        private void Awake()
        {
            generatable = FindAnyObjectByType<CulledMeshGenerator>();
        }

        private void Start()
        {
            message = ServiceLocator<IMessageService>.Locate();
        }

        public void AddSelection(Vector3Int a, Vector3Int b)
        {
            editBlock = Add;
            EditSelection(a, b);
        }

        public void RemoveSelection(Vector3Int a, Vector3Int b)
        {
            editBlock = Remove;
            EditSelection(a, b);
        }

        public void Add(Vector3Int blockPos)
        {
            Vector3Int chunkPos = blockPos / CHUNK_SIZE;
            if (!chunks.ContainsKey(chunkPos))
                chunks.Add(chunkPos, new HashSet<Vector3Int>());

            if (chunks[chunkPos].Contains(blockPos))
                return;

            chunks[chunkPos].Add(blockPos);
            NotifyChunkHasChanged(chunkPos);
        }

        public void Remove(Vector3Int blockPos)
        {
            Vector3Int chunkPos = blockPos / CHUNK_SIZE;
            if (!chunks.ContainsKey(chunkPos))
                return;

            if (!chunks[chunkPos].Contains(blockPos))
                return;

            chunks[chunkPos].Remove(blockPos);
            NotifyChunkHasChanged(chunkPos);
        }

        public void Clear()
        {
            foreach (Vector3Int chunkPos in chunks.Keys)
            {
                changedChunks.Add(chunkPos);
            }

            chunks.Clear();
        }

        public void Flush()
        {
            foreach (Vector3Int chunkPos in changedChunks)
            {
                if(!chunks.ContainsKey(chunkPos) || chunks[chunkPos].Count <= 0)
                {
                    chunks.Remove(chunkPos);
                    chunkManager.DrawChunk(chunkPos, null);
                    continue;
                }

                chunkManager.DrawChunk(chunkPos, chunks[chunkPos]);
            }
        }

        private void EditSelection(Vector3Int a, Vector3Int b)
        {
            if (a == b)
            {
                editBlock(a);
                return;
            }

            Vector3Int temp = Vector3Int.zero;

            int width = Mathf.Abs(b.x - a.x) + 1;
            int height = Mathf.Abs(b.y - a.y) + 1;
            int depth = Mathf.Abs(b.z - a.z) + 1;

            int xDirection = a.x <= b.x ? 1 : -1;
            int yDirection = a.y <= b.y ? 1 : -1;
            int zDirection = a.z <= b.z ? 1 : -1;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        temp.x = x * xDirection;
                        temp.y = y * yDirection;
                        temp.z = z * zDirection;
                        editBlock(a + temp);
                    }
                }
            }
        }

        private void NotifyChunkHasChanged(Vector3Int chunkPos) 
        {
            changedChunks.Add(chunkPos);
            changedChunks.Add(chunkPos + Vector3Int.up);
            changedChunks.Add(chunkPos + Vector3Int.forward);
            changedChunks.Add(chunkPos + Vector3Int.back);
            changedChunks.Add(chunkPos + Vector3Int.left);
            changedChunks.Add(chunkPos + Vector3Int.right);
        }

        private Vector3Int[] GetAllBlocks()
        {
            List<Vector3Int> blocks = new List<Vector3Int>();
            foreach (HashSet<Vector3Int> chunk in chunks.Values)
            {
                foreach (Vector3Int block in chunk)
                {
                    blocks.Add(block);
                }
            }

            return blocks.ToArray();
        }
        
        public void Serialize(BinaryWriter writer)
        {
            Vector3Int[] blocks = GetAllBlocks();

            try
            {
                writer.Write(Application.version);
                writer.Write(blocks.Length * 3);
                for (int i = 0; i < blocks.Length; i++)
                {
                    writer.Write(blocks[i].x);
                    writer.Write(blocks[i].y);
                    writer.Write(blocks[i].z);
                }
            }
            catch (Exception exception)
            {
                message.Send(exception.Message, Color.red);
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            try
            {
                string fileVersion = reader.ReadString();
                if (fileVersion != Application.version)
                    message.Send($"Loading from a different version. This might cause problems. \nNEW: {Application.version} | OLD: {fileVersion}", Color.yellow);

                int count = reader.ReadInt32();
                Vector3Int pos = Vector3Int.zero;
                int axisCounter = 0;
                for (int i = 0; i < count; i++)
                {
                    int coord = reader.ReadInt32();
                    switch (axisCounter)
                    {
                        case 0: // X. ===
                            pos.x = coord;
                            break;
                        case 1: // Y. ===
                            pos.y = coord;
                            break;
                        case 2: // Z. ===
                            pos.z = coord;
                            Add(pos);
                            axisCounter = -1;
                            break;
                        default:
                            break;
                    }

                    axisCounter++;
                }
            }
            catch (Exception exception)
            {
                Clear();
                message.Send(exception.Message, Color.red);
            }
        }
    }
}
