using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Mithmarie
{
    public class FloatField : StateBehaviour, IDefaultable
    {
        [SerializeField] private TMP_InputField field = default;
        [SerializeField] private TMP_Text placeholder = default;
        [SerializeField] private Button defaultButton = default;
        [SerializeField] private PersistentFloat persistent = default;
        [SerializeField] private float minValue = default;
        [SerializeField] private float maxValue = default;

        private void Start()
        {
            gameObject.name = persistent.name;
            placeholder.text = $"{minValue} --- {maxValue}";
            print($"{gameObject.name} {placeholder.text}");
        }

        public override void Enter()
        {
            base.Enter();
            persistent.Load();
            Edit(persistent.value.ToString());
            defaultButton.onClick.AddListener(ReturnToDefault);
            field.onEndEdit.AddListener(Edit);
        }

        public override void Exit()
        {
            base.Exit();
            persistent.Save();
            defaultButton.onClick.RemoveListener(ReturnToDefault);
            field.onEndEdit.RemoveListener(Edit);
        }

        private void Edit(string str)
        {
            if (!float.TryParse(str, out float newValue))
                newValue = persistent.value;

            newValue = Mathf.Clamp(newValue, minValue, maxValue);
            persistent.value = newValue;
            field.text = newValue.ToString();
        }

        public void ReturnToDefault()
        {
            persistent.value = persistent.defaultValue;
            field.text = persistent.value.ToString();
        }

        private void OnApplicationQuit() => persistent.Save();
    }
}
