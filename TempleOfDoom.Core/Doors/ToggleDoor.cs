
namespace TempleOfDoom.Core.Doors
{
    public class ToggleDoor : DoorDecorator
    {
        //For now 0 logic, but that will come

        public ToggleDoor(IDoor door) : base(door)
        {
        }

        public override bool CanEnter(Player? player)
        {
            
            return base.CanEnter(player);
        }
    }
}
