using UnityEngine;

namespace Mithmarie
{
    public class SelectionPreview : MonoBehaviour
    {
        [SerializeField] private GameObject previewPrefab = default;
        [SerializeField] private Material previewMaterial = default;
        [SerializeField, Min(1f)] private float scaleOffset = default;

        private GameObject preview;

        private void Awake()
        {
            preview = Instantiate(previewPrefab, Vector3.zero, Quaternion.identity);
        }

        public void UpdatePreview(Vector3Int? firstPos, Vector3Int? secondPos, Color color)
        {
            preview.SetActive(false);
            if (firstPos == null || secondPos == null)
                return;

            if (color.a <= 0f)
                color.a = 0.5f;
            previewMaterial.color = color;

            preview.SetActive(true);
            Vector3Int a = (Vector3Int)firstPos;
            Vector3Int b = (Vector3Int)secondPos;
            preview.transform.position = Vector3.Lerp(a, b, 0.5f);
            preview.transform.localScale = new Vector3(Mathf.Abs(b.x - a.x) + scaleOffset, Mathf.Abs(b.y - a.y) + scaleOffset, Mathf.Abs(b.z - a.z) + scaleOffset);
        }
    }
}
