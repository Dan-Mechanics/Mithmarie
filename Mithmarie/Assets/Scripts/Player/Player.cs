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
        [SerializeField] private AimedFaceFeedback aimedFaceFeedback = default;

        private StateBehaviour[] behaviour;
        private PlayerHUD playerHUD;
        private World world;

        private void Awake()
        {
            world = FindAnyObjectByType<World>();
            playerHUD = FindAnyObjectByType<PlayerHUD>();

            List<StateBehaviour> list = GetComponentsInChildren<StateBehaviour>().ToList();
            list.Remove(this);
            behaviour = list.ToArray();
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

            addTerraform.OnShowPreview += addSelectionPreview.UpdatePreview;
            removeTerraform.OnShowPreview += removeSelectionPreview.UpdatePreview;

            aimedFaceFeedback.OnAim += playerHUD.SetCenterText;

            addTerraform.OnEditSelection += world.AddSelection;
            removeTerraform.OnEditSelection += world.RemoveSelection;
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

            addTerraform.OnShowPreview -= addSelectionPreview.UpdatePreview;
            removeTerraform.OnShowPreview -= removeSelectionPreview.UpdatePreview;

            aimedFaceFeedback.OnAim -= playerHUD.SetCenterText;

            addTerraform.OnEditSelection -= world.AddSelection;
            removeTerraform.OnEditSelection -= world.RemoveSelection;
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