
namespace TempleOfDoom.Core.Items
{
    public class Key : Item
    {
        public string Color { get; set; }

        public override void Interact(Player player)
        {
            // Pickup logic komt nog
        }

        public override char GetSprite()
        {
            return 'k';
        }
    }
}
