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
        [SerializeField] private RaycastSettings raycast = default;

        private RaycastHit hit;
        private Vector3Int? firstPos;
        private Vector3Int? secondPos;
        private World world;

        private bool ButtonPressed => leftMouseButton ? Mouse.current.leftButton.wasPressedThisFrame : Mouse.current.rightButton.wasPressedThisFrame;
        private bool ButtonReleased => leftMouseButton ? Mouse.current.leftButton.wasReleasedThisFrame : Mouse.current.rightButton.wasReleasedThisFrame;

        private void Start()
        {
            world = FindAnyObjectByType<World>();
            ResetToDefault();
        }

        public override void OnTick()
        {
            base.OnTick();
            if(firstPos != null && raycast.Cast(eyes, out hit))
                secondPos = Utils.ConvertToBlockPos(hit.point);

            OnShowPreview?.Invoke(firstPos, secondPos);
        }

        public override void OnFrame()
        {
            base.OnFrame();
            if (ButtonPressed)
            {
                ResetToDefault();
                if(raycast.Cast(eyes, out hit))
                    firstPos = Utils.ConvertToBlockPos(hit.point);
            }

            if (firstPos != null && ButtonReleased)
            {
                if (raycast.Cast(eyes, out hit))
                {
                    secondPos = Utils.ConvertToBlockPos(hit.point);
                    OnEditSelection?.Invoke((Vector3Int)firstPos, (Vector3Int)secondPos);
                    world.Flush();
                }

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
