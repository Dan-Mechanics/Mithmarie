using UnityEngine;
using System;
using UnityEngine.InputSystem;

namespace Mitholca
{
    [Serializable]
    public struct PlayerSettings 
    {
        /// <summary>
        /// This is global because of UI feedback.
        /// </summary>
        public const Key SPRINT_KEY = Key.LeftCtrl;
        
        [Min(1f)] public float speed;
        [Min(0.001f)] public float sensitivity;
    }
}
