using System;

namespace Mithmarie
{
    public interface IState 
    {
        public event Action<IState> OnYield;
        void Enter();
        void Exit();
        void OnFrame();
        void OnTick();
    }
}
