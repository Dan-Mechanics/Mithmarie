using UnityEngine.UI;
using UnityEngine;

namespace Mithmarie
{
    public class Screen : StateBehaviour
    {
        public Button Button => button;
        [SerializeField] private Button button = default;

        public override void Enter()
        {
            base.Enter();
            button.interactable = false;
        }

        public override void Exit()
        {
            base.Enter();
            button.interactable = true;
        }
    }
}