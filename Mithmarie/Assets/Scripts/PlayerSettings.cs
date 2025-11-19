using System.IO;
using UnityEngine;
using System;
using System.Windows.Forms;

namespace Mithmarie
{
    /// <summary>
    /// Make this more SOLID how?
    /// </summary>
    [Serializable]
    public struct PlayerSettings
    {
        private const float defaultSens = 0.33f;
        private const float defaultSpeed = 6f;
        private const bool defaultSawpMouseButtons = false;
        public float sens;
        public float speed;
        public bool swapMouseButtons;

        public void Reset()
        {
            sens = defaultSens;
            speed = defaultSpeed;
            swapMouseButtons = defaultSawpMouseButtons;
        }

        public void Save(string path)
        {
            string json = JsonUtility.ToJson(this);

            if (!File.Exists(path))
                File.CreateText(path);

            File.WriteAllText(path, json);
        }

        public void Load(string path, IMessageService message)
        {
            if (!File.Exists(path))
            {
                message.Send("Loading default player settings", Color.yellow);
                Reset();
                return;
            }

            // DOES THIS EVEN WORK?
            this = JsonUtility.FromJson<PlayerSettings>(File.ReadAllText(path));
        }
    }
}
