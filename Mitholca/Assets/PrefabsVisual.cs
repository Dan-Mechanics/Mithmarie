using System.Collections.Generic;
using UnityEngine;

namespace Mitholca
{
    public class PrefabsVisual : MonoBehaviour, IWorldVisual
    {
        [SerializeField] private GameObject cubePrefab = default;
        private readonly Dictionary<Vector3Int, GameObject> spawned = new Dictionary<Vector3Int, GameObject>();
        
        /// <summary>
        /// Note: very inefficient.
        /// </summary>
        public void Draw(HashSet<Vector3Int> hash)
        {
            // ADD PASS.
            foreach (Vector3Int pos in hash)
            {
                if (!spawned.ContainsKey(pos))
                    spawned.Add(pos, Instantiate(cubePrefab, pos, Quaternion.identity));
            }

            // REMOVE PASS.
            foreach (KeyValuePair<Vector3Int, GameObject> cube in spawned)
            {
                if (hash.Contains(cube.Key))
                    continue;

                Destroy(cube.Value);
                spawned.Remove(cube.Key);
            }
        }
    }
}
