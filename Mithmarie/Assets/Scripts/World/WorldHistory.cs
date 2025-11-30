using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class WorldHistory : MonoBehaviour
    {
        [SerializeField, Min(0)] private int maxHistoryCount = default;
        private readonly List<ImplicitWorldChange> history = new List<ImplicitWorldChange>();
        private int index;
        private World world;

        public void Setup(World world) => this.world = world;

        public void LogImplicitWorldChange(HashSet<Vector3Int> added, HashSet<Vector3Int> removed)
        {
            for (int i = history.Count - 1; i > index; i--)
            {
                history.RemoveAt(i);
            }

            history.Add(new ImplicitWorldChange(added, removed));
            while (history.Count > maxHistoryCount)
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
            if (index < 0 || index >= history.Count)
                return;

            history[index].Undo(world);
            index--;

            world.ForgetRecentChanges();
            world.Flush();
        }

        public void Redo()
        {
            index++;
            if (index < 0 || index >= history.Count)
                return;

            history[index].Redo(world);

            world.ForgetRecentChanges();
            world.Flush();
        }
    }
}