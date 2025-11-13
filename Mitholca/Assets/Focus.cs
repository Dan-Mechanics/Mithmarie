using UnityEngine;
using UnityEngine.InputSystem;

namespace Mitholca
{
    public class Focus : MonoBehaviour, IFocus
    {
        public bool hasFocus;

        private IMessageService message;
        private bool prevFocus;


        private void Awake()
        {
            hasFocus = true;
            ServiceLocator<IFocus>.Provide(this);
        }

        private void Start()
        {
            message = ServiceLocator<IMessageService>.Locate();
        }

        private void Update()
        {
            if (GetShouldToggle())
                hasFocus = !hasFocus;

            if (Mouse.current.leftButton.wasPressedThisFrame)
                hasFocus = true;

            Cursor.visible = !hasFocus;
            Cursor.lockState = hasFocus ? CursorLockMode.Locked : CursorLockMode.None;

            /*if (prevFocus != hasFocus)
                message.Send(hasFocus ? "lock in" : "lock out", hasFocus ? Color.green : Color.red);
*/
            prevFocus = hasFocus;
        }

        private void OnApplicationFocus(bool hasFocus) => Request(hasFocus);
        public void Request(bool hasFocus) => this.hasFocus = hasFocus;
        public bool HasFocus() => hasFocus;

        private bool GetShouldToggle()
        {
            return Keyboard.current.escapeKey.wasPressedThisFrame ||
                Keyboard.current.eKey.wasPressedThisFrame ||
                Keyboard.current.slashKey.wasPressedThisFrame ||
                Keyboard.current.backquoteKey.wasPressedThisFrame;
        }

    }
}
