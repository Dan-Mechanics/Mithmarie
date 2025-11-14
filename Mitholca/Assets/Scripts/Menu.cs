using UnityEngine;

namespace Mitholca
{
    /// <summary>
    /// Handles: saving, loading, closing, settings, fill, circle, wall commands
    /// </summary>
    public class Menu : StateBehaviour
    {

        public override void Enter()
        {
            base.Enter();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            gameObject.SetActive(true);
        }

        public override void Exit()
        {
            base.Exit();
            gameObject.SetActive(false);
        }
    }
}