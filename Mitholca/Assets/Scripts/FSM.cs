using System.Collections.Generic;

namespace Mitholca
{
    public class FSM 
    {
        private readonly List<IState> states = new List<IState>();
        private readonly List<Transition> transitions = new List<Transition>();

        private IState current;

        public void AddState(IState state)
        {
            if (!states.Contains(state))
                states.Add(state);
        }

        public void AddTransition(Transition transition)
        {
            if (!transitions.Contains(transition))
                transitions.Add(transition);
        }

        public void OnFrame()
        {
            foreach (Transition transition in transitions)
            {
                if (transition.from == current && transition.goNext())
                    Enter(transition.to);
            }
            
            current?.OnFrame();
        }

        public void OnTick()
        {
            current?.OnTick();
        }

        public void Enter(IState state)
        {
            if (state == null)
                return;
            
            if (!states.Contains(state))
                return;

            if (state == current)
                return;

            current?.Exit();
            current = state;
            current.Enter();
        }
    }
}
