using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mithmarie
{
    public class Level : IBinarySerializable
    {
        private readonly List<IBinarySerializable> levelObjects;

        public Level(List<IBinarySerializable> levelObjects)
        {
            this.levelObjects = levelObjects;
        }

        public void Deserialize(BinaryReader reader, IMessageService message)
        {
            try
            {
                string fileVersion = reader.ReadString();
                if (fileVersion != Application.version)
                    message.Send($"Loading from a different version. This might cause problems. \nNEW: {Application.version} | OLD: {fileVersion}", Color.yellow);

                levelObjects.ForEach(x => x.Deserialize(reader, message));
            }
            catch (Exception exception)
            {
                message.Send(exception.Message, Color.red);
            }
        }

        public void Serialize(BinaryWriter writer, IMessageService message)
        {
            try
            {
                writer.Write(Application.version);
                levelObjects.ForEach(x => x.Serialize(writer, message));
            }
            catch (Exception exception)
            {
                message.Send(exception.Message, Color.red);
            }
        }
    }
}
