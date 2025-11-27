using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mithmarie
{
    public class PopupManager : MonoBehaviour, IMessageService
    {
        [SerializeField] private CanvasGroup group = default;
        [SerializeField] private TMP_Text text = default;
        [SerializeField] private Image image = default;
        [SerializeField, Range(0.1f, 0.9f)] private float backdropVisibility = default;
        [SerializeField, Min(0.1f)] private float standardDuration = default;
        private float duration;

        private void Awake()
        {
            duration = standardDuration;
            group.alpha = 0f;
            text.text = string.Empty;
            ServiceLocator<IMessageService>.Provide(this);
        }

        private void FixedUpdate()
        {
            group.alpha -= 1f / duration * Time.fixedDeltaTime;
            group.alpha = Mathf.Clamp01(group.alpha);
        }

        public void Send(string str, Color color, float duration = 0f)
        {
            if (!Utils.IsStringValid(str))
            {
                group.alpha = 0f;
                text.text = string.Empty;
                return;
            }

            this.duration = duration > 0 ? duration : standardDuration;

            print(str);
            group.alpha = 1f;
            text.text = str;

            Color clear = color;
            color.a = 1f;
            clear.a = 0f;
            image.color = Color.Lerp(color, clear, backdropVisibility);
        }
    }
}
