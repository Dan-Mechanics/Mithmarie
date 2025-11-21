using System;
using UnityEngine;

namespace Mithmarie
{
    public class AimedBlockHighlight : StateBehaviour
    {
        public event Action<string> OnOutputText;

        [SerializeField] private Transform eyes = default;
        [SerializeField] private GameObject outlineCubePrefab = default;

        private Transform outline;
        private Raycast raycast;
        private float hitPointExtrusion;
        private RaycastHit hit;

        private void Awake()
        {
            outline = Instantiate(outlineCubePrefab, Vector3.zero, Quaternion.identity).transform;
            outline.gameObject.SetActive(false);
        }

        public void Configure(Raycast raycast, float hitPointExtrusion)
        {
            this.raycast = raycast;
            this.hitPointExtrusion = hitPointExtrusion;
        }

        public override void OnTick()
        {
            base.OnFrame();
            Highlight(null);
            if (raycast.Cast(eyes, out hit))
                Highlight(Utils.ApplyGrid(hit.point + (hit.normal * hitPointExtrusion)));

        }

        private void Highlight(Vector3Int? blockPos)
        {
            if(blockPos == null)
            {
                outline.gameObject.SetActive(false);
                OnOutputText?.Invoke(string.Empty);
                return;
            }

            outline.gameObject.SetActive(true);
            outline.position = (Vector3Int)blockPos;
            OnOutputText?.Invoke(blockPos.ToString());
        }

    }
}
