using UnityEngine;
using UnityEngine.InputSystem;

namespace Mitholca
{
    public class GameManager : MonoBehaviour
    {
        public const int VERSION = 1;
        public const Key TOGGLE_KEY = Key.E;

        private readonly FSM fsm = new FSM();

        private void Start()
        {
            ServiceLocator<IMessageService>.Locate().Send("Welcome!\nUse [RMB] to place blocks.", Color.cyan);
            //FindAnyObjectByType<World>().Add(Vector3Int.zero);

            Player playerState = FindAnyObjectByType<Player>();
            Menu menuState = FindAnyObjectByType<Menu>();

            fsm.AddState(playerState);
            fsm.AddState(menuState);
            fsm.AddTransition(new Transition(playerState, menuState, CheckShouldToggle));
            fsm.AddTransition(new Transition(menuState, playerState, CheckShouldToggle));

            menuState.Exit();
            fsm.Open(playerState);
        }

        /// <summary>
        /// This neesd to be given to the states theneskeves eventually.
        /// </summary>
        private bool CheckShouldToggle()
        {
            return Keyboard.current[TOGGLE_KEY].wasPressedThisFrame;
        }

        private void Update() => fsm.Update();
        private void FixedUpdate() => fsm.FixedUpdate();

    }
}