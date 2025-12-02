using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class Terraformer : StateBehaviour, IRaycastProvider
    {
        public event Action<Vector3Int?, Vector3Int?> OnShowPreview;
        public event Action<Vector3Int, Vector3Int> OnEditSelection;
        
        [SerializeField] private Transform eyes = default;
        [SerializeField] private bool leftMouseButton = default;
        [SerializeField] private PersistentBool swapMouseButtons;
        [SerializeField] private RaycastSettings raycast = default;

        private RaycastHit hit;
        private Vector3Int? firstPos;
        private Vector3Int? secondPos;
        private World world;

        public void Setup(World world)
        {
            this.world = world;
            ResetToDefault();
        }

        private bool ButtonPressed()
        {
            if (!swapMouseButtons.value)
                return leftMouseButton ? Mouse.current.leftButton.wasPressedThisFrame : Mouse.current.rightButton.wasPressedThisFrame;

            return !leftMouseButton ? Mouse.current.leftButton.wasPressedThisFrame : Mouse.current.rightButton.wasPressedThisFrame;
        }

        private bool ButtonReleased()
        {
            if (!swapMouseButtons.value)
                return leftMouseButton ? Mouse.current.leftButton.wasReleasedThisFrame : Mouse.current.rightButton.wasReleasedThisFrame;

            return !leftMouseButton ? Mouse.current.leftButton.wasReleasedThisFrame : Mouse.current.rightButton.wasReleasedThisFrame;
        }

        public override void Enter()
        {
            base.Enter();
            swapMouseButtons.Load();
        }

        public override void OnTick()
        {
            base.OnTick();
            if (firstPos != null && raycast.Cast(eyes, out hit))
            {
                secondPos = Utils.GetBlockPos(hit.point);
            }
            else
            {
                secondPos = Utils.GetBlockPos(eyes.position + (eyes.forward * raycast.airPlacementDistance));
            }

            OnShowPreview?.Invoke(firstPos, secondPos);
        }

        public override void OnFrame()
        {
            base.OnFrame();
            if (ButtonPressed())
            {
                ResetToDefault();
                if (raycast.Cast(eyes, out hit))
                {
                    firstPos = Utils.GetBlockPos(hit.point);
                }
                else
                {
                    firstPos = Utils.GetBlockPos(eyes.position + (eyes.forward * raycast.airPlacementDistance));
                }
            }

            if (firstPos != null && ButtonReleased())
            {
                if (raycast.Cast(eyes, out hit))
                {
                    secondPos = Utils.GetBlockPos(hit.point);
                }
                else
                {
                    secondPos = Utils.GetBlockPos(eyes.position + (eyes.forward * raycast.airPlacementDistance));
                }

                OnEditSelection?.Invoke((Vector3Int)firstPos, (Vector3Int)secondPos);
                world.Flush();
                ResetToDefault();
            }
        }

        private void ResetToDefault()
        {
            firstPos = null;
            secondPos = null;
        }

        public RaycastSettings GetSettings() => raycast;
    }
}
