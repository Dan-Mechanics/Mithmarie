using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Mithmarie
{
    public class SettingsScreen : Screen
    {
        [SerializeField] private Button defaultAllButton = default;

        private StateBehaviour[] behaviours;
        private IDefaultable[] defaultables;
        private IMessageService message;

        public void Setup(IMessageService message)
        {
            List<StateBehaviour> list = GetComponentsInChildren<StateBehaviour>().ToList();
            list.RemoveAt(list.FindIndex(x => x is SettingsScreen));
            behaviours = list.ToArray();

            gameObject.SetActive(false);
            defaultables = GetComponentsInChildren<IDefaultable>();
            this.message = message;
        }

        public override void Enter()
        {
            base.Enter();
            gameObject.SetActive(true);

            defaultAllButton.onClick.AddListener(DefaultAll);
            for (int i = 0; i < behaviours.Length; i++)
            {
                behaviours[i].Enter();
            }
        }

        public override void Exit()
        {
            base.Exit();
            gameObject.SetActive(false);

            defaultAllButton.onClick.RemoveListener(DefaultAll);
            for (int i = 0; i < behaviours.Length; i++)
            {
                behaviours[i].Exit();
            }
        }

        private void DefaultAll()
        {
            for (int i = 0; i < defaultables.Length; i++)
            {
                defaultables[i].ReturnToDefault();
            }

            message.Send("...", Color.black, 0.5f);
        }
    }
}