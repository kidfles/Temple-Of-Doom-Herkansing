
namespace TempleOfDoom.Core
{
    // Gewoon een muur.
    public class WallTile : Tile
    {
        // Je kan niet interacteren met een muur (behalve ertegenaan lopen).
        public override bool Interact(Player player)
        {
            return false;
        }

        // Je kan er niet doorheen.
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
