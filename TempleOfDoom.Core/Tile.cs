using TempleOfDoom.Core.Items;

namespace TempleOfDoom.Core
{
    public abstract class Tile : IGameObject
    {
        public Item CurrentItem { get; set; }

        public virtual bool Interact(Player player)
        {
            if (CurrentItem != null)
            {
                bool shouldRemove = CurrentItem.Interact(player);
                if (shouldRemove)
                {
                    CurrentItem = null; // Remove item only if requested
                }
            }
            return false; // Tiles themselves are typically not removed via interaction
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
