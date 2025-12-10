
namespace TempleOfDoom.Core
{
    public class WallTile : Tile
    {
        public override bool Interact(Player player)
        {
            // Wall interactions (e.g. bumping into it)
            return false;
        }

        public override bool IsWalkable(Player player)
        {
            return false;
        }

        protected override char GetBaseSprite()
        {
            return '#';
        }
    }
}
