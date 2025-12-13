using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Mithmarie
{
    public class Menu : StateBehaviour
    {
        [SerializeField] private FileScreen fileScreen = default;
        [SerializeField] private SettingsScreen settingsScreen = default;
        [Space(15)]
        [SerializeField] private Button closeButton = default;
        [SerializeField] private Button quitButton = default;

        private Screen[] screens;
        private IMessageService message;
        private readonly FSM fsm = new FSM();

        /// <summary>
        /// You could possiblely make it so GameManager injects the dependencies to the other states here.
        /// This might make it too unreadable thought.
        /// </summary>
        public void Setup(IBinarySerializable level)
        {
            message = ServiceLocator<IMessageService>.Locate();
            fileScreen.Setup(FindAnyObjectByType<World>(), new OBJ(), new GreedyWorldMesh(), level);
            settingsScreen.Setup();

            screens = GetComponentsInChildren<Screen>(true);

            fsm.AddState(fileScreen);
            fsm.AddState(settingsScreen);
            fsm.AddTransition(new Transition(fileScreen, settingsScreen));
            fsm.AddTransition(new Transition(settingsScreen, fileScreen));
        }

        public override void OnFrame()
        {
            base.OnFrame();
            fsm.Update();

            if (Keyboard.current.escapeKey.wasPressedThisFrame)
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

            fsm.Open(fileScreen);

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