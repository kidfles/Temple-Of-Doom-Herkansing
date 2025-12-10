
namespace TempleOfDoom.Core
{
    public class FloorTile : Tile
    {


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
