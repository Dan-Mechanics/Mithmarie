using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public interface IExportStrategy
    {
        void Export(string path, Mesh mesh, IMessageService message);
        void ExportAsChunks(string path, List<Mesh> meshes, IMessageService message);
        string GetShortName();
        string GetWholeName();
    }
}
