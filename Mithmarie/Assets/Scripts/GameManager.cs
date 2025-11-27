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
            world.Setup(message);
            WorldHistory history = FindAnyObjectByType<WorldHistory>();
            KeyboardShortcuts shortcuts = FindAnyObjectByType<KeyboardShortcuts>();

            shortcuts.OnSave += FindAnyObjectByType<FileScreen>().Save;
            shortcuts.OnUndo += history.Undo;
            shortcuts.OnRedo += history.Redo;

            world.OnClear += history.Clear;
            world.OnFlush += history.LogImplicitWorldChange;

            world.OnDrawChunk += FindAnyObjectByType<ChunkManager>().DrawChunk;

            world.Add(Vector3Int.zero);
            world.ClearCaches();
            world.Flush();

            Player player = FindAnyObjectByType<Player>();
            Menu menu = FindAnyObjectByType<Menu>();

            menu.Setup();

            fsm.AddState(player);
            fsm.AddState(menu);
            fsm.AddTransition(new Transition(player, menu));
            fsm.AddTransition(new Transition(menu, player));

            fsm.Open(player);
        }

        private void Update() => fsm.Update();
        private void FixedUpdate() => fsm.FixedUpdate();

        // DO THE INVERSE OF EVERYTHING ON DESTROY ???
        private void OnDestroy()
        {
            // ...
        }
    }
}