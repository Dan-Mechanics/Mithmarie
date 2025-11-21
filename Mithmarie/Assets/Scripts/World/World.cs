using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Mithmarie
{
    public class World : MonoBehaviour, IBinarySerializable
    {
        private readonly HashSet<Vector3Int> blocks = new HashSet<Vector3Int>();
        private IMeshGeneratable generatable;
        private IMessageService message;

        private void Awake()
        {
            generatable = FindAnyObjectByType<CulledMeshGenerator>();
        }

        private void Start()
        {
            message = ServiceLocator<IMessageService>.Locate();
        }

        public void Add(Vector3Int pos) => blocks.Add(pos);
        public void Remove(Vector3Int pos) => blocks.Remove(pos);
        public void Clear() => blocks.Clear();
        public bool Has(Vector3Int pos) => blocks.Contains(pos);
        public void Flush() => generatable.GenerateMesh(blocks);

        public void AddSelection(Vector3Int a, Vector3Int b)
        {
            if (a == b)
            {
                Add(a);
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
                        Add(a + temp);
                    }
                }
            }
        }

        public void RemoveSelecton(Vector3Int a, Vector3Int b)
        {
            if (a == b)
            {
                Remove(a);
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
                        Remove(a + temp);
                    }
                }
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            try
            {
                writer.Write(Application.version);
                writer.Write(blocks.Count * 3);
                foreach (Vector3Int pos in blocks)
                {
                    writer.Write(pos.x);
                    writer.Write(pos.y);
                    writer.Write(pos.z);
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
