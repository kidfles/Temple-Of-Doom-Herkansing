namespace TempleOfDoom.Core
{
    public interface IEnemy : IGameObserver
    {
        int X { get; }
        int Y { get; }
        void TakeDamage(int amount);
        bool IsDead { get; }
    }
}
