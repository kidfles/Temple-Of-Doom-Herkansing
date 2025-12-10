
namespace TempleOfDoom.Core.Doors
{
    public class BasicDoor : IDoor
    {
        public bool IsOpen { get; private set; }

        public BasicDoor()
        {
            IsOpen = true; // Default to open
        }

        public void Interact(Player player)
        {
            //basicdoor does nothing, but this function 
        }

        public bool CanEnter(Player player)
        {
            return IsOpen;
        }

        public char GetSprite()
        {
            return IsOpen ? '|' : '+'; 
        }
    }
}
