
namespace TempleOfDoom.Core.Items
{
    public class SankaraStone : Item
    {
        public override bool Interact(Player player)
        {
            player.StonesCollected++;
            return true;
        }

        public override char GetSprite()
        {
            return 'S';
        }
    }
}
