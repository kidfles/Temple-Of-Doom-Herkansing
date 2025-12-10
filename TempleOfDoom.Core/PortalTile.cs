
namespace TempleOfDoom.Core
{
    public class PortalTile : Tile
    {
        public int TargetRoomId { get; set; }
        public int TargetX { get; set; }
        public int TargetY { get; set; }

        public override void Interact(Player player)
        {
            player.X = TargetX;
            player.Y = TargetY;
            player.CurrentRoomId = TargetRoomId;
        }

        public override bool IsWalkable()
        {
            return true;
        }

        protected override char GetBaseSprite()
        {
            return 'O'; 
        }
    }
}
