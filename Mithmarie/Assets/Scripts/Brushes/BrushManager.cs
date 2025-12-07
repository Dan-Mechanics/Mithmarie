using UnityEngine;

namespace Mithmarie
{
    /// <summary>
    /// Make a seperate class for UI memes.
    /// </summary>
    public class BrushManager : StateBehaviour
    {
        /// <summary>
        /// Use ScriptableObjects.
        /// </summary>
        private readonly IBrush[] brushes = new IBrush[]
        {
            new Sphere(),
            new Walls(),
            new Fill()
        };

        private IBrush current;

        public void Setup(World world)
        {
            for (int i = 0; i < brushes.Length; i++)
            {
                brushes[i].Setup(world);
            }

            current = brushes[0];
        }

        public void AddSelection(Vector3Int a, Vector3Int b) => current?.Add(a, b);
        public void RemoveSelection(Vector3Int a, Vector3Int b) => current?.Remove(a, b);
    }
}
