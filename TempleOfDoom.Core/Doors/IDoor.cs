
namespace TempleOfDoom.Core.Doors
{
    // Decorator Pattern: De gedeelde interface voor zowel de simpele deur als alle decorators eromheen.
    public interface IDoor
    {
        // Doe iets met de deur (bijv. toggle open/dicht).
        void Interact(Player player);
        
        // Mag de speler erdoor?
        bool CanEnter(Player? player);

        // Hoe ziet de deur eruit?
        char GetSprite();
    }
}
