using UnityEngine;
using UnityEngine.InputSystem;

namespace Mithmarie
{
    public class GameManager : MonoBehaviour
    {
        public const Key TOGGLE_STATE_KEY = Key.Escape;
        private readonly FSM fsm = new FSM();

        private void Start()
        {
            IMessageService message = ServiceLocator<IMessageService>.Locate();
            message.Send("[WASD] for movement and [MOUSE] for looking.\nUse [RMB] to place blocks, [LMB] to destroy.", Color.black);

            World world = FindAnyObjectByType<World>();
            if (world == null)
                Debug.Log("problem");

            world.Add(Vector3Int.zero);
            world.Flush();

            Menu menu = FindAnyObjectByType<Menu>();
            menu.Setup(world, new Wavefront(), new CulledMeshGenerator(), message);


            Player playerState = FindAnyObjectByType<Player>();
            Menu menuState = FindAnyObjectByType<Menu>();

            fsm.AddState(playerState);
            fsm.AddState(menuState);
            fsm.AddTransition(new Transition(playerState, menuState, playerState.GetWantsToClose));
            fsm.AddTransition(new Transition(menuState, playerState, menuState.GetWantsToClose));

            menuState.Exit();
            fsm.Open(playerState);
        }

        private void Update() => fsm.Update();
        private void FixedUpdate() => fsm.FixedUpdate();
    }
}