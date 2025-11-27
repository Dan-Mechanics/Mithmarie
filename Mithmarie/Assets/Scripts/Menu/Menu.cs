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

        private IMessageService message;
        private readonly FSM fsm = new FSM();

        private void Start()
        {
            message = ServiceLocator<IMessageService>.Locate();
            fileScreen.Setup(FindAnyObjectByType<World>(), new OBJ(), new GreedyMeshGenerator(), message);
            settingsScreen.Setup(message);

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

            fileScreen.OnDoneWithTask += Close;
            settingsScreen.OnDoneWithTask += Close;
            fsm.Open(fileScreen);

            fileScreen.Button.onClick.AddListener(() => { fsm.Open(fileScreen); });
            settingsScreen.Button.onClick.AddListener(() => { fsm.Open(settingsScreen); });
        }

        public override void Exit()
        {
            base.Exit();
            gameObject.SetActive(false);

            fsm.Close();
            quitButton.onClick.RemoveListener(Application.Quit);
            closeButton.onClick.RemoveListener(Close);
            fileScreen.OnDoneWithTask -= Close;
            settingsScreen.OnDoneWithTask -= Close;

            fileScreen.Button.onClick.RemoveListener(() => { fsm.Open(fileScreen); });
            settingsScreen.Button.onClick.RemoveListener(() => { fsm.Open(settingsScreen); });
        }
    }
}