using System.IO;

namespace Mithmarie
{
    public interface IBinarySerializable 
    {
        void Serialize(BinaryWriter writer, IMessageService message);
        void Deserialize(BinaryReader reader, IMessageService message);
    }
}
