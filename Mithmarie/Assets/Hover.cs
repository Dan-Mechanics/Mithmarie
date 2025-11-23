using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mithmarie
{
    public class Hover : MonoBehaviour, IPointerEnterHandler
    {
        public event Action OnHover;
        public void OnPointerEnter(PointerEventData eventData) => OnHover?.Invoke();
    }
}
