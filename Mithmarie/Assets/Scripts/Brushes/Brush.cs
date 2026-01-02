using UnityEngine;

namespace Mithmarie
{
    [CreateAssetMenu(menuName = "ScriptableObject/" + nameof(Brush), fileName = "New " + nameof(Brush))]
    public class Brush : ScriptableObject
    {
        public Sprite icon;
        public Mesh previewMesh;
        public IBrushable brushable;
    }
}