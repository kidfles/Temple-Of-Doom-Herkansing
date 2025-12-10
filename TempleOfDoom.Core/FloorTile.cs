
namespace TempleOfDoom.Core
{
    public class FloorTile : Tile
    {
        public override void Interact(Player player)
        {
            // Floor interactions (if any)
        }

        public override bool IsWalkable()
        {
            return true;
        }

        protected override char GetBaseSprite()
        {
            return '.';
        }
    }
}
