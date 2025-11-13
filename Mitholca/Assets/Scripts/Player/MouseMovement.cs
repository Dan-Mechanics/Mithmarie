using UnityEngine;

namespace Mitholca
{
    public class MouseMovement : MonoBehaviour
    {
        private const float MAX_CAM_ANGLE = 90f;
        [SerializeField, Min(0f)] private float sensitivity = default;

        private Transform eyes;
        private Vector2 mouseInput;
        private Vector2 rotation;

        private void Update()
        {
            mouseInput.y = Input.GetAxisRaw("Mouse X");
            mouseInput.x = -Input.GetAxisRaw("Mouse Y");

            rotation += (Cursor.visible ? 0f : 1f) * sensitivity * mouseInput;
            rotation.x = Mathf.Clamp(rotation.x, -MAX_CAM_ANGLE, MAX_CAM_ANGLE);

            eyes.localRotation = Quaternion.AngleAxis(rotation.x, Vector3.right);
            transform.rotation = Quaternion.AngleAxis(rotation.y, Vector3.up);
        }

        public void Setup(Transform eyes) => this.eyes = eyes;
    }
}