namespace TempleOfDoom.Core
{
    // Observer Pattern: Een vijand is ook een observer, want die moet elke tick bewegen/denken.
    public interface IEnemy : IGameObserver
    {
        int X { get; }
        int Y { get; }

        void TakeDamage(int amount);

        bool IsDead { get; }
    }
}
