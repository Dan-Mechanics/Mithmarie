using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class Terraformer : StateBehaviour
    {
        public event Action<SelectionInformation> OnSelectionPreview;
        public event Action<Vector3Int, Vector3Int> OnEdit;

        [SerializeField] private Transform eyes = default;
        [SerializeField] private bool leftMouseButton = default;
        [SerializeField] private PersistentBool swapMouseButtons;
        [SerializeField] private TerraformRaycast raycast = default;

        private readonly SelectionInformation selection = new SelectionInformation();
        private Vector3Int? firstPos;
        private Vector3Int secondPos;
        private RaycastHit hit;
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
            if (firstPos != null)
            {
                raycast.Cast(eyes, out hit);
                secondPos = Utils.GetBlockPos(hit.point);
            }

            if(firstPos == null)
            {
                OnSelectionPreview?.Invoke(null);
            }
            else
            {
                selection.a = (Vector3Int)firstPos;
                selection.b = secondPos;
                OnSelectionPreview?.Invoke(selection);
            }
        }

        public override void OnFrame()
        {
            base.OnFrame();
            if (ButtonPressed())
            {
                ResetToDefault();
                raycast.Cast(eyes, out hit);
                firstPos = Utils.GetBlockPos(hit.point);
            }

            if (firstPos != null && ButtonReleased())
            {
                raycast.Cast(eyes, out hit);
                secondPos = Utils.GetBlockPos(hit.point);

                world.RememberTheFollowing();
                OnEdit?.Invoke((Vector3Int)firstPos, secondPos);
                world.Flush();

                ResetToDefault();
            }
        }

        private void ResetToDefault() => firstPos = null;
    }
}
