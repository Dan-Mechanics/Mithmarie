using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class Player : StateBehaviour
    {   
        [SerializeField] private Terraformer addTerraform = default;
        [SerializeField] private Terraformer removeTerraform = default;
        [SerializeField] private SelectionPreview addSelectionPreview = default;
        [SerializeField] private SelectionPreview removeSelectionPreview = default;
        [SerializeField] private AimedBlockHighlight blockHighlight = default;

        private StateBehaviour[] playerBehaviour;
        private PlayerHUD playerHUD;
        private bool wantsToClose;
        private World world;

        private void Awake()
        {
            world = FindAnyObjectByType<World>();
            playerHUD = FindAnyObjectByType<PlayerHUD>();

            List<StateBehaviour> list = GetComponents<StateBehaviour>().ToList();
            list.RemoveAt(list.FindIndex(x => x is Player));
            playerBehaviour = list.ToArray();

            blockHighlight.Configure(addTerraform.Raycast, addTerraform.NormalDirection);
        }
         
        public override void Enter()
        {
            base.Enter();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            playerHUD.Show();
            addTerraform.OnInput += () => { blockHighlight.Configure(addTerraform.Raycast, addTerraform.NormalDirection); };
            removeTerraform.OnInput += () => { blockHighlight.Configure(removeTerraform.Raycast, removeTerraform.NormalDirection); };

            addTerraform.OnShowPreview += addSelectionPreview.UpdatePreview;
            removeTerraform.OnShowPreview += removeSelectionPreview.UpdatePreview;

            blockHighlight.OnOutputText += playerHUD.SetCenterText;

            addTerraform.OnEditSelection += world.AddSelection;
            removeTerraform.OnEditSelection += world.RemoveSelection;
        }

        public override void Exit()
        {
            base.Exit();
            wantsToClose = false;

            // APPLICATION.QUIT --> DESTROY --> EXIT --> PLAYERHUD DOESNT EXIST ANYMORE,
            // THIS FIXES IT.
            if (playerHUD != null)
                playerHUD.Hide();

            addTerraform.OnInput -= () => { blockHighlight.Configure(addTerraform.Raycast, addTerraform.NormalDirection); };
            removeTerraform.OnInput -= () => { blockHighlight.Configure(removeTerraform.Raycast, removeTerraform.NormalDirection); };

            addTerraform.OnShowPreview -= addSelectionPreview.UpdatePreview;
            removeTerraform.OnShowPreview -= removeSelectionPreview.UpdatePreview;

            blockHighlight.OnOutputText -= playerHUD.SetCenterText;

            addTerraform.OnEditSelection -= world.AddSelection;
            removeTerraform.OnEditSelection -= world.RemoveSelection;
        }

        public override void OnFrame()
        {
            base.OnFrame();

            for (int i = 0; i < playerBehaviour.Length; i++)
            {
                playerBehaviour[i].OnFrame();
            }

            if (Keyboard.current[GameManager.TOGGLE_STATE_KEY].wasPressedThisFrame)
                Close();
        }

        public override void OnTick()
        {
            base.OnTick();
            for (int i = 0; i < playerBehaviour.Length; i++)
            {
                playerBehaviour[i].OnTick();
            }
        }

        public bool GetWantsToClose() => wantsToClose;
        private void Close() => wantsToClose = true;
    }
}