using System;
using UnityEngine;

namespace Mithmarie
{
    public class KeyboardShortcuts : StateBehaviour
    {
        public event Action OnSave;
        public event Action OnUndo;
        public event Action OnRedo;
    }
}
