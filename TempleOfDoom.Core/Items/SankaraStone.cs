
namespace TempleOfDoom.Core.Items
{
    // Het doel van het spel: verzamel deze stenen!
    public class SankaraStone : Item
    {
        public override bool Interact(Player player)
        {
            player.StonesCollected++; // Teller omhoog.
            return true; // Steen is opgepakt, dus weg van het bord.
        }

        public override char GetSprite()
        {
            return 'S';
        }
    }
}
