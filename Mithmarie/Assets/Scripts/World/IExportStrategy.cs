using UnityEngine;

namespace Mithmarie
{
    public interface IExportStrategy
    {
        void Export(string path, Mesh mesh, IMessageService message);
    }
}
