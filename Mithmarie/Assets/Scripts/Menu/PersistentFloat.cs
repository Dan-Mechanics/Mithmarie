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

        public void Save()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            try
            {
                string path = $"{Application.persistentDataPath}/{name}.txt";
                File.WriteAllText(path, value.ToString());
            }
            catch (Exception exception)
            {
                ServiceLocator<IMessageService>.Locate()?.Send(exception.Message, Color.red);
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

                value = defaultValue;
            }
            catch (Exception exception)
            {
                ServiceLocator<IMessageService>.Locate()?.Send(exception.Message, Color.red);
                Debug.LogError(exception.Message);
            }
        }
    }
}