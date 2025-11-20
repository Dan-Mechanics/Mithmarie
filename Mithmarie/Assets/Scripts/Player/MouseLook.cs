using UnityEngine;

namespace Mithmarie
{
    public class MouseLook : StateBehaviour, ISettingsRequired
    {
        private const float MAX_CAM_ANGLE = 90f;
        [SerializeField] private Transform eyes = default;
        [SerializeField] public Vector2 rotation = default;

        private Vector2 mouseInput;
        private PlayerSettings settings;

        public void AssignSettings(PlayerSettings settings) => this.settings = settings;

        /// <summary>
        /// We are using the old input system 
        /// here because it just works better.
        /// </summary>
        public override void OnFrame()
        {
            base.OnFrame();
            mouseInput.y = Input.GetAxisRaw("Mouse X");
            mouseInput.x = -Input.GetAxisRaw("Mouse Y");

            rotation += settings.sensitivity * mouseInput;
            rotation.x = Mathf.Clamp(rotation.x, -MAX_CAM_ANGLE, MAX_CAM_ANGLE);

            eyes.localRotation = Quaternion.AngleAxis(rotation.x, Vector3.right);
            transform.rotation = Quaternion.AngleAxis(rotation.y, Vector3.up);
        }
    }
}