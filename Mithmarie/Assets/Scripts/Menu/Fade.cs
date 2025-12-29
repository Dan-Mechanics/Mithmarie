using UnityEngine;

namespace Mithmarie
{
    public class Fade : MonoBehaviour
    {
        [SerializeField] private CanvasGroup group = default;
        [SerializeField, Min(0f)] private float speed = default;
        private float direction;

        private void FixedUpdate()
        {
            float alpha = group.alpha;
            alpha += speed * Time.fixedDeltaTime * direction;
            alpha = Mathf.Clamp01(alpha);
            group.alpha = alpha;
        }

        public void Enable(bool active) => direction = active ? 1f : -1f;
    }
}
