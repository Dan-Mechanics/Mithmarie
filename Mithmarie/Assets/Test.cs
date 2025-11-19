using UnityEngine;

namespace Mithmarie
{
    public class Test : MonoBehaviour
    {
        public Color colorA = Color.white;
        public Color colorB = Color.white;

        private void Start()
        {
            Color sum = colorA + colorB;
            print(sum);
        }
    }
}
