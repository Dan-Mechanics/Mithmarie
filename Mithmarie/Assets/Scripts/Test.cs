using UnityEngine;

namespace Mithmarie
{
    public class Test : MonoBehaviour
    {
        private void Start()
        {
            Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
            Wavefront wavefront = new Wavefront();

            wavefront.Export(@"C:\Users\Dan-Mechanics\3D Objects\mesh.obj", mesh, ServiceLocator<IMessageService>.Locate());
        }
    }
}
