using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class Terraformer : StateBehaviour
    {
        public event Action<Raycast> OnBeginEditing;
        public event Action<Vector3Int?, Vector3Int?> OnShowPreview;
        public event Action<Vector3Int, Vector3Int> OnEditSelection;
        
        [SerializeField] private Transform eyes = default;
        [SerializeField] private Raycast raycast = default;
        [SerializeField] private float hitPointExtrusion = default;
        [SerializeField] private bool leftMouseButton = default;

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
                secondPos = Utils.ApplyGrid(hit.point + (hit.normal * hitPointExtrusion));

            OnShowPreview?.Invoke(firstPos, secondPos);
        }

        public override void OnFrame()
        {
            base.OnFrame();
            if (ButtonPressed)
            {
                OnBeginEditing?.Invoke(raycast);
                ResetToDefault();
                if(raycast.Cast(eyes, out hit))
                    firstPos = Utils.ApplyGrid(hit.point + (hit.normal * hitPointExtrusion));
            }

            if (firstPos != null && ButtonReleased)
            {
                if (raycast.Cast(eyes, out hit))
                {
                    secondPos = Utils.ApplyGrid(hit.point + (hit.normal * hitPointExtrusion));
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
    }
}
