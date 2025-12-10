
namespace TempleOfDoom.Core.Items
{
    public abstract class Item : IGameObject
    {
        public abstract bool Interact(Player player);
        
        public virtual bool IsWalkable(Player player)
        {
            return true;
        }

        public abstract char GetSprite();
    }
}
