using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Mithmarie
{
    public class Menu : StateBehaviour
    {
        [SerializeField] private Button fileButton = default;
        [SerializeField] private Button settingsButton = default;

        [SerializeField] private FilePage fileScreen = default;
        [SerializeField] private SettingsPage settingsScreen = default;
        [Space(15)]
        [SerializeField] private Button closeButton = default;
        [SerializeField] private Button quitButton = default;

        private IMessageService message;
        private bool wantsToClose;
        private readonly FSM fsm = new FSM();
        private IState currentHover;

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

            fileButton.onClick.AddListener(() => { currentHover = fileScreen; });
            settingsButton.onClick.AddListener(() => { currentHover = settingsScreen; });
            fileScreen.OnDone += Close;
            fsm.Open(fileScreen);
            currentHover = fileScreen;
        }

        private bool WantsNextPage() => currentHover != fsm.Current;

        public override void Exit()
        {
            base.Exit();
            gameObject.SetActive(false);
            wantsToClose = false;

            fsm.Close();
            quitButton.onClick.RemoveListener(Application.Quit);
            closeButton.onClick.RemoveListener(Close);
            fileScreen.OnDone -= Close;
            fileButton.onClick.RemoveListener(() => { currentHover = fileScreen; });
            settingsButton.onClick.RemoveListener(() => { currentHover = settingsScreen; });
        }

    }
}