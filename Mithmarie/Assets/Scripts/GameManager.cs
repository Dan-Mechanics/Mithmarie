using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Brush[] brushes = default;
        private readonly FSM fsm = new FSM();        

        private ChunkVisualManager chunkVisualManager;
        private KeyboardShortcuts keyboardShortcuts;
        private OverlayManager overlayManager;
        private HoverHighlight hoverHighlight;
        private PlayerDisplay playerDisplay;
        private StairsPreview stairsPreview;
        private OverlapSphere greenOverlap;
        private BrushDisplay brushDisplay;
        private BrushManager brushManager;
        private ClonePreview clonePreview;
        private PopupManager popupManager;
        private OverlapSphere redOverlap;
        private FileScreen fileScreen;
        private WikiScreen wikiScreen;
        private WorldHistory history;
        private Transform eyes;
        private Player player;
        private World world;
        private Menu menu;


        private void Awake()
        {
            world = FindAnyObjectByType<World>();
            history = FindAnyObjectByType<WorldHistory>();
            keyboardShortcuts = FindAnyObjectByType<KeyboardShortcuts>();
            fileScreen = FindAnyObjectByType<FileScreen>();
            wikiScreen = FindAnyObjectByType<WikiScreen>();
            chunkVisualManager = FindAnyObjectByType<ChunkVisualManager>();
            player = FindAnyObjectByType<Player>();
            playerDisplay = FindAnyObjectByType<PlayerDisplay>();
            menu = FindAnyObjectByType<Menu>();
            hoverHighlight = FindAnyObjectByType<HoverHighlight>();
            brushDisplay = FindAnyObjectByType<BrushDisplay>();
            brushManager = FindAnyObjectByType<BrushManager>();
            overlayManager = FindAnyObjectByType<OverlayManager>();
            clonePreview = FindAnyObjectByType<ClonePreview>();
            popupManager = FindAnyObjectByType<PopupManager>();
            stairsPreview = FindAnyObjectByType<StairsPreview>();

            eyes = GameObject.FindWithTag("MainCamera").transform;
            OverlapSphere[] overlapSpheres = eyes.GetComponents<OverlapSphere>();
            greenOverlap = overlapSpheres[0];
            redOverlap = overlapSpheres[1];
        }
        
        private void Start()
        {
            popupManager.Setup();
            playerDisplay.Setup();
            brushDisplay.Setup(brushes);

            keyboardShortcuts.OnSave += fileScreen.Save;
            keyboardShortcuts.OnExport += fileScreen.Export;
            keyboardShortcuts.OnUndo += history.Undo;
            keyboardShortcuts.OnRedo += history.Redo;

            greenOverlap.OnChange += overlayManager.EnableGreen;
            redOverlap.OnChange += overlayManager.EnableRed;

            world.OnClear += history.Clear;
            world.OnNewChanges += history.StoreWorldChange;

            chunkVisualManager.Setup(eyes);
            world.OnDrawChunk += chunkVisualManager.DrawChunk;
            history.Setup(world);

            player.OnOpen += playerDisplay.Show;
            player.OnClose += playerDisplay.Hide;
            hoverHighlight.OnHover += playerDisplay.SetCenterText;

            stairsPreview.Setup(player.transform);
            stairsPreview.OnNewStairsMesh += brushManager.ReloadPreviewMesh;

            Clone clone = new Clone(world);
            clonePreview.SetVisibilityWithBrushIndex(-1);
            clone.OnNewExample += clonePreview.Show;
            hoverHighlight.OnHover += clonePreview.UpdatePreview;

            IBrushable[] brushables = new IBrushable[] 
            {
                new Fill(world),
                new Walls(world),
                new Cylinder(world),
                new Stairs(world, eyes),
                new Noise(world),
                clone
            };

            brushManager.Setup(brushes, brushables);
            wikiScreen.Setup(brushes);
            clonePreview.Setup(brushes.Length - 1);

            brushManager.OnNewBrushSelected += brushDisplay.NewIndexSelected;
            brushManager.OnNewBrushSelected += clonePreview.SetVisibilityWithBrushIndex;

            world.Setup();
            world.Flush();

            player.Setup(world);
            brushManager.OnNewPreviewMesh += player.GetAddSelectionPreview().UpdateMesh;
            brushManager.OnNewPreviewMesh += player.GetRemoveSelectionPreview().UpdateMesh;

            menu.Setup(new Level(new List<IBinarySerializable> { world }));

            fsm.AddState(player);
            fsm.AddState(menu);
            fsm.AddTransition(new Transition(player, menu));
            fsm.AddTransition(new Transition(menu, player));

            fsm.Open(menu);
        }

        private void Update() => fsm.Update();
        private void FixedUpdate() => fsm.FixedUpdate();
        private void OnApplicationQuit() => fsm?.Close();
    }
}