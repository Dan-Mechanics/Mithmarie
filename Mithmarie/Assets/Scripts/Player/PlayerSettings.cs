using System;
using UnityEngine;

namespace Mithmarie
{
    [Serializable]
    public class PlayerSettings
    {
        public string version = "no version";
        public float sensitivity = 0.33f;
        public float speed = 7.5f;
        public bool leftClickIsDestroy = true;
    }
}
