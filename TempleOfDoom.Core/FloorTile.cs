
namespace TempleOfDoom.Core
{
    // De standaard vloer.
    public class FloorTile : Tile
    {
        // Altijd begaanbaar.
        public override bool IsWalkable(Player player)
        {
            return true;
        }

        protected override char GetBaseSprite()
        {
            return '.';
        }
    }
}
