
namespace TempleOfDoom.Core.Items
{
    public class BoobyTrap : Item
    {
        public int Damage { get; set; }

        public override void Interact(Player player)
        {
            // Damage logic maar nog zonder de logic
        }

        public override char GetSprite()
        {
            return 'X';
        }
    }
}
