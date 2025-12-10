
namespace TempleOfDoom.Core
{
    public abstract class Tile : IGameObject
    {
        public abstract void Interact(Player player);
        public abstract bool IsWalkable();
        public abstract char GetSprite();
    }
}
