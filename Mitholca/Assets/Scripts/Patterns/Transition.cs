using System;
using UnityEngine;

namespace Mitholca
{
    public class Transition 
    {
        public IState from;
        public IState to;
        public Func<bool> goNext;

        public Transition(IState from, IState to, Func<bool> goNext)
        {
            this.from = from;
            this.to = to;
            this.goNext = goNext;
        }
    }
}
