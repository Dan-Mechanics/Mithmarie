using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class GameManager : MonoBehaviour
    {
        public const Key TOGGLE_STATE_KEY = Key.Escape;

        private readonly FSM fsm = new FSM();

        private void Start()
        {
            ServiceLocator<IMessageService>.Locate().Send("[WASD] for movement and [MOUSE] for looking.\nUse [RMB] to place blocks, [LMB] to destroy.", Color.black);

            World world = FindAnyObjectByType<World>();
            world.Add(Vector3Int.zero);
            world.Flush();
            
            Player playerState = FindAnyObjectByType<Player>();
            Menu menuState = FindAnyObjectByType<Menu>();

            fsm.AddState(playerState);
            fsm.AddState(menuState);
            fsm.AddTransition(new Transition(playerState, menuState, playerState.GetWantsToClose));
            fsm.AddTransition(new Transition(menuState, playerState, menuState.GetWantsToClose));

            menuState.Exit();
            fsm.Open(playerState);
        }

        private void Update() => fsm.Update();
        private void FixedUpdate() => fsm.FixedUpdate();
    }
}