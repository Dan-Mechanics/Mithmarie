using System;
using UnityEngine;

namespace Mitholca
{
    public class Transition 
    {
        public State from;
        public State to;
        public Func<bool> goNext;

        public Transition(State from, State to, Func<bool> shouldTransition)
        {
            this.from = from;
            this.to = to;
            this.goNext = shouldTransition;
        }
    }
}
