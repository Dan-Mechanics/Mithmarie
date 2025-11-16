using UnityEngine;
using UnityEngine.InputSystem;

namespace Mitholca
{
    public class PlayerMovement : StateBehaviour
    {
        public const Key SPRINT_KEY = Key.LeftCtrl;
        
        [SerializeField, Min(0f)] private float speed = default;
        [SerializeField, Min(0f)] private float sprintSpeedMult = default;

        public override void OnFrame()
        {
            Vector3 movement = GetInputDirection();
            movement = transform.TransformDirection(movement);
            movement.Normalize();

            movement *= speed;

            if (Keyboard.current[SPRINT_KEY].isPressed)
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
