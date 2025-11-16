using UnityEngine;

namespace Mitholca
{
    public class State : IState
    {
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void OnFrame() { }
        public virtual void OnTick() { }
    }
}
