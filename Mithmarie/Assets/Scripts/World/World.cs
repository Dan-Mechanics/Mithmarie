using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mithmarie
{
    public class World : MonoBehaviour, IBinarySerializable
    {
        public const int CHUNK_SIZE = 8;

        public event Action<Vector3Int, Dictionary<Vector3Int, HashSet<Vector3Int>>> OnDrawChunk;
        public event Action<HashSet<Vector3Int>, HashSet<Vector3Int>> OnNewChanges;
        public event Action OnClear;
        
        private readonly Dictionary<Vector3Int, HashSet<Vector3Int>> chunks = new Dictionary<Vector3Int, HashSet<Vector3Int>>();
        private readonly HashSet<Vector3Int> changedChunkPositions = new HashSet<Vector3Int>();

        private readonly HashSet<Vector3Int> addedBlocksCache = new HashSet<Vector3Int>();
        private readonly HashSet<Vector3Int> removedBlocksCache = new HashSet<Vector3Int>();

        private bool rememberTheFollowing;

        public void Add(Vector3Int blockPos)
        {
            Vector3Int chunkPos = Utils.GetChunkPos(blockPos, CHUNK_SIZE);
            if (!chunks.TryGetValue(chunkPos, out HashSet<Vector3Int> blocks))
            {
                blocks = new HashSet<Vector3Int>();
                chunks[chunkPos] = blocks;
            }

            if (blocks.Contains(blockPos))
                return;

            blocks.Add(blockPos);
            NotifyChunkChange(chunkPos);

            if (rememberTheFollowing)
                addedBlocksCache.Add(blockPos);
        }

        public void Remove(Vector3Int blockPos)
        {
            Vector3Int chunkPos = Utils.GetChunkPos(blockPos, CHUNK_SIZE);
            if (!chunks.TryGetValue(chunkPos, out HashSet<Vector3Int> blocks))
                return;

            if (!blocks.Contains(blockPos))
                return;

            blocks.Remove(blockPos);
            NotifyChunkChange(chunkPos);

            if (rememberTheFollowing)
                removedBlocksCache.Add(blockPos);
        }

        public void Clear()
        {
            foreach (Vector3Int chunkPos in chunks.Keys)
            {
                changedChunkPositions.Add(chunkPos);
            }

            chunks.Clear();
            OnClear?.Invoke();
        }

        /// <summary>
        /// If you call this, all the work done 
        /// since the previous Flush() cannot be undone.
        /// </summary>
        private void ForgetRecentChanges()
        {
            addedBlocksCache.Clear();
            removedBlocksCache.Clear();
        }

        public void RememberTheFollowing() => rememberTheFollowing = true;

        public void Flush()
        {
            foreach (Vector3Int chunkPos in changedChunkPositions)
            {
                if (!chunks.ContainsKey(chunkPos) || chunks[chunkPos] == null || chunks[chunkPos].Count <= 0)
                    chunks.Remove(chunkPos);

                OnDrawChunk?.Invoke(chunkPos, chunks);
            }

            rememberTheFollowing = false;
            changedChunkPositions.Clear();

            if (addedBlocksCache.Count <= 0 && removedBlocksCache.Count <= 0)
                return;

            OnNewChanges?.Invoke(addedBlocksCache, removedBlocksCache);
            ForgetRecentChanges();
        }

        public HashSet<Vector3Int> GetAllBlocks()
        {
            HashSet<Vector3Int> blocks = new HashSet<Vector3Int>();
            foreach (HashSet<Vector3Int> chunk in chunks.Values)
            {
                blocks.UnionWith(chunk);
            }

            return blocks;
        }

        public Dictionary<Vector3Int, HashSet<Vector3Int>> GetChunks() => chunks;

        public bool Has(Vector3Int blockPos)
        {
            Vector3Int chunkPos = Utils.GetChunkPos(blockPos, CHUNK_SIZE);
            if (!chunks.ContainsKey(chunkPos))
                return false;

            return chunks[chunkPos].Contains(blockPos);
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
        
        public void Serialize(BinaryWriter writer, IMessageService message)
        {
            int blockCount = 0;
            foreach (KeyValuePair<Vector3Int, HashSet<Vector3Int>> chunk in chunks)
            {
                blockCount += chunk.Value.Count;
            }

            try
            {
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
        
        public void Deserialize(BinaryReader reader, IMessageService message)
        {
            try
            {
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
