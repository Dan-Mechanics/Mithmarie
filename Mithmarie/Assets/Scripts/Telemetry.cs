using TMPro;
using UnityEngine;

namespace Mithmarie
{
    public class Telemetry : MonoBehaviour
    {
        private TMP_Text text;
        private void Awake()
        {
            text = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            text.text = Mathf.RoundToInt(1f / Time.smoothDeltaTime).ToString();
        }
    }
}
