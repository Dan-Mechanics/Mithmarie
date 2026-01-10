using DanUtils;
using System;
using UnityEngine;

namespace Mithmarie
{
    public class HoverHighlight : StateBehaviour
    {
        public event Action<HoverInformation> OnHover;

        [SerializeField] private Transform eyes = default;
        [SerializeField] private TerraformRaycast raycast = default;

        private readonly HoverInformation hover = new HoverInformation();
        private RaycastHit hit;

        public override void OnTick()
        {
            base.OnFrame();

            hover.hasHit = raycast.Cast(eyes, out hit);
            hover.pos = Utils.GetBlockPos(hit.point);
            hover.normal = hit.normal;

            OnHover?.Invoke(hover);
        }
    }
}
