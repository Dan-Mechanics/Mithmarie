using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class GameManager : MonoBehaviour
    {
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

            world.OnAdd += history.InscribeAddCommand;
            world.OnRemove += history.InscribeRemoveCommand;

            world.OnClear += history.Clear;

            world.OnDrawChunk += FindAnyObjectByType<WorldVisualizer>().DrawChunk;

            world.Add(Vector3Int.zero);
            world.ClearCaches();
            world.Flush();

            Player playerState = FindAnyObjectByType<Player>();
            Menu menuState = FindAnyObjectByType<Menu>();

            fsm.AddState(playerState);
            fsm.AddState(menuState);
            fsm.AddTransition(new Transition(playerState, menuState));
            fsm.AddTransition(new Transition(menuState, playerState));

            menuState.Exit();
            // NOTE: THIS MIGHT BE AN ORDERING PROBLEM SOON
            fsm.Open(playerState);
        }

        private void Update() => fsm.Update();
        private void FixedUpdate() => fsm.FixedUpdate();
    }
}