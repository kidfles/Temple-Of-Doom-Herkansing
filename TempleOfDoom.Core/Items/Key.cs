
namespace TempleOfDoom.Core.Items
{
    public class Key : Item
    {
        public string Color { get; set; } = "unknown";

        public override bool Interact(Player player)
        {
            player.Inventory.Add(this);
            return true;
        }

        public override char GetSprite()
        {
            return 'k';
        }
    }
}
