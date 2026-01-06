
namespace TempleOfDoom.Core.Doors
{
    // De simpelste vorm van een deur. 'Component' in het Decorator Pattern.
    public class BasicDoor : IDoor
    {
        public bool IsOpen { get; private set; }

        public BasicDoor()
        {
            IsOpen = true; // Standaard open.
        }

        public void Interact(Player player)
        {
            // Een gewone deur doet niks speciaals bij interactie (hij is gewoon open).
        }

        public bool CanEnter(Player? player)
        {
            return IsOpen;
        }

        public char GetSprite()
        {
            return IsOpen ? '|' : '+'; 
        }
    }
}
