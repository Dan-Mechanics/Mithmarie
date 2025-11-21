using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class Terraformer : StateBehaviour
    {
        [SerializeField] private Transform eyes = default;
        [SerializeField] private Raycast raycast = default;
        [SerializeField] private GameObject previewPrefab = default;

        private World world;
        private RaycastHit hit;
        private Vector3Int? firstPos;
        private Vector3Int? secondPos;
        private Transform preview;
        
        private void Start()
        {
            world = FindAnyObjectByType<World>();
            preview = Instantiate(previewPrefab, Vector3.zero, Quaternion.identity).transform;
            ResetToDefault();
        }

        public override void OnTick()
        {
            base.OnTick();
            if(firstPos != null && raycast.Cast(eyes, out hit))
                secondPos = Utils.ApplyGrid(hit.point + (hit.normal * 0.5f));

            UpdatePreview();
        }

        private void UpdatePreview()
        {
            preview.gameObject.SetActive(false);
            if (firstPos == null || secondPos == null)
                return;

            preview.gameObject.SetActive(true);
            Vector3Int a = (Vector3Int)firstPos;
            Vector3Int b = (Vector3Int)secondPos;
            preview.position = Vector3.Lerp(a, b, 0.5f);
            preview.localScale = new Vector3(Mathf.Abs(b.x - a.x) + 1f, Mathf.Abs(b.y - a.y) + 1f, Mathf.Abs(b.z - a.z) + 1f);
        }

        public override void OnFrame()
        {
            base.OnFrame();
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                ResetToDefault();
                if(raycast.Cast(eyes, out hit))
                    firstPos = Utils.ApplyGrid(hit.point + (hit.normal * 0.5f));
            }

            if (firstPos != null && Mouse.current.rightButton.wasReleasedThisFrame)
            {
                if (raycast.Cast(eyes, out hit))
                {
                    secondPos = Utils.ApplyGrid(hit.point + (hit.normal * 0.5f));
                    world.AddSelection((Vector3Int)firstPos, (Vector3Int)secondPos);
                    world.Flush();
                }

                ResetToDefault();
            }
        }

        private void ResetToDefault()
        {
            firstPos = null;
            secondPos = null;
        }
    }
}
