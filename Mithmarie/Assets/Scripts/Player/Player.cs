using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class Player : StateBehaviour
    {
        public event Action OnOpen;
        public event Action OnClose;
        
        [SerializeField] private Terraformer addTerraform = default;
        [SerializeField] private Terraformer removeTerraform = default;
        [SerializeField] private SelectionPreview addSelectionPreview = default;
        [SerializeField] private SelectionPreview removeSelectionPreview = default;
        [SerializeField] private BrushManager brushManager = default;
        [SerializeField] private HoverHighlight hoverHighlight = default;
        [SerializeField] private HoverPreview hoverPreview = default;

        private StateBehaviour[] behaviour;

        public void Setup(World world)
        {
            List<StateBehaviour> list = GetComponentsInChildren<StateBehaviour>().ToList();
            list.Remove(this);
            behaviour = list.ToArray();

            addSelectionPreview.Setup();
            removeSelectionPreview.Setup();

            addTerraform.Setup(world);
            removeTerraform.Setup(world);

            hoverPreview.Setup();
        }
        
        public override void Enter()
        {
            base.Enter();
            for (int i = 0; i < behaviour.Length; i++)
            {
                behaviour[i].Enter();
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
            for (int i = 0; i < behaviour.Length; i++)
            {
                behaviour[i].Exit();
            }

            addTerraform.OnPreview -= addSelectionPreview.UpdatePreview;
            removeTerraform.OnPreview -= removeSelectionPreview.UpdatePreview;
            hoverHighlight.OnHover -= hoverPreview.UpdatePreview;

            addTerraform.OnEditSelection -= brushManager.AddSelection;
            removeTerraform.OnEditSelection -= brushManager.RemoveSelection;

            OnClose?.Invoke();
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