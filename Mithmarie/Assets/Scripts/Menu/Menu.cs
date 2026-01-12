using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Mithmarie
{
    public class Menu : StateBehaviour
    {
        [SerializeField] private FileScreen fileScreen = default;
        [SerializeField] private SettingsScreen settingsScreen = default;
        [SerializeField] private WikiScreen wikiScreen = default;
        [SerializeField] private Button closeButton = default;
        [SerializeField] private Button quitButton = default;
        [SerializeField] private string pauseName = default;

        private readonly FSM fsm = new FSM();
        private InputAction pauseAction;
        private IMessageService message;
        private Screen[] screens;
        private bool hasOpened;
        
        /// <summary>
        /// You could possiblely make it so GameManager injects the dependencies to the other states here.
        /// This might make it too unreadable though.
        /// </summary>
        public void Setup()
        {
            message = ServiceLocator<IMessageService>.Locate();
            settingsScreen.Setup();

            pauseAction = InputSystem.actions.FindAction(pauseName);
            screens = GetComponentsInChildren<Screen>(true);

            fsm.AddState(fileScreen);
            fsm.AddState(settingsScreen);
            fsm.AddState(wikiScreen);

            fsm.AddTransition(new Transition(fileScreen, settingsScreen));
            fsm.AddTransition(new Transition(fileScreen, wikiScreen));

            fsm.AddTransition(new Transition(settingsScreen, fileScreen));
            fsm.AddTransition(new Transition(settingsScreen, wikiScreen));

            fsm.AddTransition(new Transition(wikiScreen, fileScreen));
            fsm.AddTransition(new Transition(wikiScreen, settingsScreen));
        }

        public override void OnFrame()
        {
            base.OnFrame();
            fsm.Update();

            if (pauseAction.WasPressedThisFrame())
                Close();
        }

        public override void OnTick()
        {
            base.OnTick();
            fsm.FixedUpdate();
        }

        public override void Enter()
        {
            base.Enter();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            message.Send(string.Empty, Color.clear);
            gameObject.SetActive(true);

            quitButton.onClick.AddListener(Application.Quit);
            closeButton.onClick.AddListener(Close);

            fsm.Open(hasOpened ? fileScreen : wikiScreen);
            hasOpened = true;

            foreach (Screen screen in screens)
            {
                screen.Button.onClick.AddListener(() => { fsm.Open(screen); });
                screen.OnDoneWithTask += Close;
            }
        }

        public override void Exit()
        {
            base.Exit();
            gameObject.SetActive(false);

            fsm.Close();
            quitButton.onClick.RemoveListener(Application.Quit);
            closeButton.onClick.RemoveListener(Close);

            foreach (Screen screen in screens)
            {
                screen.Button.onClick.RemoveListener(() => { fsm.Open(screen); });
                screen.OnDoneWithTask -= Close;
            }
        }
    }
}