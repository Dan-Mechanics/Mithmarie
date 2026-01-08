using UnityEngine;

namespace Mithmarie
{
    [CreateAssetMenu(menuName = "ScriptableObject/" + nameof(Brush), fileName = "New " + nameof(Brush))]
    public class Brush : ScriptableObject
    {
        public Sprite icon;
        public GameObject prefab;
        public Mesh previewMesh;
        public string tooltip;
        public IBrushable brushable;

        private void OnValidate()
        {
            if (prefab == null)
                return;

            previewMesh = prefab.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
        }
    }
}