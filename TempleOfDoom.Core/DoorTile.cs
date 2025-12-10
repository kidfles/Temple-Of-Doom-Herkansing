using TempleOfDoom.Core.Doors;

namespace TempleOfDoom.Core
{
    public class DoorTile : Tile
    {
        private IDoor _doorLogic;

        public DoorTile(IDoor doorLogic)
        {
            _doorLogic = doorLogic;
        }

        public override void Interact(Player player)
        {
            base.Interact(player);
            _doorLogic.Interact(player);
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
