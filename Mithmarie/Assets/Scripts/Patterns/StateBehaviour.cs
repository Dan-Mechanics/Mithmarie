using UnityEngine;

namespace Mithmarie
{
    public class StateBehaviour : MonoBehaviour, IState
    {
        public virtual void Enter()
        {
            print($"Entering {gameObject.name} state ...");
        }

        public virtual void Exit() { }
        public virtual void OnFrame() { }
        public virtual void OnTick() { }
        public virtual void OnDestroy() => Exit();
    }
}
