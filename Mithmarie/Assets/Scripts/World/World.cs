using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mithmarie
{
    /// <summary>
    /// This class does too much, something has to go.
    /// </summary>
    public class World : MonoBehaviour, IBinarySerializable
    {
        public event Action<HashSet<Vector3Int>> OnAdd;
        public event Action<HashSet<Vector3Int>> OnRemove;
        
        public const int CHUNK_SIZE = 8;
        
        private readonly Dictionary<Vector3Int, HashSet<Vector3Int>> chunks = new Dictionary<Vector3Int, HashSet<Vector3Int>>();
        private readonly HashSet<Vector3Int> changedChunkPositions = new HashSet<Vector3Int>();
        
        private WorldVisualizer chunksVisualizer;
        private IMessageService message;

        private delegate void EditBlock(Vector3Int blockPos);
        private EditBlock editBlock;

        private readonly HashSet<Vector3Int> roamingAdds = new HashSet<Vector3Int>();
        private readonly HashSet<Vector3Int> roamingRemoves = new HashSet<Vector3Int>();

        private void Awake()
        {
            chunksVisualizer = FindAnyObjectByType<WorldVisualizer>();
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

        public void SilentAdd(Vector3Int blockPos)
        {
            Vector3Int chunkPos = Utils.GetChunkPos(blockPos, CHUNK_SIZE);
            if (!chunks.ContainsKey(chunkPos))
                chunks.Add(chunkPos, new HashSet<Vector3Int>());

            if (chunks[chunkPos].Contains(blockPos))
                return;

            chunks[chunkPos].Add(blockPos);
            NotifyChunkChange(chunkPos);
        }

        public void SilentRemove(Vector3Int blockPos)
        {
            Vector3Int chunkPos = Utils.GetChunkPos(blockPos, CHUNK_SIZE);
            if (!chunks.ContainsKey(chunkPos))
                return;

            if (!chunks[chunkPos].Contains(blockPos))
                return;

            chunks[chunkPos].Remove(blockPos);
            NotifyChunkChange(chunkPos);
        }

        public void Add(Vector3Int blockPos)
        {
            SilentAdd(blockPos);
            roamingAdds.Add(blockPos);
        }

        public void Remove(Vector3Int blockPos)
        {
            SilentRemove(blockPos);
            roamingRemoves.Add(blockPos);
        }

        public void Clear()
        {
            foreach (Vector3Int chunkPos in chunks.Keys)
            {
                changedChunkPositions.Add(chunkPos);
            }

            chunks.Clear();
        }

        public void Flush()
        {
            foreach (Vector3Int chunkPos in changedChunkPositions)
            {
                if(!chunks.ContainsKey(chunkPos) || chunks[chunkPos] == null || chunks[chunkPos].Count <= 0)
                {
                    chunks.Remove(chunkPos);
                    chunksVisualizer.DrawChunk(chunkPos, chunks);
                    continue;
                }

                chunksVisualizer.DrawChunk(chunkPos, chunks);
            }

            OnAdd?.Invoke(roamingAdds);
            OnRemove?.Invoke(roamingRemoves);
            roamingAdds.Clear();
            roamingRemoves.Clear();
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

        public HashSet<Vector3Int> GetAllBlocks()
        {
            HashSet<Vector3Int> blocks = new HashSet<Vector3Int>();
            foreach (HashSet<Vector3Int> chunk in chunks.Values)
            {
                foreach (Vector3Int blockPos in chunk)
                {
                    blocks.Add(blockPos);
                }
            }

            return blocks;
        }

        private void NotifyChunkChange(Vector3Int chunkPos)
        {
            changedChunkPositions.Add(chunkPos);
            changedChunkPositions.Add(chunkPos + Vector3Int.up);
            changedChunkPositions.Add(chunkPos + Vector3Int.down);
            changedChunkPositions.Add(chunkPos + Vector3Int.forward);
            changedChunkPositions.Add(chunkPos + Vector3Int.back);
            changedChunkPositions.Add(chunkPos + Vector3Int.left);
            changedChunkPositions.Add(chunkPos + Vector3Int.right);
        }
        
        public void Serialize(BinaryWriter writer)
        {
            int blockCount = 0;
            foreach (KeyValuePair<Vector3Int, HashSet<Vector3Int>> chunk in chunks)
            {
                blockCount += chunk.Value.Count;
            }

            try
            {
                writer.Write(Application.version);
                writer.Write(blockCount * 3);
                foreach (KeyValuePair<Vector3Int, HashSet<Vector3Int>> chunk in chunks)
                {
                    foreach (Vector3Int blockPos in chunk.Value)
                    {
                        writer.Write(blockPos.x);
                        writer.Write(blockPos.y);
                        writer.Write(blockPos.z);
                    }
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
