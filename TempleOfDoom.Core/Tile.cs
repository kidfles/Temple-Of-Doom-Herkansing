using TempleOfDoom.Core.Items;

namespace TempleOfDoom.Core
{
    public abstract class Tile : IGameObject
    {
        public Item CurrentItem { get; set; }

        public virtual void Interact(Player player)
        {
            if (CurrentItem != null)
            {
                CurrentItem.Interact(player);
                CurrentItem = null; // Remove item after interaction (e.g. pickup)
            }
        }
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
