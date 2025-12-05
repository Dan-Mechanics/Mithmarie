using UnityEngine;

namespace Mithmarie
{
    public class SelectionPreview : MonoBehaviour
    {
        [SerializeField] private GameObject previewPrefab = default;
        [SerializeField, Min(1f)] private float scale = default;
        private Transform preview;

        public void Setup()
        {
            preview = Instantiate(previewPrefab, Vector3.zero, Quaternion.identity).transform;
            preview.gameObject.SetActive(false);
        }

        public void UpdatePreview(SelectionInformation selection)
        {
            if(selection == null)
            {
                preview.gameObject.SetActive(false);
                return;
            }

            preview.gameObject.SetActive(true);
            preview.transform.position = Vector3.Lerp(selection.a, selection.b, 0.5f);
            preview.transform.localScale = new Vector3(
                Mathf.Abs(selection.b.x - selection.a.x) + scale,
                Mathf.Abs(selection.b.y - selection.a.y) + scale,
                Mathf.Abs(selection.b.z - selection.a.z) + scale);
        }
    }
}
