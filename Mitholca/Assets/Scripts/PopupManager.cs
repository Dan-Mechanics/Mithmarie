using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mitholca
{
    public class PopupManager : MonoBehaviour, IMessageService
    {
        [SerializeField] private CanvasGroup group = default;
        [SerializeField] private TMP_Text text = default;
        [SerializeField] private Image image = default;
        [SerializeField, Range(0.1f, 0.9f)] private float backdropVisibility = default;
        [SerializeField, Min(0.001f)] private float decayRate = default;

        private void Awake()
        {
            group.alpha = 0f;
            text.text = string.Empty;
            ServiceLocator<IMessageService>.Provide(this);
        }

        private void FixedUpdate()
        {
            group.alpha -= decayRate * Time.fixedDeltaTime;
            group.alpha = Mathf.Clamp01(group.alpha);
        }

        public void Send(string text, Color color)
        {
            group.alpha = 1f;
            this.text.text = text;

            Color clear = color;
            color.a = 1f;
            clear.a = 0f;
            image.color = Color.Lerp(color, clear, backdropVisibility);

            print(text);
        }
    }
}
