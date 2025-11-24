using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class FSM 
    {
        public IState Current => current;
        
        private readonly List<IState> states = new List<IState>();
        private readonly List<Transition> transitions = new List<Transition>();
        private IState current;

        public void AddState(IState state)
        {
            if (state == null)
                return;

            if (!states.Contains(state))
                states.Add(state);
        }

        public void AddTransition(Transition transition)
        {
            if (transition == null)
                return;

            if (!transitions.Contains(transition))
                transitions.Add(transition);
        }

        public void Update()
        {
            current?.OnFrame();

            foreach (var transition in transitions)
            {
                if (transition.from != current)
                    continue;

                if (!transition.goNext())
                    continue;

                Open(transition.to);
                return;
            }
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
            Debug.Log(current.ToString().ToUpperInvariant());
        }

        public void Close()
        {
            if (current == null)
                return;

            current.Exit();
            current = null;
        }
    }
}
