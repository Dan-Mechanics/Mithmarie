using TMPro;
using UnityEngine;

namespace Mithmarie
{
    public class Telemetry : MonoBehaviour
    {
        [SerializeField] private TMP_Text text = default;

        private void Update()
        {
            text.text = Mathf.RoundToInt(1f / Time.smoothDeltaTime).ToString();
        }
    }
}
