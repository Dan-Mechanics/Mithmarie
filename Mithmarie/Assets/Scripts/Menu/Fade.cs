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

        public void SetDirection(bool becomeVisible) => direction = becomeVisible ? 1f : -1f;

        public void Flash()
        {
            group.alpha = 1f;
            SetDirection(false);
        }
    }
}
