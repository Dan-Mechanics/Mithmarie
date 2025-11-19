using System.Globalization;
using System.Threading;
using UnityEngine;

namespace Mithmarie
{
    public class SceneBoilerplate : MonoBehaviour
    {
        [SerializeField, Min(1)] private int fps = default;
        [SerializeField, Min(1f)] private float physicsTicksPerSecond = default;

        private void Awake()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Application.targetFrameRate = fps;
            Time.fixedDeltaTime = 1f / physicsTicksPerSecond;
        }
    }
}