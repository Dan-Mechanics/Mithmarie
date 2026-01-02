using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class BrushManager : StateBehaviour
    {
        public event Action<int> OnNewBrushSelected;
        public event Action<Mesh> OnNewPreviewMesh;

        [SerializeField, Min(0f)] private float scrollDeadzone = default;

        private Brush[] brushes;
        private Brush current;
        private int index;

        public void Setup(Brush[] brushes, IBrushable[] brushables)
        {
            for (int i = 0; i < brushes.Length; i++)
            {
                brushes[i].brushable = brushables[i];
            }

            this.brushes = brushes;
            index = -1;
            ChangeBrush(1);
        }

        public void AddSelection(Vector3Int a, Vector3Int b)
        {
            if (current == null)
                return;

            current.brushable?.Add(a, b);
        }

        public void RemoveSelection(Vector3Int a, Vector3Int b)
        {
            if (current == null)
                return;

            current.brushable?.Remove(a, b);
        }

        public override void OnFrame()
        {
            base.OnFrame();
            float value = Mouse.current.scroll.value.y;

            if (value > scrollDeadzone)
            {
                ChangeBrush(1);
            }
            else if (value < -scrollDeadzone)
            {
                ChangeBrush(-1);
            }
        }

        private void ChangeBrush(int direction)
        {
            index += direction;
            index %= brushes.Length;
            if (index < 0)
                index += brushes.Length;

            current = brushes[index];
            OnNewBrushSelected?.Invoke(index);
            OnNewPreviewMesh?.Invoke(current.previewMesh);
        }
    }
}
