
namespace TempleOfDoom.Core.Items
{
    public class DisappearingBoobyTrap : BoobyTrap
    {
        public override bool Interact(Player player)
        {
            base.Interact(player); // Apply damage
            return true; // Remove from tile
        }
    }
}
