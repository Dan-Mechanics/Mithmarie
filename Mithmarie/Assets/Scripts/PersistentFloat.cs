using System;
using System.Globalization;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Mithmarie
{
    [CreateAssetMenu(menuName = "ScriptableObject/" + nameof(PersistentFloat), fileName = "New " + nameof(PersistentFloat))]
    public class PersistentFloat : ScriptableObject
    {
        [HideInInspector] public float value;
        public float defaultValue;
        private IMessageService message;

        public void Setup(IMessageService message) => this.message = message;

        public void Save()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            try
            {
                string path = $"{Application.persistentDataPath}/{name}.txt";
                Debug.Log(path);
                // does it need to exist first ??
                File.WriteAllText(path, value.ToString());
            }
            catch (Exception exception)
            {
                message.Send(exception.Message, Color.red);
            }
        }

        public void Load()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            try
            {
                string path = $"{Application.persistentDataPath}/{name}.txt";
                if (File.Exists(path) && float.TryParse(File.ReadAllText(path), out value))
                    return;

                message.Send($"Loading default value for {name} ...", Color.yellow, 0.5f);
                value = defaultValue;
            }
            catch (Exception exception)
            {
                message.Send(exception.Message, Color.red);
            }
        }
    }
}