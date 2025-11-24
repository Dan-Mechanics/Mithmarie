using UnityEngine;
using UnityEngine.UI;

namespace Mithmarie
{
    public class BoolField : StateBehaviour, IDefaultable
    {
        [SerializeField] private Toggle toggle = default;
        [SerializeField] private Button defaultButton = default;
        [SerializeField] private PersistentBool persistent = default;

        private void Start()
        {
            gameObject.name = persistent.name;
        }

        public override void Enter()
        {
            base.Enter();
            persistent.Load();
            toggle.isOn = persistent.value;
            defaultButton.onClick.AddListener(ReturnToDefault);
            toggle.onValueChanged.AddListener(Edit);
        }

        public override void Exit()
        {
            base.Exit();
            persistent.Save();
            defaultButton.onClick.RemoveListener(ReturnToDefault);
            toggle.onValueChanged.RemoveListener(Edit);
        }

        private void Edit(bool value) => persistent.value = value;

        public void ReturnToDefault()
        {
            persistent.value = persistent.defaultValue;
            toggle.isOn = persistent.value;
        }

        private void OnApplicationQuit() => persistent.Save();
    }
}
