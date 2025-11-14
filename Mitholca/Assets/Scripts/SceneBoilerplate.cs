using UnityEngine;
using UnityEngine.InputSystem;

namespace Mitholca
{
    public class SceneBoilerplate : MonoBehaviour
    {
        [SerializeField, Min(1)] private int fps = default;
        [SerializeField, Min(1f)] private float physicsTicksPerSecond = default;
        [SerializeField] private Key closeKey = default;

        private void Start()
        {
            Application.targetFrameRate = fps;
            Time.fixedDeltaTime = 1f / physicsTicksPerSecond;
        }

        private void Update()
        {
            if (Keyboard.current[closeKey].wasPressedThisFrame)
                Application.Quit();
        }
    }
}