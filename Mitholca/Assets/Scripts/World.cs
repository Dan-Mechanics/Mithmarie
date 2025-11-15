using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mitholca
{
    public class World : MonoBehaviour, IBinarySerializable
    {
        /// <summary>
        /// TODO: CHUNKS !!
        /// </summary>
        private readonly HashSet<Vector3Int> blocks = new HashSet<Vector3Int>();
        private IMeshGeneratable generatable;
        private IMessageService message;

        private void Awake()
        {
            generatable = FindAnyObjectByType<DemoMeshGenerator>();
        }

        private void Start()
        {
            message = ServiceLocator<IMessageService>.Locate();
        }

        public void Add(Vector3Int pos)
        {
            if (Has(pos))
                return;

            blocks.Add(pos);
        }

        public void Remove(Vector3Int pos) => blocks.Remove(pos);
        public void Clear() => blocks.Clear();
        public bool Has(Vector3Int pos) => blocks.Contains(pos);
        public void Flush() => generatable.GenerateMesh(blocks);

        public void Serialize(BinaryWriter writer)
        {
            // Flush();
            
            writer.Write(GameManager.VERSION);
            writer.Write(blocks.Count);
            foreach (Vector3Int block in blocks)
            {
                writer.Write(block.x);
                writer.Write(block.y);
                writer.Write(block.z);
            }
        }

        public void Deserialize(BinaryReader reader)
        {
           // Clear();

            int version = reader.ReadInt32();
            if(version != GameManager.VERSION)
                message.Send($"Loading from a different version. This might cause problems. \nNEW: {GameManager.VERSION} | OLD: {version}", Color.yellow);

            int blockCount = reader.ReadInt32();
            Vector3Int pos = Vector3Int.zero;
            int axisCounter = 0;
            for (int i = 0; i < blockCount; i++)
            {
                switch (axisCounter)
                {
                    case 0: // X. ===
                        pos.x = reader.ReadInt32();
                        break;
                    case 1: // Y. ===
                        pos.y = reader.ReadInt32();
                        break;
                    case 2: // Z. ===
                        pos.z = reader.ReadInt32();
                        Add(pos);
                        axisCounter = -1;
                        break;
                    default:
                        break;
                }

                axisCounter++;
            }

            //Flush();
        }
    }
}
