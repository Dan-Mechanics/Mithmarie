using UnityEngine;
using UnityEngine.InputSystem;

namespace Mitholca
{
    public class GameManager : MonoBehaviour
    {
        private readonly FSM fsm = new FSM();

        private void Start()
        {
            ServiceLocator<IMessageService>.Locate().Send("Welcome", Color.gray);

            Player playerState = FindAnyObjectByType<Player>();
            Menu menuState = FindAnyObjectByType<Menu>();

            fsm.AddState(playerState);
            fsm.AddState(menuState);
            fsm.AddTransition(new Transition(playerState, menuState, CheckShouldToggle));
            fsm.AddTransition(new Transition(menuState, playerState, CheckShouldToggle));

            fsm.Enter(playerState);
        }

        /// <summary>
        /// This neesd to be given to the states theneskeves eventually.
        /// </summary>
        /// <returns></returns>
        private bool CheckShouldToggle()
        {
            return Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.eKey.wasPressedThisFrame;
        }

        private void Update() => fsm.Update();
        private void FixedUpdate() => fsm.FixedUpdate();

    }
}