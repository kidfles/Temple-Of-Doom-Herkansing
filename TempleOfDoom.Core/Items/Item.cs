
namespace TempleOfDoom.Core.Items
{
    public abstract class Item : IGameObject
    {
        public abstract void Interact(Player player);
        
        public virtual bool IsWalkable()
        {
            return true;
        }

        public abstract char GetSprite();
    }
}
