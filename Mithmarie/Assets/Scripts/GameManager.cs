using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class GameManager : MonoBehaviour
    {
        private readonly FSM fsm = new FSM();

        private World world;
        private KeyboardShortcuts shortcuts;
        private WorldHistory history;
        private FileScreen fileScreen;
        private ChunkVisualizationManager chunkVisualizationManager;
        private Player player;
        private Menu menu;

        private void Awake()
        {
            world = FindAnyObjectByType<World>();
            history = FindAnyObjectByType<WorldHistory>();
            shortcuts = FindAnyObjectByType<KeyboardShortcuts>();
            fileScreen = FindAnyObjectByType<FileScreen>();
            chunkVisualizationManager = FindAnyObjectByType<ChunkVisualizationManager>();
            player = FindAnyObjectByType<Player>();
            menu = FindAnyObjectByType<Menu>();
        }
        
        private void Start()
        {
            IMessageService message = ServiceLocator<IMessageService>.Locate();
            message.Send("[WASD] for movement and [MOUSE] for looking.\nUse [RMB] to place blocks, [LMB] to destroy.", Color.black, 4f);

            shortcuts.OnSave += fileScreen.Save;
            shortcuts.OnUndo += history.Undo;
            shortcuts.OnRedo += history.Redo;

            world.OnClear += history.Clear;
            world.OnChange += history.LogImplicitWorldChange;

            world.OnDrawChunk += chunkVisualizationManager.DrawChunk;

            world.Add(Vector3Int.zero);
            world.ForgetRecentChanges();
            world.Flush();

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