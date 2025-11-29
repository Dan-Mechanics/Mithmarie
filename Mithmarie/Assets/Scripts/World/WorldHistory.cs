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

        private void Awake()
        {
            world = FindAnyObjectByType<World>();
        }

        public void LogImplicitWorldChange(HashSet<Vector3Int> added, HashSet<Vector3Int> removed)
        {
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
            Debug.Log("hello");
            Debug.Log(history.Count);
            if (index < 0 || index >= history.Count)
                return;

            history[index].Undo(world);
            index--;

            world.ForgetRecentChanges();
            world.Flush();
        }

        public void Redo()
        {
            if (index < 0 || index >= history.Count)
                return;

            history[index].Redo(world);
            index++;

            world.ForgetRecentChanges();
            world.Flush();
        }
    }
}