using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class StairsPreview : MonoBehaviour
    {
        public event Action OnNewStairsMesh;
        
        [SerializeField] private Brush stairs = default;
        [SerializeField] private GameObject[] prefabs = default;

        private Dictionary<CardinalDirection, Mesh> directionToMesh;
        private CardinalDirection prev;
        private Transform player;

        public void Setup(Transform player)
        {
            this.player = player;

            directionToMesh = new Dictionary<CardinalDirection, Mesh>();
            for (int i = 0; i < prefabs.Length; i++)
            {
                directionToMesh.Add((CardinalDirection)i,
                    prefabs[i].transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh);
            }

            stairs.previewMesh = directionToMesh[CardinalDirection.North];
        }

        private void FixedUpdate()
        {
            CardinalDirection cardinal = Utils.GetCardinal(player.rotation.eulerAngles.y);
            if (cardinal != prev)
            {
                stairs.previewMesh = directionToMesh[cardinal];
                OnNewStairsMesh?.Invoke();
            }

            prev = cardinal;
        }
    }
}
