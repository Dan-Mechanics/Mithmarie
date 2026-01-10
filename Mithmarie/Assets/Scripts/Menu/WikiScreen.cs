using System.Text;
using TMPro;
using UnityEngine;

namespace Mithmarie
{
    public class WikiScreen : Screen
    {
        [SerializeField] private TMP_Text brushesText = default;

        public void Setup(Brush[] brushes)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < brushes.Length; i++)
            {
                Brush brush = brushes[i];
                builder.Append(brush.name.ToUpperInvariant()).Append(": ").
                    Append(brush.tooltip).AppendLine().AppendLine();
            }

            brushesText.text = builder.ToString();
        }
    }
}
