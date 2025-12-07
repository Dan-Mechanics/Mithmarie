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
        [SerializeField] private BrushManager brushManager = default;
        [SerializeField] private HoverHighlight hoverHighlight = default;
        [SerializeField] private HoverPreview hoverPreview = default;

        private StateBehaviour[] behaviour;
        private PlayerHUD playerHUD;
        private World world;

        public void Setup(World world, PlayerHUD playerHUD)
        {
            this.world = world;
            this.playerHUD = playerHUD;

            List<StateBehaviour> list = GetComponentsInChildren<StateBehaviour>().ToList();
            list.Remove(this);
            behaviour = list.ToArray();

            addSelectionPreview.Setup();
            removeSelectionPreview.Setup();

            addTerraform.Setup(world);
            removeTerraform.Setup(world);

            brushManager.Setup(world);

            hoverPreview.Setup();
        }
        
        public override void Enter()
        {
            base.Enter();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            for (int i = 0; i < behaviour.Length; i++)
            {
                behaviour[i].Enter();
            }

            playerHUD.Show();

            addTerraform.OnPreview += addSelectionPreview.UpdatePreview;
            removeTerraform.OnPreview += removeSelectionPreview.UpdatePreview;

            hoverHighlight.OnHover += hoverPreview.UpdatePreview;
            hoverHighlight.OnHover += playerHUD.SetCenterText;

            addTerraform.OnEditSelection += brushManager.AddSelection;
            removeTerraform.OnEditSelection += brushManager.RemoveSelection;
        }

        public override void Exit()
        {
            base.Exit();
            for (int i = 0; i < behaviour.Length; i++)
            {
                behaviour[i].Exit();
            }

            if (playerHUD != null)
                playerHUD.Hide();

            addTerraform.OnPreview -= addSelectionPreview.UpdatePreview;
            removeTerraform.OnPreview -= removeSelectionPreview.UpdatePreview;

            hoverHighlight.OnHover -= hoverPreview.UpdatePreview;
            hoverHighlight.OnHover -= playerHUD.SetCenterText;

            if (brushManager == null)
                return;

            if (addTerraform != null)
                addTerraform.OnEditSelection -= brushManager.AddSelection;

            if (removeTerraform != null)
                removeTerraform.OnEditSelection -= brushManager.RemoveSelection;
        }

        public override void OnFrame()
        {
            base.OnFrame();

            for (int i = 0; i < behaviour.Length; i++)
            {
                behaviour[i].OnFrame();
            }

            if (Keyboard.current.escapeKey.wasPressedThisFrame)
                Close();
        }

        public override void OnTick()
        {
            base.OnTick();
            for (int i = 0; i < behaviour.Length; i++)
            {
                behaviour[i].OnTick();
            }
        }
    }
}