using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace Mitholca
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private Highlight sprintHighlight = default;
        [SerializeField] private Highlight selectHighlight = default;
        [SerializeField] private Highlight menuHighlight = default;
        [SerializeField] private Color colorA = Color.white;
        [SerializeField] private Color colorB = Color.white;

        private readonly List<Highlight> highlights = new List<Highlight>();

        private void Start()
        {
            sprintHighlight.key = PlayerMovement.SPRINT_KEY;
            selectHighlight.key = WorldEditor.SELECTION_KEY;
            menuHighlight.key = GameManager.TOGGLE_KEY;

            highlights.Add(sprintHighlight);
            highlights.Add(selectHighlight);
            highlights.Add(menuHighlight);

            Draw();
        }

        private void Update() => Draw();

        private void Draw()
        {
            highlights.ForEach(x => x.Draw(colorA, colorB));
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        [Serializable]
        public class Highlight
        {
            public Image image;
            public TMP_Text text;
            [HideInInspector] public Key key;

            public void Draw(Color colorA, Color colorB)
            {
                image.color = Keyboard.current[key].isPressed ? colorB : colorA;
                text.color = Keyboard.current[key].isPressed ? colorA : colorB;
            }
        }
    }
}
