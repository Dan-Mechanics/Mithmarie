using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class PlayerMovement : StateBehaviour
    {
        [SerializeField, Min(0f)] private float sprintSpeedMult = default;
        [SerializeField] private Key sprintKey = default;
        [SerializeField] private Key downKey = default;
        [SerializeField] private PersistentFloat speed = default;

        public override void Enter()
        {
            base.Enter();
            speed.Load();
        }

        public override void OnFrame()
        {
            Vector3 movement = GetMovement();
            float currentSpeed = Keyboard.current[sprintKey].isPressed ? speed.Value * sprintSpeedMult : speed.Value;

            transform.Translate(currentSpeed * Time.deltaTime * movement, Space.Self);
        }

        private Vector3 GetMovement() 
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

            Vector3 result = new Vector3(x, 0f, z);
            result.Normalize();

            float y = 0f;
            if (Keyboard.current.spaceKey.isPressed)
                y++;

            if (Keyboard.current[downKey].isPressed)
                y--;

            result.y = y;
            return result;
        }
    }
}
