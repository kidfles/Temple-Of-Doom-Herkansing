
namespace TempleOfDoom.Core.Doors
{
    // Decorator Pattern: Dit is de abstracte decorator. Hij geeft alle calls door aan de 'wrappedDoor'.
    // Subclasses kunnen hier hun eigen gedrag aan toevoegen.
    public abstract class DoorDecorator : IDoor
    {
        protected IDoor wrappedDoor;

        public DoorDecorator(IDoor door)
        {
            wrappedDoor = door;
        }

        public virtual void Interact(Player player)
        {
            wrappedDoor.Interact(player);
        }

        public virtual bool CanEnter(Player? player)
        {
            return wrappedDoor.CanEnter(player);
        }

        public virtual char GetSprite()
        {
            return wrappedDoor.GetSprite();
        }
    }
}
