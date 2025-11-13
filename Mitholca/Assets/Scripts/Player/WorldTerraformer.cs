using UnityEngine;
using UnityEngine.InputSystem;

namespace Mitholca
{
    public class WorldTerraformer : MonoBehaviour
    {
        [SerializeField] private Transform eyes = default;
        [SerializeField] private GameObject cubePrefab = default;
        [SerializeField] private Raycast placeRaycat = default;
        [SerializeField] private Raycast destroyRaycast = default;

        private IMessageService message;

        private void Start()
        {
            message = ServiceLocator<IMessageService>.Locate();
        }

        private void Update()
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
                message.Send("Test", Color.green);
            

            // PLACE.
            if (Input.GetKeyDown(KeyCode.Mouse1) && placeRaycat.Cast(eyes, out RaycastHit hit))
                Instantiate(cubePrefab, Utils.ApplyGrid(hit.point + (hit.normal * 0.5f)), Quaternion.identity);

            // DESTROY.
            if (Input.GetKeyDown(KeyCode.Mouse0) && destroyRaycast.Cast(eyes, out hit))
                Destroy(hit.transform.gameObject);

        }

        public void Setup(Transform eyes) => this.eyes = eyes;
    }
}
