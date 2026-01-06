
namespace TempleOfDoom.Core.Doors
{
    // Decorator Pattern: Een deur die voor altijd sluit nadat je er 1 keer doorheen bent.
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
            hasClosed = true;
        }

        public override bool CanEnter(Player? player)
        {
            if (hasClosed) return false;
            return base.CanEnter(player);
        }
    }
}
