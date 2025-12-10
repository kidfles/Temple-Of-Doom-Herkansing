namespace TempleOfDoom.Core
{
    public interface IGameObject
    {
        void Interact(Player player);
        bool IsWalkable(Player player);
        char GetSprite();
    }
}
