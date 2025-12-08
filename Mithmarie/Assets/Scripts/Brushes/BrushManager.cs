using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class BrushManager : StateBehaviour
    {
        public event Action<int> OnNewBrushSelected;
        
        [SerializeField, Min(0f)] private float deadzone = default;

        private IBrushable[] brushables;
        private IBrushable current;
        private int index;

        public void Setup(World world, Transform eyes)
        {
            brushables = new IBrushable[]
            {
                new Fill(world),
                new Walls(world),
                new Cylinder(world),
                new Stairs(world, eyes),
                new Clone(world)
            };

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
            if (index < 0)
                index += brushables.Length;

            current = brushables[index];
            OnNewBrushSelected?.Invoke(index);
        }
    }
}
