using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class DemoMeshGenerator : MonoBehaviour, IMeshGeneratable
    {
        [SerializeField] private GameObject cubePrefab = default;
        private readonly List<GameObject> spawned = new List<GameObject>();
        
        /// <summary>
        /// Should only be used when debugging.
        /// </summary>
        public void GenerateMesh(HashSet<Vector3Int> blocks)
        {
            spawned.ForEach(x => Destroy(x));
            spawned.Clear();

            foreach (Vector3Int blockPos in blocks)
            {
                spawned.Add(Instantiate(cubePrefab, blockPos, Quaternion.identity));
            }
        }
    }
}
