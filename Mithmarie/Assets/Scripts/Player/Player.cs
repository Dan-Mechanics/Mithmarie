using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class Player : StateBehaviour
    {
        public SelectionPreview AddSelectionPreview => addSelectionPreview;
        public SelectionPreview RemoveSelectionPreview => removeSelectionPreview;
        
        public event Action OnOpen;
        public event Action OnClose;

        [SerializeField] private InputActionAsset inputActions = default;
        [SerializeField] private PlayerMovement playerMovement = default;
        [SerializeField] private Terraformer addTerraform = default;
        [SerializeField] private Terraformer removeTerraform = default;
        [SerializeField] private SelectionPreview addSelectionPreview = default;
        [SerializeField] private SelectionPreview removeSelectionPreview = default;
        [SerializeField] private BrushManager brushManager = default;
        [SerializeField] private HoverHighlight hoverHighlight = default;
        [SerializeField] private HoverPreview hoverPreview = default;
        [SerializeField] private string pauseName = default;

        private InputAction pauseAction;
        private StateBehaviour[] behaviours;

        public void Setup(World world)
        {
            List<StateBehaviour> list = GetComponentsInChildren<StateBehaviour>().ToList();
            list.Remove(this);
            behaviours = list.ToArray();

            pauseAction = InputSystem.actions.FindAction(pauseName);

            addSelectionPreview.Setup();
            removeSelectionPreview.Setup();

            addTerraform.Setup(world);
            removeTerraform.Setup(world);

            hoverPreview.Setup();
            playerMovement.Setup();
        }

        public override void Enter()
        {
            base.Enter();
            for (int i = 0; i < behaviours.Length; i++)
            {
                behaviours[i].Enter();
            }

            addTerraform.OnPreview += addSelectionPreview.UpdatePreview;
            removeTerraform.OnPreview += removeSelectionPreview.UpdatePreview;
            hoverHighlight.OnHover += hoverPreview.UpdatePreview;

            addTerraform.OnEditSelection += brushManager.AddSelection;
            removeTerraform.OnEditSelection += brushManager.RemoveSelection;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            OnOpen?.Invoke();
        }

        public override void Exit()
        {
            base.Exit();
            for (int i = 0; i < behaviours.Length; i++)
            {
                behaviours[i].Exit();
            }

            addTerraform.OnPreview -= addSelectionPreview.UpdatePreview;
            removeTerraform.OnPreview -= removeSelectionPreview.UpdatePreview;

            addTerraform.OnEditSelection -= brushManager.AddSelection;
            removeTerraform.OnEditSelection -= brushManager.RemoveSelection;

            hoverHighlight.OnHover -= hoverPreview.UpdatePreview;
            OnClose?.Invoke();
        }

        public override void OnFrame()
        {
            base.OnFrame();

            for (int i = 0; i < behaviours.Length; i++)
            {
                behaviours[i].OnFrame();
            }

            if (pauseAction.WasPressedThisFrame())
                Close();
        }

        public override void OnTick()
        {
            base.OnTick();
            for (int i = 0; i < behaviours.Length; i++)
            {
                behaviours[i].OnTick();
            }
        }

        private void OnEnable()
        {
            inputActions.FindActionMap("Player").Enable();
        }

        private void OnDisable()
        {
            inputActions.FindActionMap("Player").Disable();
        }
    }
}