using System;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class KeyboardShortcuts : StateBehaviour
    {
        public event Action OnSave;
        public event Action OnUndo;
        public event Action OnRedo;

        public override void OnFrame()
        {
            base.OnFrame();
            if (!Keyboard.current.leftCtrlKey.isPressed)
                return;

            if (Keyboard.current.sKey.wasPressedThisFrame)
                OnSave?.Invoke();

            if (Keyboard.current.zKey.wasPressedThisFrame)
                (Keyboard.current.leftShiftKey.isPressed ? OnRedo : OnUndo)?.Invoke();
        }
    }
}
