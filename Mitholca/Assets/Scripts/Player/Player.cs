using UnityEngine;

namespace Mitholca
{
    public class Player : StateBehaviour
    {
        [SerializeField] private MouseMovement mouseMovement = default;
        [SerializeField] private PlayerMovement playerMovement = default;
        [SerializeField] private WorldEditor worldEditor = default;

        public override void OnFrame()
        {
            base.OnFrame();
            mouseMovement.OnTick();
            playerMovement.OnTick();
            worldEditor.OnTick();
        }

        public override void Enter()
        {
            base.Enter();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}