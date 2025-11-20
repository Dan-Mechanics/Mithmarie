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

        public void DebugDobule()
        {
            Vector3Int[] temp = blocks.ToArray();
            for (int i = 0; i < temp.Length; i++)
            {
                Add(temp[i] + (Vector3Int.forward * 10));
            }

            Flush();
        }

        public void Add(Vector3Int pos) => blocks.Add(pos);
        public void Remove(Vector3Int pos) => blocks.Remove(pos);
        public void Clear() => blocks.Clear();
        public bool Has(Vector3Int pos) => blocks.Contains(pos);
        public void Flush() => generatable.GenerateMesh(blocks);

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
