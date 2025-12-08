using UnityEngine;
using UnityEngine.UI;

namespace Mithmarie
{
    public class BrushDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab = default;
        [SerializeField] private float perSlotOffset = default;
        private Slot[] slots;

        public void Setup(int count, int index)
        {
            slots = new Slot[count];
            for (int i = 0; i < count; i++)
            {
                RectTransform newSlot = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity).GetComponent<RectTransform>();
                newSlot.SetParent(transform);
                newSlot.localPosition = Vector3.zero;
                newSlot.localRotation = Quaternion.identity;

                newSlot.anchoredPosition += i * perSlotOffset * Vector2.up;
                slots[i].image = newSlot.GetComponent<Image>();
                slots[i].arrow = newSlot.GetChild(0).gameObject;
            }

            ShowIndex(index);
        }

        public void ShowIndex(int index)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].arrow.SetActive(i == index);
                slots[i].image.color = i == index ? Color.white : Color.gray;
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
