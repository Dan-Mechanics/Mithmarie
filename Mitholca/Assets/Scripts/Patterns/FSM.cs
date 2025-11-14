using System.Collections.Generic;
using System;

namespace Mitholca
{
    public class FSM 
    {
        private readonly List<IState> states = new List<IState>();
        private readonly List<Transition> transitions = new List<Transition>();

        private IState current;

        public void AddState(IState state)
        {
            if (state == null)
                throw new NullReferenceException();
            
            if (!states.Contains(state))
                states.Add(state);
        }

        public void AddTransition(Transition transition)
        {
            if (transition == null)
                throw new NullReferenceException();

            if (!transitions.Contains(transition))
                transitions.Add(transition);
        }

        public void Update()
        {
            // !DICT
            foreach (var transition in transitions)
            {
                if (transition.from != current)
                    continue;

                if (!transition.goNext())
                    continue;

                Open(transition.to);
                return;
            }
            
            current?.OnFrame();
        }

        public void FixedUpdate()
        {
            current?.OnTick();
        }

        public void Open(IState state)
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
