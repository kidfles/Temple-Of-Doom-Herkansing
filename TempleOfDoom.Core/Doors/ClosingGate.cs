
namespace TempleOfDoom.Core.Doors
{
    public class ClosingGate : DoorDecorator
    {
        private bool hasClosed;

        public ClosingGate(IDoor door) : base(door)
        {
            hasClosed = false;
        }

        public override void Interact(Player player)
        {
            base.Interact(player);
            //after you enter, the door closes? for now
            hasClosed = true;
        }

        public override bool CanEnter(Player player)
        {
            if (hasClosed) return false;
            return base.CanEnter(player);
        }
    }
}
