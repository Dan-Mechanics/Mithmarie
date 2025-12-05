using UnityEngine;

namespace Mithmarie
{
    [CreateAssetMenu(menuName = "ScriptableObject/" + nameof(TerraformRaycast), fileName = "New " + nameof(TerraformRaycast))]
    public class TerraformRaycast : ScriptableObject
    {
        [Min(0f)] public float maxRange;
        public LayerMask mask;
        public float normalOffset;
        public float airPlacementDistance;

        public bool Cast(Transform eyes, out RaycastHit hit)
        {
            if (Physics.Raycast(eyes.position, eyes.forward, out hit, maxRange, mask, QueryTriggerInteraction.Ignore))
            {
                hit.point += hit.normal * normalOffset;
                return true;
            }

            hit.point = eyes.position + (eyes.forward * airPlacementDistance);
            hit.normal = -eyes.forward; // INCORRECT.
            return false;
        }
    }
}