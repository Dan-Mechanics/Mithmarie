using System;
using System.Globalization;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Mithmarie
{
    [CreateAssetMenu(menuName = "ScriptableObject/" + nameof(PersistentFloat), fileName = "New " + nameof(PersistentFloat))]
    public class PersistentFloat : ScriptableObject, IDefaultable
    {
        public float Value => value;
        
        public float defaultValue;
        public float minValue;
        public float maxValue;

        private float value;

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
                if (File.Exists(path) && float.TryParse(File.ReadAllText(path), out float newValue))
                {
                    Set(newValue);
                    return;
                }

                ReturnToDefault();
            }
            catch (Exception exception)
            {
                ServiceLocator<IMessageService>.Locate()?.Send(exception.Message, Color.red);
            }
        }


        public void Set(float newValue)
        {
            newValue = Mathf.Clamp(newValue, minValue, maxValue);
            value = newValue;
        }

        public override string ToString() => value.ToString();
        public void ReturnToDefault() => Set(defaultValue);

        private void OnValidate()
        {
            if (minValue > maxValue)
                Debug.LogWarning($"minValue > maxValue {name}.");

            if (defaultValue > maxValue)
                Debug.LogWarning($"defaultValue > maxValue {name}.");

            if (defaultValue < minValue)
                Debug.LogWarning($"defaultValue < minValue {name}.");
        }
    }
}