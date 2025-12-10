using TempleOfDoom.Core.Items;

namespace TempleOfDoom.Core
{
    public abstract class Tile : IGameObject
    {
        public Item CurrentItem { get; set; }

        public abstract void Interact(Player player);
        public abstract bool IsWalkable();
        
        
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
