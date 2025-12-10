
namespace TempleOfDoom.Core
{
    public class PortalTile : Tile
    {
        public int TargetRoomId { get; set; }
        public int TargetX { get; set; }
        public int TargetY { get; set; }

        public override void Interact(Player player)
        {
            base.Interact(player);
            player.X = TargetX;
            player.Y = TargetY;
            player.CurrentRoomId = TargetRoomId;
        }

        public override bool IsWalkable(Player player)
        {
            return true;
        }

        protected override char GetBaseSprite()
        {
            return 'O'; 
        }
    }
}
