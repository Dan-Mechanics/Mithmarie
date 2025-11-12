using UnityEngine;

namespace Mitholca
{
    public class PlayerUtils : MonoBehaviour
    {
        [SerializeField, Min(1)] private int fps = default;
        [SerializeField, Min(1f)] private float ticksPerSecond = default;

        private void Start()
        {
            Application.targetFrameRate = fps;
            Time.fixedDeltaTime = 1f / ticksPerSecond;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                Application.Quit();
        }
    }
}