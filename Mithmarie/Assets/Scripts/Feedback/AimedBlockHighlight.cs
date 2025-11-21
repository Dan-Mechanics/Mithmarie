using System;
using UnityEngine;

namespace Mithmarie
{
    public class AimedBlockHighlight : StateBehaviour
    {
        public event Action<string> OnOutputText;

        [SerializeField] private Transform eyes = default;
        [SerializeField] private GameObject hoverCubePrefab = default;

        private Transform hover;
        private Raycast raycast;
        private float normalDirection;
        private RaycastHit hit;

        private void Awake()
        {
            hover = Instantiate(hoverCubePrefab, Vector3.zero, Quaternion.identity).transform;
            hover.gameObject.SetActive(false);
        }

        public void Configure(Raycast raycast, float normalDirection)
        {
            this.raycast = raycast;
            this.normalDirection = normalDirection;
        }

        public override void OnTick()
        {
            base.OnFrame();
            Highlight(null);
            if (raycast.Cast(eyes, out hit))
                Highlight(Utils.ApplyGrid(hit.point + (hit.normal * normalDirection)));

        }

        private void Highlight(Vector3Int? blockPos)
        {
            if(blockPos == null)
            {
                hover.gameObject.SetActive(false);
                OnOutputText?.Invoke(string.Empty);
                return;
            }

            hover.gameObject.SetActive(true);
            hover.position = (Vector3Int)blockPos;
            OnOutputText?.Invoke(blockPos.ToString());
        }

    }
}
