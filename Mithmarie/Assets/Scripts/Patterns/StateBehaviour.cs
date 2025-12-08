using System;
using UnityEngine;

namespace Mithmarie
{
    public class StateBehaviour : MonoBehaviour, IState
    {
        public event Action<IState> OnYield;

        public virtual void Enter() => print(ToString().ToLowerInvariant());
        public virtual void Exit() { }
        public virtual void OnFrame() { }
        public virtual void OnTick() { }
        protected void Close() => OnYield?.Invoke(this);
    }
}
