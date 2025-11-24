using System;
using UnityEngine;

namespace Mithmarie
{
    public class AimedBlockOutline : StateBehaviour
    {
        public event Action<object> OnAim;

        [SerializeField] private Transform eyes = default;
        [SerializeField] private GameObject previewPrefab = default;
        [SerializeField, Min(0f)] private float scale = default; 

        private Transform outline;
        private RaycastSettings raycast;
        private RaycastHit hit;

        private void Awake()
        {
            outline = Instantiate(previewPrefab, Vector3.zero, Quaternion.identity).transform;
            outline.localScale = Vector3.one * scale;
            outline.gameObject.SetActive(false);
        }

        private void Start()
        {
            raycast = GetComponent<IRaycastProvider>().GetSettings();
        }

        public override void OnTick()
        {
            base.OnFrame();
            OutlineBlock(null);
            if (raycast.Cast(eyes, out hit))
                OutlineBlock(Utils.ConvertToBlockPos(hit.point));

        }

        private void OutlineBlock(Vector3Int? blockPos)
        {
            if(blockPos == null)
            {
                outline.gameObject.SetActive(false);
                OnAim?.Invoke(null);
                return;
            }

            outline.gameObject.SetActive(true);
            outline.position = (Vector3Int)blockPos;
            OnAim?.Invoke(blockPos);
        }

    }
}
