
namespace TempleOfDoom.Core.Items
{
    public class Key : Item
    {
        public string Color { get; set; }

        public override bool Interact(Player player)
        {
            // Pickup logic komt nog
            return true;
        }

        public override char GetSprite()
        {
            return 'k';
        }
    }
}
