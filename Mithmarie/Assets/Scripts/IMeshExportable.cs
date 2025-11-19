using UnityEngine;

namespace Mithmarie
{
    public interface IMeshExportable
    {
        void Export(string path, Mesh mesh);
    }
}
