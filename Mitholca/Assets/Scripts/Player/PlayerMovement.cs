using UnityEngine;

namespace Mitholca
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float speed = default;
        [SerializeField, Min(1f)] private float sprintSpeedMult = default;

        private Vector3 movement;

        private void Update()
        {
            movement.y = (Input.GetKey(KeyCode.Space) ? 1f : 0f) + (Input.GetKey(KeyCode.LeftShift) ? -1f : 0f);

            movement = Vector3.zero;
            movement += transform.forward * Input.GetAxisRaw("Vertical");
            movement += transform.right * Input.GetAxisRaw("Horizontal");
            movement.Normalize();

            if (Input.GetKey(KeyCode.Space))
                movement += Vector3.up;

            if (Input.GetKey(KeyCode.LeftShift))
                movement += Vector3.down;

            movement *= speed;

            if (Input.GetKey(KeyCode.LeftControl))
                movement *= sprintSpeedMult;

            transform.Translate(movement * Time.deltaTime, Space.World);
        }
    }
}
