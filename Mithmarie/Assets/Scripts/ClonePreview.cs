using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class ClonePreview : MonoBehaviour
    {
        [SerializeField] private MeshFilter filter = default;
        [SerializeField] private MeshCollider coll = default;
        [SerializeField] private Vector3 offset = default;
        private int brushIndex;

        public void Setup(int brushIndex)
        {
            this.brushIndex = brushIndex;
            SetMesh(null);
        }

        public void Show(HashSet<Vector3Int> blocks)
        {
            IWorldMeshStrategy worldMeshStrat = new CulledWorldMesh();
            SetMesh(worldMeshStrat.GenerateMesh(blocks));
            gameObject.SetActive(true);
        }

        private void SetMesh(Mesh mesh)
        {
            filter.sharedMesh = mesh;
            coll.sharedMesh = mesh;
        }

        public void UpdatePreview(HoverInformation hover)
        {
            if (hover == null || !gameObject.activeSelf)
                return;

            transform.position = hover.pos + offset;
        }

        /// <summary>
        /// Invalid indices hide preview.
        /// </summary>
        public void SetVisibilityWithBrushIndex(int index)
        {
            if(index < 0)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(index == brushIndex);
        }
    }
}
