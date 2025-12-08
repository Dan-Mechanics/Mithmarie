using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    /// <summary>
    /// Make a seperate class for UI memes.
    /// </summary>
    public class BrushManager : StateBehaviour
    {
        public event Action<int> OnNewBrushSelected;

        [SerializeField] private Brush[] brushes = default;
        [SerializeField, Min(0f)] private float deadzone = default;
        private readonly IBrushable[] brushables = new IBrushable[]
        {
            new Fill(),
            new Walls(),
            new Cylinder()
        };

        private IBrushable current;
        private int index;

        public void Setup(World world)
        {
            for (int i = 0; i < brushables.Length; i++)
            {
                brushables[i].Setup(world);
            }

            index = -1;
            ChangeBrush(1);
        }

        public void AddSelection(Vector3Int a, Vector3Int b) => current?.Add(a, b);
        public void RemoveSelection(Vector3Int a, Vector3Int b) => current?.Remove(a, b);

        public override void OnFrame()
        {
            base.OnFrame();
            float value = Mouse.current.scroll.value.y;

            if (value > deadzone)
            {
                ChangeBrush(1);
            }
            else if (value < -deadzone)
            {
                ChangeBrush(-1);
            }
        }

        private void ChangeBrush(int direction)
        {
            index += direction;
            index %= brushables.Length;
            current = brushables[index];

            OnNewBrushSelected?.Invoke(index);
        }
    }
}
