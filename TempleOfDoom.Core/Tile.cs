using TempleOfDoom.Core.Items;

namespace TempleOfDoom.Core
{
    public abstract class Tile : IGameObject
    {
        public Item CurrentItem { get; set; }

        public abstract void Interact(Player player);
        public virtual bool IsWalkable(Player player)
        {
             return true; 
        }
        // Making it virtual with default true is safer/easier, or keep abstract. 
        // Let's keep abstract to force implementation consideration, but Abstract methods match interface.
        // Wait, IsWalkable is on IGameObject too.
        
        
        public char GetSprite()
        {
            if (CurrentItem != null)
            {
                return CurrentItem.GetSprite();
            }
            return GetBaseSprite();
        }

        protected abstract char GetBaseSprite();
    }
}
