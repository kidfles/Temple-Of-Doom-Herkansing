using TempleOfDoom.Core.Doors;

namespace TempleOfDoom.Core
{
    public class DoorTile : Tile
    {
        private IDoor _doorLogic;

        public int TargetRoomId { get; set; }
        public int TargetX { get; set; }
        public int TargetY { get; set; }

        public DoorTile(IDoor doorLogic)
        {
            _doorLogic = doorLogic;
        }

        public override void Interact(Player player)
        {
            base.Interact(player);
            _doorLogic.Interact(player);
             if (TargetRoomId != 0) 
             {
                 player.CurrentRoomId = TargetRoomId;
                 player.X = TargetX;
                 player.Y = TargetY;
             }
        }

        public override bool IsWalkable(Player player)
        {
            return _doorLogic.CanEnter(player);
        }

        protected override char GetBaseSprite()
        {
            return _doorLogic.GetSprite();
        }
    }
}
