using UnityEngine;
using UnityEngine.Events;

namespace Mithmarie
{
    public class OverlapSphere : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float radius = default;
        [SerializeField] private LayerMask mask = default;
        [SerializeField] private QueryTriggerInteraction interaction = default;

        // TODO: ACTION EVENTS AND LINK VIA GAMEMANAGER.
        [SerializeField] private UnityEvent onEnter = default;
        [SerializeField] private UnityEvent onExit = default;

        private void FixedUpdate()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, mask, interaction);
            if (colliders.Length > 0) 
            {
                onEnter?.Invoke();
            }
            else
            {
                onExit?.Invoke();
            }
        }
    }
}
