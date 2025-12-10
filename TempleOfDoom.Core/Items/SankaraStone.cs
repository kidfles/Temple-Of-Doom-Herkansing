
namespace TempleOfDoom.Core.Items
{
    public class SankaraStone : Item
    {
        public override bool Interact(Player player)
        {
            // Pickup logic komt nog
            return true;
        }

        public override char GetSprite()
        {
            return 'S';
        }
    }
}
