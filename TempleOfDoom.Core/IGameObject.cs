namespace TempleOfDoom.Core
{
    // Dit zorgt dat alle objecten in het spel (tegels, items) zich op dezelfde basismanier gedragen.
    public interface IGameObject
    {
        // Wat gebeurd er als de speler ermee interact?
        bool Interact(Player player);

        // Mag de speler (of een ander object) hierop staan?
        bool IsWalkable(Player? player);

        // Welk tekentje moeten we op het scherm zetten?
        char GetSprite();
    }
}
