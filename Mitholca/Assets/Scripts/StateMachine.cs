using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mitholca
{
    public class StateMachine : MonoBehaviour
    {
        private readonly FSM fsm = new FSM();

        /// <summary>
        ///  Or we make the player a state beahviour so you can assign all the shit.
        /// </summary>
        public class Player : State
        {
            private readonly List<IState> behaviour;

            public override void OnFrame()
            {
                base.OnFrame();
                behaviour.ForEach(x => x.OnFrame());
            }

            // this class then has references to the player baisclaly.
            // because the mouse and stuff is a state too.
            // ..
        }

        private void Start()
        {
            Player editingState = new Player();
            fsm.AddState(editingState);
            fsm.Enter(editingState);
            
            // fsm.AddState();
            // fsm.AddState();
            // fsm.AddState();
            // fsm.AddState();
            // fsm.AddTransition();
            // fsm.AddTransition();
            // fsm.AddTransition();
            // fsm.AddTransition();
            

        }

        private void Update() => fsm.OnFrame();
        private void FixedUpdate() => fsm.OnTick();

    }
}