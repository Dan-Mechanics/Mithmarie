using TMPro;
using UnityEngine;

namespace Mithmarie
{
    public class PlayerDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text centerText = default;
        [SerializeField] private Color colorA = Color.white;
        [SerializeField] private Color colorB = Color.white;

        private KeyHighlight[] keyHighlights;

        public void Setup()
        {
            keyHighlights = GetComponentsInChildren<KeyHighlight>();
        }

        private void Update()
        {
            for (int i = 0; i < keyHighlights.Length; i++)
            {
                keyHighlights[i].Draw(colorA, colorB);
            }
        }

        public void SetCenterText(HoverInformation hover) 
        {
            if(hover == null)
            {
                centerText.text = string.Empty;
                return;
            }

            centerText.text = hover.pos.ToString();
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}
