using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class FieldOfView : StateBehaviour
    {
        [SerializeField] private Camera cam = default;
        [SerializeField] private PersistentFloat fov = default;
        [SerializeField, Min(0f)] private float fovPerScroll = default;
        [SerializeField] private float minValue = default;
        [SerializeField] private float maxValue = default;

        public override void Enter()
        {
            base.Enter();
            fov.Load();
            cam.fieldOfView = fov.value;
        }

        public override void OnFrame()
        {
            base.OnFrame();
            float value = -Mouse.current.scroll.value.y;
            if (value > 0f)
            {
                fov.value += fovPerScroll;
            }
            else if (value < 0f)
            {
                fov.value -= fovPerScroll;
            }
            else
            {
                return;
            }

            fov.value = Mathf.Clamp(fov.value, minValue, maxValue);
            cam.fieldOfView = fov.value;
        }

        public override void Exit()
        { 
            base.Exit();
            fov.Save();
        }
    }
}
