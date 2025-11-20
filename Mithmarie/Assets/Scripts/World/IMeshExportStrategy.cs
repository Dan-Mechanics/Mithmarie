using UnityEngine;

namespace Mithmarie
{
    public interface IMeshExportStrategy
    {
        void Export(string path, Mesh mesh, IMessageService message);
    }
}
