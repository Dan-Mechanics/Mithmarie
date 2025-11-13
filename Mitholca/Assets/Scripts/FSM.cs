using System.Collections.Generic;

namespace Mitholca
{
    public class FSM 
    {
        private readonly List<State> states = new List<State>();
        private readonly List<Transition> transitions = new List<Transition>();

        private State current;

        public void AddState(State state)
        {
            if (!states.Contains(state))
                states.Add(state);
        }

        public void AddTransition(Transition transition)
        {
            if (!transitions.Contains(transition))
                transitions.Add(transition);
        }

        public void Update()
        {
            foreach (Transition transition in transitions)
            {
                if (transition.from == current && transition.goNext())
                    Enter(transition.to);
            }
            
            current?.Update();
        }

        public void FixedUpdate()
        {
            current?.FixedUpdate();
        }

        public void Enter(State state)
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
