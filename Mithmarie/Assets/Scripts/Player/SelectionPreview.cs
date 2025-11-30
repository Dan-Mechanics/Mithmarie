using UnityEngine;

namespace Mithmarie
{
    public class SelectionPreview : MonoBehaviour
    {
        [SerializeField] private GameObject previewPrefab = default;
        [SerializeField, Min(1f)] private float scale = default;
        private GameObject preview;

        public void Setup()
        {
            preview = Instantiate(previewPrefab, Vector3.zero, Quaternion.identity);
        }

        public void UpdatePreview(Vector3Int? firstPos, Vector3Int? secondPos)
        {
            preview.SetActive(false);
            if (firstPos == null || secondPos == null)
                return;

            preview.SetActive(true);
            Vector3Int a = (Vector3Int)firstPos;
            Vector3Int b = (Vector3Int)secondPos;
            preview.transform.position = Vector3.Lerp(a, b, 0.5f);
            preview.transform.localScale = new Vector3(Mathf.Abs(b.x - a.x) + scale, Mathf.Abs(b.y - a.y) + scale, Mathf.Abs(b.z - a.z) + scale);
        }
    }
}
