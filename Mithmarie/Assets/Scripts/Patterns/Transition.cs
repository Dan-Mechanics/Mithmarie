using System;

namespace Mithmarie
{
    public struct Transition 
    {
        public IState from;
        public IState to;

        public Transition(IState from, IState to)
        {
            if (to == from)
                throw new Exception("to == from. This is not allowed.");

            this.from = from;
            this.to = to;
        }
    }
}
