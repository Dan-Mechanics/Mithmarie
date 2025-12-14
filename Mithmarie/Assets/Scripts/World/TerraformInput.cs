using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class TerraformInput : StateBehaviour
    {
        public event Action OnAddPressed;
        public event Action OnAddReleased;
        public event Action OnRemovePressed;
        public event Action OnRemoveReleased;

        [SerializeField] private PersistentBool swapMouseButtons = default;

        public override void Enter()
        {
            base.Enter();
            swapMouseButtons.Load();
        }

        public override void OnFrame()
        {
            base.OnFrame();
            if (!swapMouseButtons.value)
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                    OnAddPressed?.Invoke();

                if (Mouse.current.leftButton.wasReleasedThisFrame)
                    OnAddReleased?.Invoke();

                if (Mouse.current.rightButton.wasPressedThisFrame)
                    OnRemovePressed?.Invoke();

                if (Mouse.current.rightButton.wasReleasedThisFrame)
                    OnRemoveReleased?.Invoke();
            }
            else
            {
                if (Mouse.current.rightButton.wasPressedThisFrame)
                    OnAddPressed?.Invoke();

                if (Mouse.current.rightButton.wasReleasedThisFrame)
                    OnAddReleased?.Invoke();

                if (Mouse.current.leftButton.wasPressedThisFrame)
                    OnRemovePressed?.Invoke();

                if (Mouse.current.leftButton.wasReleasedThisFrame)
                    OnRemoveReleased?.Invoke();
            }
        }
    }
}
