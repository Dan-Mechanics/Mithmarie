using UnityEngine;
using UnityEngine.InputSystem;

namespace Mitholca
{
    public class WorldEditor : StateBehaviour
    {
        [SerializeField] private Transform eyes = default;
        [SerializeField] private GameObject cubePrefab = default;
        [SerializeField] private Raycast buildRaycast = default;
        [SerializeField] private Raycast destroyRaycast = default;

        public override void OnFrame()
        {
            base.OnFrame();
            if (Mouse.current.rightButton.wasPressedThisFrame && buildRaycast.Cast(eyes, out RaycastHit hit))
                Instantiate(cubePrefab, Utils.ApplyGrid(hit.point + (hit.normal * 0.5f)), Quaternion.identity);

            if (Mouse.current.leftButton.wasPressedThisFrame && destroyRaycast.Cast(eyes, out hit))
                Destroy(hit.transform.gameObject);
        }
    }
}
