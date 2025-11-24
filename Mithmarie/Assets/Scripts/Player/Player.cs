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
        [SerializeField] private AimedBlockOutline blockHighlight = default;

        private StateBehaviour[] behaviour;
        private PlayerHUD playerHUD;
        private bool wantsToClose;
        private World world;

        private void Awake()
        {
            world = FindAnyObjectByType<World>();
            playerHUD = FindAnyObjectByType<PlayerHUD>();

            List<StateBehaviour> list = GetComponents<StateBehaviour>().ToList();
            list.RemoveAt(list.FindIndex(x => x is Player));
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

            blockHighlight.OnAim += playerHUD.SetCenterText;

            addTerraform.OnEditSelection += world.AddSelection;
            removeTerraform.OnEditSelection += world.RemoveSelection;
        }

        public override void Exit()
        {
            base.Exit();
            wantsToClose = false;

            for (int i = 0; i < behaviour.Length; i++)
            {
                behaviour[i].Exit();
            }

            playerHUD.Hide();
            addTerraform.OnShowPreview -= addSelectionPreview.UpdatePreview;
            removeTerraform.OnShowPreview -= removeSelectionPreview.UpdatePreview;

            blockHighlight.OnAim -= playerHUD.SetCenterText;

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

            if (Keyboard.current[GameManager.TOGGLE_STATE_KEY].wasPressedThisFrame)
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

        public bool GetWantsToClose() => wantsToClose;
        private void Close() => wantsToClose = true;
    }
}