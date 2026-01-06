using TempleOfDoom.Core;
using CODE_TempleOfDoom_DownloadableContent;

namespace TempleOfDoom.Logic.Enemies
{
    // Adapter Pattern: Maakt van een 'DLL Enemy' een 'IEnemy' die ons spel begrijpt.
    public class EnemyAdapter : IEnemy
    {
        private Enemy _dllEnemy; // Het externe object dat we wrappen (de Adaptee).

        public EnemyAdapter(Enemy dllEnemy)
        {
            _dllEnemy = dllEnemy;
        }

        // Bridge Properties: Mapt onze properties naar die van de DLL.
        public int X => _dllEnemy.CurrentXLocation;
        public int Y => _dllEnemy.CurrentYLocation;
        public bool IsDead => _dllEnemy.NumberOfLives <= 0;

        public void OnGameTick()
        {
            if (!IsDead)
            {
                _dllEnemy.Move(); // Vertaal 'GameTick' naar 'Move'.
            }
        }

        public void TakeDamage(int amount)
        {
            _dllEnemy.DoDamage(amount); // Vertaal 'TakeDamage' naar 'DoDamage'.
        }
    }
}
