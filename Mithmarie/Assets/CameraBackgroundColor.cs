using UnityEngine;

namespace Mithmarie
{
    public class CameraBackgroundColor : MonoBehaviour
    {
        [SerializeField] private Camera cam = default;
        [SerializeField] private Color insideBlockColor = default;

        private Color defaultColor;
        private World world;

        private void Start()
        {
            world = FindAnyObjectByType<World>();
            defaultColor = cam.backgroundColor;
            RenderSettings.fogColor = insideBlockColor;
        }

        private void FixedUpdate()
        {
            bool openAir = !world.Has(Utils.ApplyGrid(transform.position));
            cam.backgroundColor = openAir ? defaultColor : insideBlockColor;
            RenderSettings.fog = !openAir;
        }
    }
}
