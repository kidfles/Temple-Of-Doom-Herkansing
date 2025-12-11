using TempleOfDoom.Core;
using CODE_TempleOfDoom_DownloadableContent;

namespace TempleOfDoom.Logic.Enemies
{
    public class EnemyAdapter : IEnemy
    {
        private Enemy _dllEnemy;

        public EnemyAdapter(Enemy dllEnemy)
        {
            _dllEnemy = dllEnemy;
        }

        // Bridge Properties
        public int X => _dllEnemy.CurrentXLocation;
        public int Y => _dllEnemy.CurrentYLocation;
        public bool IsDead => _dllEnemy.NumberOfLives <= 0;

        public void OnGameTick()
        {
            if (!IsDead)
            {
                _dllEnemy.Move();
            }
        }

        public void TakeDamage(int amount)
        {
            _dllEnemy.DoDamage(amount);
        }
    }
}
