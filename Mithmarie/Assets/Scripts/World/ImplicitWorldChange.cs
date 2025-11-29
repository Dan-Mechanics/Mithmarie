using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class ImplicitWorldChange
    {
        public HashSet<Vector3Int> added;
        public HashSet<Vector3Int> removed;

        public ImplicitWorldChange(HashSet<Vector3Int> added, HashSet<Vector3Int> removed)
        {
            this.added = new HashSet<Vector3Int>(added);
            this.removed = new HashSet<Vector3Int>(removed);
        }

        public void Undo(World world)
        {
            foreach (Vector3Int addBlock in added)
            {
                world.Remove(addBlock);
            }

            foreach (Vector3Int removeBlock in removed)
            {
                world.Add(removeBlock);
            }
        }

        public void Redo(World world)
        {
            foreach (Vector3Int addBlock in added)
            {
                world.Add(addBlock);
            }

            foreach (Vector3Int removeBlock in removed)
            {
                world.Remove(removeBlock);
            }
        }
    }
}
