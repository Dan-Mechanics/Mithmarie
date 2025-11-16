using System.IO;

namespace Mithmarie
{
    public interface IBinarySerializable 
    {
        void Serialize(BinaryWriter writer);
        void Deserialize(BinaryReader reader);
    }
}
