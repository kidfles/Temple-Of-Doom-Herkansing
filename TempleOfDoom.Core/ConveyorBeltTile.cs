
namespace TempleOfDoom.Core
{
    public class ConveyorBeltTile : Tile
    {
        public string Direction { get; set; }

        public override void Interact(Player player)
        {
            // Logic for moving player in Direction will be implemented later
        }

        public override bool IsWalkable()
        {
            return true; 
        }

        public override char GetSprite()
        {
            return 'C'; // Placeholder sprite
        }
    }
}
