using System.IO;

namespace Mitholca
{
    public interface IBinarySerializable 
    {
        void Serialize(BinaryWriter writer);
        void Deserialize(BinaryReader reader);
    }
}
