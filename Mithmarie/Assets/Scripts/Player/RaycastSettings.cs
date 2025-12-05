using System;
using UnityEngine;

namespace Mithmarie
{
    [Serializable]
    public struct RaycastSettings
    {
        [Min(0.1f)] public float maxRange;
        public LayerMask mask;
        public float normalOffset;
        public float airPlacementDistance;
        public QueryTriggerInteraction interaction;

        public bool Cast(Transform eyes, out RaycastHit hit)
        {
            if(Physics.Raycast(eyes.position, eyes.forward, out hit, maxRange, mask, interaction))
            {
                hit.point += hit.normal * normalOffset;
                return true;
            }

            hit.point = eyes.position + (eyes.forward * airPlacementDistance);
            hit.normal = -eyes.forward;
            return true;
        }
    }
}
