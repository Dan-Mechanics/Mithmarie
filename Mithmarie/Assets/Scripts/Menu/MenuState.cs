using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Mithmarie
{
    public class MenuState : StateBehaviour
    {
        [SerializeField] private FileScreen fileScreen = default;
        [SerializeField] private SettingsScreen settingsScreen = default;
        [SerializeField] private Screen[] screens = default;
        [Space(15)]
        [SerializeField] private Button closeButton = default;
        [SerializeField] private Button quitButton = default;

        private IMessageService message;
        private bool wantsToClose;
        private readonly FSM fsm = new FSM();
        private IState desiredScreen;

        private void Start()
        {
            message = ServiceLocator<IMessageService>.Locate();
            fileScreen.Setup(FindAnyObjectByType<World>(), new OBJ(), new GreedyMeshGenerator(), message);
            settingsScreen.Setup(message);

            fsm.AddState(fileScreen);
            fsm.AddState(settingsScreen);
            fsm.AddTransition(new Transition(fileScreen, settingsScreen, WantsNextPage));
            fsm.AddTransition(new Transition(settingsScreen, fileScreen, WantsNextPage));
        }

        public override void OnFrame()
        {
            base.OnFrame();
            fsm.Update();

            // !FIX
            if (Keyboard.current[GameManager.TOGGLE_STATE_KEY].wasPressedThisFrame)
                Close();
        }

        public override void OnTick()
        {
            base.OnTick();
            fsm.FixedUpdate();
        }

        private void Close() => wantsToClose = true;
        public bool GetWantsToClose() => wantsToClose;

        public override void Enter()
        {
            base.Enter();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            message.Send(string.Empty, Color.clear);
            gameObject.SetActive(true);

            quitButton.onClick.AddListener(Application.Quit);
            closeButton.onClick.AddListener(Close);

            fileScreen.OnDone += Close;
            fsm.Open(fileScreen);
            desiredScreen = fileScreen;

            fileScreen.Button.onClick.AddListener(() => { desiredScreen = fileScreen; });
            settingsScreen.Button.onClick.AddListener(() => { desiredScreen = settingsScreen; });
        }

        private bool WantsNextPage() => desiredScreen != fsm.Current;

        public override void Exit()
        {
            base.Exit();
            gameObject.SetActive(false);
            wantsToClose = false;

            fsm.Close();
            quitButton.onClick.RemoveListener(Application.Quit);
            closeButton.onClick.RemoveListener(Close);
            fileScreen.OnDone -= Close;

            fileScreen.Button.onClick.RemoveListener(() => { desiredScreen = fileScreen; });
            settingsScreen.Button.onClick.RemoveListener(() => { desiredScreen = settingsScreen; });
        }
    }
}