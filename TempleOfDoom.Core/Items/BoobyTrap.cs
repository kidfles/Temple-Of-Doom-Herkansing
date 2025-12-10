
namespace TempleOfDoom.Core.Items
{
    public class BoobyTrap : Item
    {
        public int Damage { get; set; }

        public override bool Interact(Player player)
        {
            player.Lives -= Damage;
            return false;
        }

        public override char GetSprite()
        {
            return 'X';
        }
    }
}
