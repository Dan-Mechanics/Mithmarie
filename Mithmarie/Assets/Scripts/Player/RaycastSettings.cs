using System;
using UnityEngine;

namespace Mithmarie
{
    [Serializable]
    public struct RaycastSettings
    {
        [Min(0.1f)] public float range;
        public LayerMask mask;
        public float normalOffset;
        public QueryTriggerInteraction interaction;

        public bool Cast(Transform arrow, out RaycastHit hit)
        {
            if(Physics.Raycast(arrow.position, arrow.forward, out hit, range, mask, interaction))
            {
                hit.point += hit.normal * normalOffset;
                return true;
            }

            return false;
        }
    }
}
