
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

        public (int x, int y) GetTransportVector()
        {
            return Direction switch
            {
                Direction.NORTH => (0, -1),
                Direction.EAST => (1, 0),
                Direction.SOUTH => (0, 1),
                Direction.WEST => (-1, 0),
                _ => (0, 0)
            };
        }
    }
}
