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
            message.Send("[WASD] for movement and [MOUSE] for looking.\nUse [RMB] to place blocks, [LMB] to destroy.", Color.black, 4f);

            World world = FindAnyObjectByType<World>();
            WorldHistory history = FindAnyObjectByType<WorldHistory>();
            KeyboardShortcuts shortcuts = FindAnyObjectByType<KeyboardShortcuts>();

            shortcuts.OnSave += FindAnyObjectByType<FileScreen>().Save;
            shortcuts.OnUndo += history.Undo;
            shortcuts.OnRedo += history.Redo;

            world.OnAdd += history.EnscribeAddCommand;
            world.OnRemove += history.EnscribeRemoveCommand;

            world.OnClear += history.Clear;

            world.OnDrawChunk += FindAnyObjectByType<WorldVisualizer>().DrawChunk;

            world.Add(Vector3Int.zero);
            world.ClearCaches();
            world.Flush();

            PlayerState playerState = FindAnyObjectByType<PlayerState>();
            MenuState menuState = FindAnyObjectByType<MenuState>();

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