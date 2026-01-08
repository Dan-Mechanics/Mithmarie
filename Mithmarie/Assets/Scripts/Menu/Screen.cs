using UnityEngine.UI;
using UnityEngine;
using System;

namespace Mithmarie
{
    public class Screen : StateBehaviour
    {
        public event Action OnDoneWithTask;

        public Button Button => button;
        [SerializeField] private Button button = default;

        public override void Enter()
        {
            base.Enter();
            gameObject.SetActive(true);
            button.interactable = false;
        }

        public override void Exit()
        {
            base.Enter();
            gameObject.SetActive(false);
            button.interactable = true;
        }

        protected void CloseCompletely() => OnDoneWithTask?.Invoke();
    }
}