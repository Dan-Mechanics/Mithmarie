using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Mithmarie
{
    public class FloatField : StateBehaviour, IDefaultable
    {
        private const int MAX_STRING_LENGTH = 25;
        
        [SerializeField] private TMP_InputField field = default;
        [SerializeField] private TMP_Text placeholder = default;
        [SerializeField] private Button defaultButton = default;
        [SerializeField] private PersistentFloat persistent = default;
        private bool hasChanged;

        private void Start()
        {
            gameObject.name = persistent.name;
            placeholder.text = $"{persistent.minValue} to {persistent.maxValue}";
            print($"{gameObject.name} {placeholder.text}");
        }

        public override void Enter()
        {
            base.Enter();
            persistent.Load();
            Edit(persistent.ToString());
            defaultButton.onClick.AddListener(ReturnToDefault);
            field.onEndEdit.AddListener(Edit);
        }

        public override void Exit()
        {
            base.Exit();
            if (hasChanged)
                persistent.Save();

            hasChanged = false;
            defaultButton.onClick.RemoveListener(ReturnToDefault);
            field.onEndEdit.RemoveListener(Edit);
        }

        /// <summary>
        /// Sanitizes string.
        /// </summary>
        private void Edit(string str)
        {
            if (str.Length > MAX_STRING_LENGTH || !Utils.IsStringValid(str))
                return;

            str = str.Replace(',', '.');
            if (!float.TryParse(str, out float value))
                value = persistent.Value;

            hasChanged = true;
            persistent.Set(value);
            field.text = persistent.ToString();
        }

        public void ReturnToDefault()
        {
            persistent.ReturnToDefault();
            field.text = persistent.ToString();
        }

        private void OnApplicationQuit() => persistent.Save();
    }
}
