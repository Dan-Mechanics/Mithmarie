using UnityEngine;

namespace Mithmarie
{
    public class FieldOfView : StateBehaviour
    {
        [SerializeField] private Camera cam = default;
        [SerializeField] private PersistentFloat fov = default;

        public override void Enter()
        {
            base.Enter();
            fov.Load();
            cam.fieldOfView = fov.Value;
        }
    }
}
