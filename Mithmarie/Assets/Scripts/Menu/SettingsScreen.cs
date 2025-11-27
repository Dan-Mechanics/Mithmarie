using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Mithmarie
{
    public class SettingsScreen : Screen, IDefaultable
    {
        [SerializeField] private Button defaultAllButton = default;

        private StateBehaviour[] behaviours;
        private IDefaultable[] defaultables;
        private IMessageService message;

        public void Setup(IMessageService message)
        {
            List<StateBehaviour> behaviourList = GetComponentsInChildren<StateBehaviour>().ToList();
            behaviourList.Remove(this);
            behaviours = behaviourList.ToArray();

            List<IDefaultable> defaultablesList = GetComponentsInChildren<IDefaultable>().ToList();
            defaultablesList.Remove(this);
            defaultables = defaultablesList.ToArray();  

            gameObject.SetActive(false);
            this.message = message;
        }

        public override void Enter()
        {
            base.Enter();
            gameObject.SetActive(true);

            defaultAllButton.onClick.AddListener(ReturnToDefault);
            for (int i = 0; i < behaviours.Length; i++)
            {
                behaviours[i].Enter();
            }
        }

        public override void Exit()
        {
            base.Exit();
            gameObject.SetActive(false);

            defaultAllButton.onClick.RemoveListener(ReturnToDefault);
            for (int i = 0; i < behaviours.Length; i++)
            {
                behaviours[i].Exit();
            }
        }

        public void ReturnToDefault()
        {
            for (int i = 0; i < defaultables.Length; i++)
            {
                defaultables[i].ReturnToDefault();
            }

            message.Send("...", Color.black, 0.5f);
        }
    }
}