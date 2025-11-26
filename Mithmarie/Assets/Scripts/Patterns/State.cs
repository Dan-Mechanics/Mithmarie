using System;
using UnityEngine;

namespace Mithmarie
{
    public class State : IState
    {
        public event Action<IState> OnYield;

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void OnFrame() { }
        public virtual void OnTick() { }

        protected void Close() => OnYield?.Invoke(this);
    }
}
