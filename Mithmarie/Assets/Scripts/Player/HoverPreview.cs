using UnityEngine;

namespace Mithmarie
{
    public class HoverPreview : MonoBehaviour
    {
        [SerializeField] private GameObject facePreviewPrefab = default;
        [SerializeField] private GameObject cubePreviewPrefab = default;
        [SerializeField, Min(1f)] private float scale = default;

        private Transform facePreview;
        private Transform cubePreview;

        public void Setup()
        {
            facePreview = Instantiate(facePreviewPrefab, Vector3.zero, Quaternion.identity).transform;
            cubePreview = Instantiate(cubePreviewPrefab, Vector3.zero, Quaternion.identity).transform;

            facePreview.localScale = Vector3.one * scale;
            cubePreview.localScale = Vector3.one * scale;

            facePreview.gameObject.SetActive(false);
            cubePreview.gameObject.SetActive(false);
        }

        public void UpdatePreview(HoverInformation hover)
        {
            if(hover == null)
            {
                facePreview.gameObject.SetActive(false);
                cubePreview.gameObject.SetActive(false);
                return;
            }

            facePreview.gameObject.SetActive(hover.hasHit);
            cubePreview.gameObject.SetActive(!hover.hasHit);

            cubePreview.position = hover.pos;
            if (!hover.hasHit)
                return;

            facePreview.position = hover.pos;
            facePreview.forward = hover.normal;
        }
    }
}
