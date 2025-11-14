using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Mitholca
{
    public class SceneBoilerplate : MonoBehaviour
    {
        [SerializeField, Min(1)] private int fps = 300;
        [SerializeField, Min(1f)] private float physicsTicksPerSecond = 50f;
        [SerializeField] private Key closeKey = Key.Q;
        [SerializeField] private Key reloadKey = Key.R;

        private void Start()
        {
            Application.targetFrameRate = 300;
            Time.fixedDeltaTime = 1f / 50f;
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