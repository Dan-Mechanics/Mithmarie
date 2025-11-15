using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Mitholca
{
    public class SceneBoilerplate : MonoBehaviour
    {
        [SerializeField, Min(1)] private int fps = default;
        [SerializeField, Min(1f)] private float physicsTicksPerSecond = default;
        [SerializeField] private Key closeKey = default;
        [SerializeField] private Key reloadKey = default;

        private void Awake()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Application.targetFrameRate = fps;
            Time.fixedDeltaTime = 1f / physicsTicksPerSecond;
        }

        private void Update()
        {
            if (Keyboard.current[closeKey].wasPressedThisFrame)
                Application.Quit();

            if (Keyboard.current[reloadKey].wasPressedThisFrame)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}