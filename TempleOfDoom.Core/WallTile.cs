
namespace TempleOfDoom.Core
{
    public class WallTile : Tile
    {
        public override void Interact(Player player)
        {
            // Wall interactions (e.g. bumping into it)
        }

        public override bool IsWalkable()
        {
            return false;
        }

        protected override char GetBaseSprite()
        {
            return '#';
        }
    }
}
