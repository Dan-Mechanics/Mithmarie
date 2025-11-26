using System;
using System.Globalization;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Mithmarie
{
    [CreateAssetMenu(menuName = "ScriptableObject/" + nameof(PersistentBool), fileName = "New " + nameof(PersistentBool))]
    public class PersistentBool : ScriptableObject, IDefaultable
    {
        [HideInInspector] public bool value;
        public bool defaultValue;

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
                if (File.Exists(path) && bool.TryParse(File.ReadAllText(path), out value))
                    return;

                ReturnToDefault();
            }
            catch (Exception exception)
            {
                ServiceLocator<IMessageService>.Locate()?.Send(exception.Message, Color.red);
            }
        }

        public void ReturnToDefault() => value = defaultValue;
    }
}