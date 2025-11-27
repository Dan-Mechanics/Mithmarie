using UnityEngine;
using UnityEngine.UI;

namespace Mithmarie
{
    public class BoolField : StateBehaviour, IDefaultable
    {
        [SerializeField] private Toggle toggle = default;
        [SerializeField] private Button defaultButton = default;
        [SerializeField] private PersistentBool persistent = default;
        private bool hasChanged;

        /*private void Start()
        {
            gameObject.name = persistent.name;
        }*/

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
            if (hasChanged)
                persistent.Save();

            hasChanged = false;
            defaultButton.onClick.RemoveListener(ReturnToDefault);
            toggle.onValueChanged.RemoveListener(Edit);
        }

        private void Edit(bool value)
        {
            persistent.value = value;
            hasChanged = true;
        }

        public void ReturnToDefault()
        {
            persistent.ReturnToDefault();
            toggle.isOn = persistent.value;
        }

        private void OnApplicationQuit()
        {
            if (!hasChanged)
                return;

            persistent.Save();
        }
    }
}
