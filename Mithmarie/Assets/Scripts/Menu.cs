using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Mithmarie
{
    /// <summary>
    /// Handles: saving, loading, closing, settings, fill, circle, wall commands
    /// </summary>
    public class Menu : StateBehaviour
    {
        [SerializeField] private Hover filePageHover = default;
        [SerializeField] private Button closeButton = default;
        [SerializeField] private Button quitButton = default;

        private IMessageService message;
        private bool wantsToClose;

        private void Start()
        {
            message = ServiceLocator<IMessageService>.Locate();
        }

        public override void OnFrame()
        {
            base.OnFrame();
            if (Keyboard.current[GameManager.TOGGLE_STATE_KEY].wasPressedThisFrame)
                Close();
        }

        private void Close() => wantsToClose = true;
        public bool GetWantsToClose() => wantsToClose;

        public override void Enter()
        {
            base.Enter();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            gameObject.SetActive(true);

            quitButton.onClick.AddListener(Application.Quit);

            closeButton.onClick.AddListener(Close);

            filePageHover.OnHover += FilePageHover_OnHover;

            message.Send(string.Empty, Color.clear);
        }

        private void FilePageHover_OnHover()
        {
            Debug.Log("test");
        }

        public override void Exit()
        {
            base.Exit();
            gameObject.SetActive(false);
            wantsToClose = false;


            quitButton.onClick.RemoveListener(Application.Quit);

            filePageHover.OnHover -= FilePageHover_OnHover;

            closeButton.onClick.RemoveListener(Close);
        }

    }
}