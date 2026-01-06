using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Mithmarie
{
    public class KeyHighlight : MonoBehaviour
    {
        [SerializeField] private Image image = default;
        [SerializeField] private Image icon = default;
        [SerializeField] private TMP_Text text = default;
        [SerializeField] private string key = default;

        private InputAction action;

        public void Setup()
        {
            action = InputSystem.actions.FindAction(key);
            text.text = gameObject.name.ToUpperInvariant();
        }

        public void Draw(Color colorA, Color colorB)
        {
            bool pressed = action.IsPressed();
            image.color = pressed ? colorB : colorA;
            text.color = icon.color = pressed ? colorA : colorB;
        }
    }
}
