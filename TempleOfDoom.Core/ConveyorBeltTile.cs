
namespace TempleOfDoom.Core
{
    public class ConveyorBeltTile : Tile
    {
        public Direction Direction { get; set; }

        public override void Interact(Player player)
        {
            base.Interact(player);
            switch (Direction)
            {
                case Direction.NORTH:
                    player.Y--;
                    break;
                case Direction.EAST:
                    player.X++;
                    break;
                case Direction.SOUTH:
                    player.Y++;
                    break;
                case Direction.WEST:
                    player.X--;
                    break;
            }
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
