using TempleOfDoom.Core.Items;

namespace TempleOfDoom.Core
{
    // Base class voor elk vakje in het level grid.
    public abstract class Tile : IGameObject
    {
        // Er kan een item op liggen (zoals een sleutel).
        public Item? CurrentItem { get; set; }

        public virtual bool Interact(Player player)
        {
            // Als er iets ligt, pakken we dat eerst op of interacteren we ermee.
            if (CurrentItem != null)
            {
                bool shouldRemove = CurrentItem.Interact(player);
                if (shouldRemove)
                {
                    CurrentItem = null; // Item is weg (bijv. opgepakt).
                }
            }
            return false; 
        }

        // Standaard mag je eroverheen lopen, behalve als een subclass anders zegt.
        public virtual bool IsWalkable(Player? player)
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
