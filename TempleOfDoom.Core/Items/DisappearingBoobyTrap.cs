
namespace TempleOfDoom.Core.Items
{
    // Zelfde valstrik, maar gaat nadat je erin bent getrapt.
    public class DisappearingBoobyTrap : BoobyTrap
    {
        public override bool Interact(Player player)
        {
            base.Interact(player); // Doe eerst damage (van de base class).
            return true; // true betekent: verwijder dit item nu van het bord.
        }
    }
}
