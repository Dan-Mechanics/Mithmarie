using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class WorldHistory : MonoBehaviour
    {
        [SerializeField, Min(0)] private int maxHistoryCount = default;
        private readonly List<IWorldCommand> history = new List<IWorldCommand>();
        private int index;
        private World world;

        private void Awake()
        {
            world = FindAnyObjectByType<World>();
        }

        public void LogAdd(HashSet<Vector3Int> blocks) => LogCommand(new AddCommand(blocks));
        public void LogRemove(HashSet<Vector3Int> blocks) => LogCommand(new RemoveCommand(blocks));

        private void LogCommand(IWorldCommand command)
        {
            history.Add(command);
            while(history.Count > maxHistoryCount)
            {
                history.RemoveAt(0);
            }

            index = history.Count - 1;
        }

        public void Undo()
        {
            if (index >= history.Count)
                return;

            if (index < 0)
                return;

            history[index].Undo(world);
            index--;
            world.Flush();
        }

        public void Redo()
        {
            if (index >= history.Count - 1)
                return;

            if (index < 0)
                return;

            history[index].Execute(world);
            index++;
        }
    }
}