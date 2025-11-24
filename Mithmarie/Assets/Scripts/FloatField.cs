using UnityEngine;
using TMPro;

namespace Mithmarie
{
    public class FloatField : StateBehaviour, IDefaultable
    {
        [SerializeField] private TMP_InputField field = default;
        [SerializeField] private TMP_Text placeholder = default;
        [SerializeField] private PersistentFloat persistent = default;
        [SerializeField] private float minValue = default;
        [SerializeField] private float maxValue = default;

        private void Start()
        {
            placeholder.text = $"{minValue} - {maxValue}";
            print($"{gameObject.name} {placeholder.text}");
        }

        public override void Enter()
        {
            base.Enter();
            persistent.Load();
            Edit(persistent.value.ToString());
            field.onEndEdit.AddListener(Edit);
        }

        public override void Exit()
        {
            base.Exit();
            persistent.Save();
            field.onEndEdit.RemoveListener(Edit);
        }

        private void Edit(string str)
        {
            float newValue = persistent.value;
            if (float.TryParse(str, out newValue))
                newValue = Mathf.Clamp(newValue, minValue, maxValue);

            persistent.value = newValue;
            field.text = newValue.ToString();
        }

        public void ReturnToDefault()
        {
            persistent.value = persistent.defaultValue;
            field.text = persistent.value.ToString();
        }
    }
}
