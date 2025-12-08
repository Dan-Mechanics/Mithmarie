using UnityEngine;

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
        private PopupManager popupManager;
        private Player player;
        private PlayerDisplay playerDisplay;
        private Menu menu;
        private Transform eyes;

        private void Awake()
        {
            world = FindAnyObjectByType<World>();
            history = FindAnyObjectByType<WorldHistory>();
            shortcuts = FindAnyObjectByType<KeyboardShortcuts>();
            fileScreen = FindAnyObjectByType<FileScreen>();
            chunkVisualizationManager = FindAnyObjectByType<ChunkVisualizationManager>();
            player = FindAnyObjectByType<Player>();
            playerDisplay = FindAnyObjectByType<PlayerDisplay>();
            menu = FindAnyObjectByType<Menu>();

            eyes = GameObject.FindWithTag("MainCamera").transform;
            popupManager = FindAnyObjectByType<PopupManager>();
        }
        
        private void Start()
        {
            popupManager.Setup();
            playerDisplay.Setup();

            shortcuts.OnSave += fileScreen.Save;
            shortcuts.OnUndo += history.Undo;
            shortcuts.OnRedo += history.Redo;

            world.OnClear += history.Clear;
            world.OnNewChanges += history.LogImplicitWorldChange;

            chunkVisualizationManager.Setup(eyes);
            world.OnDrawChunk += chunkVisualizationManager.DrawChunk;
            history.Setup(world);

            world.Add(Vector3Int.zero);
            world.Flush();

            player.Setup(world, playerDisplay);
            menu.Setup();

            fsm.AddState(player);
            fsm.AddState(menu);
            fsm.AddTransition(new Transition(player, menu));
            fsm.AddTransition(new Transition(menu, player));

            fsm.Open(player);

            // ===

            ServiceLocator<IMessageService>.Locate().Send("[WASD] for movement and [MOUSE] for looking.\n" +
                "Use [RMB] to place blocks, [LMB] to destroy.", Color.black, 4f);
        }

        private void Update() => fsm.Update();
        private void FixedUpdate() => fsm.FixedUpdate();

        private void OnDestroy()
        {
            // ...
        }
    }
}