using System;
using UnityEngine;

namespace Mithmarie
{
    public class Terraformer : StateBehaviour
    {
        public event Action<SelectionInformation> OnPreview;
        public event Action<Vector3Int, Vector3Int> OnEditSelection;

        [SerializeField] private Transform eyes = default;
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

        private void ResetToDefault() => firstPos = null;

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
