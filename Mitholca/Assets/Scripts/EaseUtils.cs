using UnityEngine;
using UnityEngine.InputSystem;

namespace Mitholca
{
    public class EaseUtils : MonoBehaviour
    {
        [SerializeField, Min(1)] private int fps = default;
        [SerializeField, Min(1f)] private float physicsTicksPerSecond = default;

        private void Start()
        {
            Application.targetFrameRate = fps;
            Time.fixedDeltaTime = 1f / physicsTicksPerSecond;
        }

        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
                Application.Quit();
        }
    }
}