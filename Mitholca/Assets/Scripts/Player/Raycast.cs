using System;
using UnityEngine;

namespace Mitholca
{
    [Serializable]
    public struct Raycast
    {
        [Min(0f)] public float range;
        public LayerMask mask;
        public QueryTriggerInteraction interaction;

        public bool Cast(Transform arrow, out RaycastHit hit) => Physics.Raycast(arrow.position, arrow.forward, out hit, range, mask, interaction);
    }
}
