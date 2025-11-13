using UnityEngine;
using UnityEngine.InputSystem;

namespace Mitholca
{
    public class Focus : MonoBehaviour, IFocus
    {
        public bool hasFocus;

        private void Awake()
        {
            hasFocus = true;
            ServiceLocator<IFocus>.Provide(this);
        }

        private void Update()
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
                hasFocus = !hasFocus;

            Cursor.visible = !hasFocus;
            Cursor.lockState = hasFocus ? CursorLockMode.Locked : CursorLockMode.None;
        }

        private void OnApplicationFocus(bool hasFocus) => this.hasFocus = hasFocus;
        public bool HasFocus() => hasFocus;
    }
}
