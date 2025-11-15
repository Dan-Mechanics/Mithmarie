using UnityEngine;
using UnityEngine.InputSystem;

namespace Mitholca
{
    public class GameManager : MonoBehaviour
    {
        public const int VERSION = 2;
        public const Key TOGGLE_STATE_KEY = Key.Escape;

        private readonly FSM fsm = new FSM();

        private void Start()
        {
            ServiceLocator<IMessageService>.Locate().Send("Welcome!\nUse [RMB] to place blocks.", Color.black);

            World world = FindAnyObjectByType<World>();
            world.Add(Vector3Int.zero);
            world.Flush();
            
            Player playerState = FindAnyObjectByType<Player>();
            Menu menuState = FindAnyObjectByType<Menu>();

            fsm.AddState(playerState);
            fsm.AddState(menuState);
            fsm.AddTransition(new Transition(playerState, menuState, playerState.GetShouldReturnToMenu));
            fsm.AddTransition(new Transition(menuState, playerState, menuState.GetShouldReturnToPlayer));

            menuState.Exit();
            fsm.Open(playerState);
        }

        private void Update() => fsm.Update();
        private void FixedUpdate() => fsm.FixedUpdate();

    }
}