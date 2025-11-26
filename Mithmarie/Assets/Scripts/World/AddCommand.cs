using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class AddCommand : IWorldCommand
    {
        public HashSet<Vector3Int> blocks;

        public AddCommand(HashSet<Vector3Int> blocks)
        {
            this.blocks = blocks;
        }

        public void Execute(World world)
        {
            foreach (Vector3Int block in blocks)
            {
                world.Add(block);
            }
        }

        public void Undo(World world)
        {
            foreach (Vector3Int block in blocks)
            {
                world.Add(block);
            }
        }
    }
}
