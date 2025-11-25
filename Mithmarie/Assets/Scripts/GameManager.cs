using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// FIX !! not solid
        /// </summary>
        public const Key TOGGLE_STATE_KEY = Key.Escape;
        private readonly FSM fsm = new FSM();

        private void Start()
        {
            IMessageService message = ServiceLocator<IMessageService>.Locate();
            message.Send("[WASD] for movement and [MOUSE] for looking.\nUse [RMB] to place blocks, [LMB] to destroy.", Color.black);

            // SOMETHING LIKE THE FOLLOWING !!
            FindAnyObjectByType<KeyboardShortcuts>().OnUndo += FindAnyObjectByType<WorldHistory>().Undo;
            FindAnyObjectByType<KeyboardShortcuts>().OnRedo += FindAnyObjectByType<WorldHistory>().Redo;

            FindAnyObjectByType<World>().OnAdd += FindAnyObjectByType<WorldHistory>().LogAdd;
            FindAnyObjectByType<World>().OnRemove += FindAnyObjectByType<WorldHistory>().LogRemove;

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
            // NOTE: THIS MIGHT BE AN ORDERING PROBLEM SOON
            fsm.Open(playerState);
        }

        private void Update() => fsm.Update();
        private void FixedUpdate() => fsm.FixedUpdate();
    }
}