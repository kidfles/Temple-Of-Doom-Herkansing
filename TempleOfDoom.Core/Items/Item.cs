
namespace TempleOfDoom.Core.Items
{
    // Base class voor alle spullen.
    public abstract class Item : IGameObject
    {
        // Wat gebeurt er.
        public abstract bool Interact(Player player);
        
        // Items blokkeren normaal gesproken niet, je kan er gewoon doorheen lopen.
        public virtual bool IsWalkable(Player player)
        {
            return true;
        }

        // Elk item moet zijn eigen plaatje hebben.
        public abstract char GetSprite();
    }
}
