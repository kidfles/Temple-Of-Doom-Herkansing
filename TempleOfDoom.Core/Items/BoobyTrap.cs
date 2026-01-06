
namespace TempleOfDoom.Core.Items
{
    // Doet pijn als je erop stapt.
    public class BoobyTrap : Item
    {
        public int Damage { get; set; }

        // Interactie zorgt voor schade aan de speler.
        public override bool Interact(Player player)
        {
            player.Lives -= Damage;
            return false; // false betekent: item blijft liggen (je kan er nog eens in trappen).
        }

        public override char GetSprite()
        {
            return 'X';
        }
    }
}
