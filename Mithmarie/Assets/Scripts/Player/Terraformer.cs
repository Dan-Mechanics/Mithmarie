using DanUtils;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class Terraformer : StateBehaviour
    {
        public event Action<Selection> OnPreview;
        public event Action<Vector3Int, Vector3Int> OnEditSelection;

        [SerializeField] private Transform eyes = default;
        [SerializeField] private TerraformRaycast raycast = default;
        [SerializeField] private string buttonName = default;

        private readonly Selection selection = new Selection();
        private InputAction button;
        private Vector3Int? firstPos;
        private Vector3Int secondPos;
        private RaycastHit hit;
        private World world;

        public void Setup(World world)
        {
            this.world = world;
            button = InputSystem.actions.FindAction(buttonName);
            ResetToDefault();
        }

        private void ResetToDefault() => firstPos = null;

        public override void OnFrame()
        {
            base.OnFrame();
            if (button.WasPressedThisFrame())
                Press();

            if(button.WasReleasedThisFrame())
                Release();
        }

        public override void OnTick()
        {
            base.OnTick();
            if (firstPos != null)
            {
                raycast.Cast(eyes, out hit);
                secondPos = Utils.GetBlockPos(hit.point);
            }

            if(firstPos == null)
            {
                OnPreview?.Invoke(null);
                return;
            }

            selection.a = (Vector3Int)firstPos;
            selection.b = secondPos;
            OnPreview?.Invoke(selection);
        }

        public void Press()
        {
            ResetToDefault();
            raycast.Cast(eyes, out hit);
            firstPos = Utils.GetBlockPos(hit.point);
        }

        public void Release()
        {
            if (firstPos == null)
                return;
            
            raycast.Cast(eyes, out hit);
            secondPos = Utils.GetBlockPos(hit.point);

            world.RememberTheFollowing();
            OnEditSelection?.Invoke((Vector3Int)firstPos, secondPos);
            world.Flush();

            ResetToDefault();
        }
    }
}
