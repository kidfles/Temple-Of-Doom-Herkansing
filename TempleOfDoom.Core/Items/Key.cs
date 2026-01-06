
namespace TempleOfDoom.Core.Items
{
    // Een sleutel om gekleurde deuren mee te openen.
    public class Key : Item
    {
        public string Color { get; set; } = "unknown";

        // Als je erop gaat staan, pak je hem op.
        public override bool Interact(Player player)
        {
            player.Inventory.Add(this);
            return true; // item is opgepakt, verwijder van tile.
        }

        public override char GetSprite()
        {
            return 'k';
        }
    }
}
