using UnityEngine;

namespace Mitholca
{
    public class StateBehaviour : MonoBehaviour, IState
    {
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void OnFrame() { }
        public virtual void OnTick() { }
    }
}
