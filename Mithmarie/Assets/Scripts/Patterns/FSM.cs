using System.Collections.Generic;
using UnityEngine;

namespace Mithmarie
{
    public class FSM 
    {
        private readonly List<IState> states = new List<IState>();
        private readonly List<Transition> transitions = new List<Transition>();
        private IState current;

        public void AddState(IState state)
        {
            if (state == null || states.Contains(state))
                return;

            states.Add(state);
        }

        public void AddTransition(Transition transition)
        {
            if (transitions.Contains(transition))
                return;

            transitions.Add(transition);
        }

        public void Update() => current?.OnFrame();
        public void FixedUpdate() => current?.OnTick();

        public void Open(IState state)
        {
            if (state == null || !states.Contains(state))
                return;

            if (state == current)
                return;

            if (current != null)
            {
                current.OnYield -= Yield;
                current.Exit();
            }

            current = state;
            current.Enter();
            current.OnYield += Yield;

            Debug.Log(current.ToString().ToUpperInvariant());
        }

        private void Yield(IState from)
        {
            foreach (Transition transition in transitions)
            {
                if (transition.from != from)
                    continue;

                Open(transition.to);
                return;
            }
        }

        public void Close()
        {
            if (current == null)
                return;

            current.OnYield -= Yield;
            current.Exit();
            current = null;
        }
    }
}
