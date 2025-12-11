
namespace TempleOfDoom.Core.Doors
{
    public class ColoredDoor : DoorDecorator
    {
        private string requiredColor;

        public ColoredDoor(IDoor door, string color) : base(door)
        {
            requiredColor = color;
        }

        public override bool CanEnter(Player player)
        {
            if (player == null) return false;
            if (player.HasKey(requiredColor))
            {
                return base.CanEnter(player);
            }
            return false;
        }

        public override char GetSprite()
        {
            return 'D'; 
        }
    }
}
