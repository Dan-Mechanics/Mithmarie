namespace Mithmarie
{
    public interface IWorldCommand
    {
        void Execute(World world);
        void Undo(World world);
    }
}
