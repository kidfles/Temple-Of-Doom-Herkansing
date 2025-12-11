
namespace TempleOfDoom.Core.Doors
{
    public interface IDoor
    {
        void Interact(Player player);
        bool CanEnter(Player? player);
        char GetSprite();
    }
}
