using UnityEngine;
using UnityEngine.UI;

namespace Mitholca
{
    public class Overlay : MonoBehaviour
    {
        [SerializeField] private Image image = default;
        private IFocus focus;

        private void Start()
        {
            focus = ServiceLocator<IFocus>.Locate();
        }

        private void FixedUpdate()
        {
            image.enabled = !focus.HasFocus();
        }
    }
}
