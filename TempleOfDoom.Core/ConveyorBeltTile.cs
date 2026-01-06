
namespace TempleOfDoom.Core
{
    // Module B: Een lopende band die de speler automatisch verplaatst.
    public class ConveyorBeltTile : Tile
    {
        public Direction Direction { get; set; }

        public override bool Interact(Player player)
        {
            // Eerst kijken of er item interactie is.
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

        // Module B: Berekenen in welke richting we de speler duwen.
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
