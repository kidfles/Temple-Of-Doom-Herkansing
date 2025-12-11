using TempleOfDoom.Core.Items;

namespace TempleOfDoom.Core
{
    public abstract class Tile : IGameObject
    {
        public Item? CurrentItem { get; set; }

        public virtual bool Interact(Player player)
        {
            if (CurrentItem != null)
            {
                bool shouldRemove = CurrentItem.Interact(player);
                if (shouldRemove)
                {
                    CurrentItem = null; 
                }
            }
            return false; 
        }
        public virtual bool IsWalkable(Player player)
        {
             return true; 
        }
        
        
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
