using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class PlayerMovement : StateBehaviour
    {
        [SerializeField, Min(0f)] private float sprintMultiplyer = default;
        [SerializeField] private PersistentFloat speed = default;
        [SerializeField] private string moveName = default;
        [SerializeField] private string jumpName = default;
        [SerializeField] private string crouchName = default;
        [SerializeField] private string sprintName = default;

        private InputAction moveAction;
        private InputAction jumpAction;
        private InputAction crouchAction;
        private InputAction sprintAction;

        public void Setup()
        {
            moveAction = InputSystem.actions.FindAction(moveName);
            jumpAction = InputSystem.actions.FindAction(jumpName);
            crouchAction = InputSystem.actions.FindAction(crouchName);
            sprintAction = InputSystem.actions.FindAction(sprintName);
        }

        public override void Enter()
        {
            base.Enter();
            speed.Load();
        }

        public override void OnFrame()
        {
            Vector3 movement = GetMovementInput();
            float currentSpeed = speed.Value;
            if (sprintAction.IsPressed())
                currentSpeed *= sprintMultiplyer;

            transform.Translate(currentSpeed * Time.deltaTime * movement, Space.Self);
        }

        private Vector3 GetMovementInput() 
        {
            Vector2 wasd = moveAction.ReadValue<Vector2>();
            wasd.Normalize();

            float y = 0f;
            if (jumpAction.IsPressed())
                y++;

            if (crouchAction.IsPressed())
                y--;

            return new Vector3(wasd.x, y, wasd.y);
        }
    }
}
