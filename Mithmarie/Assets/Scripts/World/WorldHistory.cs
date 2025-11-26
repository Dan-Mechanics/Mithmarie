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

        public void EnscribeAddCommand(HashSet<Vector3Int> blocks) => EnscribeCommand(new AddCommand(blocks));
        public void EnscribeRemoveCommand(HashSet<Vector3Int> blocks) => EnscribeCommand(new RemoveCommand(blocks));

        private void EnscribeCommand(IWorldCommand command)
        {
            history.Add(command);
            while(history.Count > maxHistoryCount)
            {
                history.RemoveAt(0);
            }

            index = history.Count - 1;
        }

        public void Clear()
        {
            history.Clear();
            index = 0;
        }

        public void Undo()
        {
            if (index >= history.Count)
                return;

            if (index < 0)
                return;

            history[index].Undo(world);
            index--;

            world.ClearCaches();
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

            world.ClearCaches();
            world.Flush();
        }
    }
}