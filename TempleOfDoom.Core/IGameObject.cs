namespace TempleOfDoom.Core
{
    public interface IGameObject
    {
        void Interact(Player player);
        bool IsWalkable();
        char GetSprite();
    }
}
