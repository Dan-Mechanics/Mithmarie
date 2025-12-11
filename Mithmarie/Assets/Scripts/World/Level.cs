using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mithmarie
{
    public class Level : IBinarySerializable
    {
        private readonly List<IBinarySerializable> serializables;

        public Level(List<IBinarySerializable> serializables)
        {
            this.serializables = serializables;
        }

        public void Deserialize(BinaryReader reader, IMessageService message)
        {
            string fileVersion = reader.ReadString();
            if (fileVersion != Application.version)
                message.Send($"Loading from a different version. This might cause problems. \nNEW: {Application.version} | OLD: {fileVersion}", Color.yellow);

            serializables.ForEach(x => x.Deserialize(reader, message));
        }

        public void Serialize(BinaryWriter writer, IMessageService message)
        {
            writer.Write(Application.version);
            serializables.ForEach(x => x.Serialize(writer, message));
        }
    }
}
