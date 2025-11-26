using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class PlayerMovement : StateBehaviour
    {
        [SerializeField, Min(0f)] private float sprintSpeedMult = default;
        [SerializeField] private PersistentFloat speed = default;

        public override void Enter()
        {
            base.Enter();
            speed.Load();
        }

        public override void OnFrame()
        {
            Vector3 movement = GetInputDirection();
            movement = transform.TransformDirection(movement);
            movement.Normalize();

            movement *= speed.Value;

            if (Keyboard.current.leftCtrlKey.isPressed)
                movement *= sprintSpeedMult;

            transform.Translate(movement * Time.deltaTime, Space.World);
        }

        private Vector3 GetInputDirection() 
        {
            float z = 0f;
            if (Keyboard.current.wKey.isPressed)
                z++;

            if (Keyboard.current.sKey.isPressed)
                z--;

            float x = 0f;
            if (Keyboard.current.dKey.isPressed)
                x++;

            if (Keyboard.current.aKey.isPressed)
                x--;

            float y = 0f;
            if (Keyboard.current.spaceKey.isPressed)
                y++;

            if (Keyboard.current.leftShiftKey.isPressed)
                y--;

            return new Vector3(x, y, z);
        }
    }
}
