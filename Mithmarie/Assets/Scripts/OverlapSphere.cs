using System;
using UnityEngine;
using UnityEngine.Events;

namespace Mithmarie
{
    public class OverlapSphere : MonoBehaviour
    {
        public event Action<bool> OnChange;

        [SerializeField, Min(0f)] private float radius = default;
        [SerializeField] private LayerMask mask = default;
        [SerializeField] private QueryTriggerInteraction interaction = default;

        private void FixedUpdate()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, mask, interaction);
            if (colliders.Length > 0) 
            {
                OnChange?.Invoke(true);
            }
            else
            {
                OnChange?.Invoke(false);
            }
        }
    }
}
