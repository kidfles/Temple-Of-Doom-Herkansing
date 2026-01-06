
namespace TempleOfDoom.Core.Doors
{
    // Decorator Pattern: Deze deur gaat steeds open en dicht (toggle).
    public class ToggleDoor : DoorDecorator
    {

        public ToggleDoor(IDoor door) : base(door)
        {
        }

        public override bool CanEnter(Player? player)
        {
            
            return base.CanEnter(player);
        }
    }
}
