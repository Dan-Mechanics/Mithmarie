using UnityEngine;

namespace Mitholca
{
    public class WorldTerraformer : MonoBehaviour
    {
        [SerializeField] private GameObject cubePrefab = default;
        [SerializeField] private Raycast placeRaycat = default;
        [SerializeField] private Raycast destroyRaycast = default;

        private Transform eyes;

        private void Update()
        {
            // PLACE.
            if (Input.GetKeyDown(KeyCode.Mouse1) && placeRaycat.Get(eyes, out RaycastHit hit))
                Instantiate(cubePrefab, Utils.ApplyGrid(hit.point + (hit.normal * 0.5f)), Quaternion.identity);

            // DESTROY.
            if (Input.GetKeyDown(KeyCode.Mouse0) && destroyRaycast.Get(eyes, out hit))
                Destroy(hit.transform.gameObject);

        }

        public void Setup(Transform eyes) => this.eyes = eyes;
    }
}
