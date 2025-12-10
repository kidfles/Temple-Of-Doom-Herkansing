
namespace TempleOfDoom.Core
{
    public class ConveyorBeltTile : Tile
    {
        public Direction Direction { get; set; }

        public override bool Interact(Player player)
        {
            return base.Interact(player);
        }

        public override bool IsWalkable(Player player)
        {
            return true; 
        }

        protected override char GetBaseSprite()
        {
            return 'C'; 
        }
    }
}
