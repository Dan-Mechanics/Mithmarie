using System.Collections.Generic;
using UnityEngine;

namespace Mitholca
{
    public class World : MonoBehaviour
    {
        /// <summary>
        /// TODO: CHUNKS !!
        /// </summary>
        private readonly HashSet<Vector3Int> blocks = new HashSet<Vector3Int>();
        private IWorldVisual visual;

        private void Awake()
        {
            visual = FindAnyObjectByType<PrefabsVisual>();
        }

        public void SilentAdd(Vector3Int pos)
        {
            if (Has(pos))
                return;

            blocks.Add(pos);
        }

        public void SilentRemove(Vector3Int pos)
        {
            blocks.Remove(pos);
        }

        public void SilentClear()
        {
            blocks.Clear();
        }

        public void Add(Vector3Int pos)
        {
            SilentAdd(pos);
            visual.Draw(blocks);
        }

        public void Remove(Vector3Int pos)
        {
            SilentRemove(pos);
            visual.Draw(blocks);
        }

        public void Clear()
        {
            SilentClear();
            visual.Draw(blocks);
        }

        public bool Has(Vector3Int pos) => blocks.Contains(pos);
    }
}
