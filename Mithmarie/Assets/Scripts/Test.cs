using UnityEngine;

namespace Mithmarie
{
    public class Test : MonoBehaviour
    {
        private void Start()
        {
            for (int i = 0; i < 100; i++)
            {
                print(i / 16);
            }
            
            /*Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
            Wavefront wavefront = new Wavefront();

            wavefront.Export(@"C:\Users\Dan-Mechanics\3D Objects\mesh.obj", mesh, ServiceLocator<IMessageService>.Locate());*/
        }
    }
}
