namespace TempleOfDoom.Core
{
    public interface IGameObject
    {
        bool Interact(Player player);
        bool IsWalkable(Player player);
        char GetSprite();
    }
}
