using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public interface IExportStrategy
    {
        void Export(string path, MeshData mesh, IMessageService message);
        void ExportAsChunks(string path, List<MeshData> meshes, IMessageService message);
        string GetShortName();
        string GetWholeName();
    }
}
