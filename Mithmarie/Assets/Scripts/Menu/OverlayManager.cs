using UnityEngine;

namespace Mithmarie
{
    public class OverlayManager : MonoBehaviour
    {
        [SerializeField] private Fade green = default;
        [SerializeField] private Fade red = default;

        public void EnableGreen(bool active) => green.SetDirection(active);
        public void EnableRed(bool active) => red.SetDirection(active);
    }
}
