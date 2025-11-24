using TMPro;
using UnityEngine;

namespace Mithmarie
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private TMP_Text centerText = default;
        [SerializeField] private Color colorA = Color.white;
        [SerializeField] private Color colorB = Color.white;

        private KeyHighlight[] keyHighlights;

        private void Awake()
        {
            keyHighlights = transform.GetComponentsInChildren<KeyHighlight>();
        }

        private void Update()
        {
            for (int i = 0; i < keyHighlights.Length; i++)
            {
                keyHighlights[i].Draw(colorA, colorB); ;
            }
        }

        public void SetCenterText(object obj) 
        {
            centerText.text = string.Empty;
            if (obj == null)
                return;

            centerText.text = obj as string;
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}
