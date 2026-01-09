using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mithmarie
{
    public class BrushDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab = default;
        [SerializeField] private TMP_Text brushNameText = default;
        [SerializeField] private TMP_Text tooltipText = default;
        [SerializeField] private Fade tooltipFade = default;
        [SerializeField] private float perSlotOffset = default;
        [SerializeField] private Vector2 globalOffset = default;
        [SerializeField] private Color colorA = Color.white;
        [SerializeField] private Color colorB = Color.white;

        private Brush[] brushes;
        private Slot[] slots;

        public void Setup(Brush[] brushes)
        {
            this.brushes = brushes;
            slots = new Slot[brushes.Length];
            for (int i = 0; i < slots.Length; i++)
            {
                RectTransform newSlot = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
                newSlot.SetParent(transform);
                newSlot.localPosition = Vector3.zero;
                newSlot.localRotation = Quaternion.identity;

                newSlot.anchoredPosition += i * perSlotOffset * Vector2.up;
                newSlot.anchoredPosition += globalOffset;
                slots[i].image = newSlot.GetComponent<Image>();
                slots[i].arrow = newSlot.GetChild(0).gameObject;

                slots[i].image.sprite = brushes[i].icon;
                slots[i].image.transform.name = brushes[i].name;
            }

            NewIndexSelected(0);
        }

        public void NewIndexSelected(int index)
        {
            brushNameText.text = brushes[index].name;
            tooltipText.text = brushes[index].tooltip;
            tooltipFade.Flash();

            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].arrow.SetActive(i == index);
                slots[i].image.color = i == index ? colorA : colorB;
            }
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        private struct Slot
        {
            public Image image;
            public GameObject arrow;
        }
    }
}
