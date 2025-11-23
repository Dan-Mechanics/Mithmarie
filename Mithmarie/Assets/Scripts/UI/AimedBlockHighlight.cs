using System;
using UnityEngine;

namespace Mithmarie
{
    public class AimedBlockHighlight : StateBehaviour
    {
        public event Action<string> OnOutputText;

        [SerializeField] private Transform eyes = default;
        [SerializeField] private GameObject outlineCubePrefab = default;
        [SerializeField] private Raycast raycast = default;
        [SerializeField] private float hitPointExtrusion = default;

        private Transform outline;
        private RaycastHit hit;

        private void Awake()
        {
            outline = Instantiate(outlineCubePrefab, Vector3.zero, Quaternion.identity).transform;
            outline.gameObject.SetActive(false);
        }

        public void Configure(Raycast raycast) => this.raycast = raycast;
        public override void OnTick()
        {
            base.OnFrame();
            Highlight(null, Vector3.zero);
            if (raycast.Cast(eyes, out hit))
                Highlight(Utils.ApplyGrid(hit.point + (hit.normal * hitPointExtrusion)), hit.normal);

        }

        private void Highlight(Vector3Int? blockPos, Vector3 normal)
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

            outline.forward = normal;
        }

    }
}
