using System;
using UnityEngine;

namespace Mithmarie
{
    [Serializable]
    public class RaycastSettings
    {
        [Min(0.1f)] public float range = 10f;
        public LayerMask mask = 1;
        public float normalOffset = 0f;
        public QueryTriggerInteraction interaction = QueryTriggerInteraction.Ignore;

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
