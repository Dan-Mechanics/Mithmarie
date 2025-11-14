using UnityEngine;

namespace Mitholca
{
    /// <summary>
    /// Handles: saving, loading, closing, settings, fill, circle, wall commands
    /// </summary>
    public class Menu : StateBehaviour
    {
        /*[SerializeField] private MouseMovement mouseMovement = default;
        [SerializeField] private PlayerMovement playerMovement = default;
        [SerializeField] private WorldEditor worldEditor = default;

        public override void OnFrame()
        {
            base.OnFrame();
            mouseMovement.OnTick();
            playerMovement.OnTick();
            worldEditor.OnTick();
        }*/

        public override void Enter()
        {
            base.Enter();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}