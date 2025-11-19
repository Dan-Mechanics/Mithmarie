using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    /// <summary>
    /// In the future you could have GreedyMeshGenerator.cs 
    /// for the most performance in the final export.
    /// </summary>
    public class CulledMeshGenerator : MonoBehaviour, IMeshGeneratable
    {
        [SerializeField] private GameObject cubePrefab = default;
        private readonly List<GameObject> spawned = new List<GameObject>();
        
        /// <summary>
        /// Note: very inefficient.
        /// </summary>
        public void GenerateMesh(HashSet<Vector3Int> hash)
        {
            spawned.ForEach(x => Destroy(x));
            spawned.Clear();

            foreach (Vector3Int pos in hash)
            {
                spawned.Add(Instantiate(cubePrefab, pos, Quaternion.identity));
            }
        }
    }
}
