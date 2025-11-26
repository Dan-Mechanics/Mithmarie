using System;
using UnityEngine;

namespace Mithmarie
{
    public class AimedFaceFeedback : StateBehaviour
    {
        public event Action<object> OnAim;

        [SerializeField] private Transform eyes = default;
        [SerializeField] private GameObject faceOutlinePrefab = default;
        [SerializeField, Min(0f)] private float scale = default; 

        private Transform faceOutline;
        private RaycastSettings raycast;
        private RaycastHit hit;

        private void Awake()
        {
            faceOutline = Instantiate(faceOutlinePrefab, Vector3.zero, Quaternion.identity).transform;
            faceOutline.localScale = Vector3.one * scale;
            faceOutline.gameObject.SetActive(false);
        }

        private void Start()
        {
            raycast = GetComponent<IRaycastProvider>().GetSettings();
            OnAim += OutlineFace;
        }

        public override void OnTick()
        {
            base.OnFrame();
            OutlineFace(null);
            if (!raycast.Cast(eyes, out hit))
                return;

            faceOutline.forward = hit.normal;
            OnAim?.Invoke(Utils.ConvertToBlockPos(hit.point));
        }

        private void OutlineFace(object blockPos)
        {
            faceOutline.gameObject.SetActive(blockPos == null);
            if (!faceOutline.gameObject.activeSelf)
                return;

            faceOutline.position = (Vector3Int)blockPos;
            OnAim?.Invoke(blockPos);
        }

        private void OnDestroy()
        {
            OnAim -= OutlineFace;
        }
    }
}
