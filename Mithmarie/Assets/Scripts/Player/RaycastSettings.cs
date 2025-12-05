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

        public bool Cast(Transform arrow, out RaycastHit hit)
        {
            if(Physics.Raycast(arrow.position, arrow.forward, out hit, maxRange, mask, interaction))
            {
                hit.point += hit.normal * normalOffset;
                return true;
            }

            return false;
        }
    }
}
