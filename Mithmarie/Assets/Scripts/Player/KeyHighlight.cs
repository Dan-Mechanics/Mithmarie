using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Mithmarie
{
    public class KeyHighlight : MonoBehaviour
    {
        [SerializeField] private Key key = default;
        [SerializeField] private Image image = default;
        [SerializeField] private Image icon = default;
        [SerializeField] private TMP_Text text = default;

        public void Draw(Color colorA, Color colorB)
        {
            image.color = Keyboard.current[key].isPressed ? colorB : colorA;
            text.color = icon.color = Keyboard.current[key].isPressed ? colorA : colorB;
        }
    }
}
