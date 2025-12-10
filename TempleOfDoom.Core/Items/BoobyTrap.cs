
namespace TempleOfDoom.Core.Items
{
    public class BoobyTrap : Item
    {
        public int Damage { get; set; }

        public override bool Interact(Player player)
        {
            // Damage logic maar nog zonder de logic
            return false;
        }

        public override char GetSprite()
        {
            return 'X';
        }
    }
}
