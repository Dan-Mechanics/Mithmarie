namespace Mitholca
{
    public interface IState 
    {
        void Enter();
        void Exit();
        void OnFrame();
        void OnTick();
    }
}
