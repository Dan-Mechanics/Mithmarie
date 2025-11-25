using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    /// <summary>
    /// Exact opposite of AddCommand.
    /// </summary>
    public class RemoveCommand : IWorldCommand
    {
        private readonly AddCommand add;

        public RemoveCommand(HashSet<Vector3Int> blocks)
        {
            add = new AddCommand(blocks);
        }

        public void Execute(World world)
        {
            add.Undo(world);
        }

        public void Undo(World world)
        {
            add.Execute(world);
        }
    }
}
