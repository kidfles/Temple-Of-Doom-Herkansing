
namespace TempleOfDoom.Core
{
    // Observer Pattern: Dit is de 'subscriber'. 
    public interface IGameObserver
    {
        // Dit wordt aangeroepen door de GameLoop als er weer een tick voorbij is.
        void OnGameTick();
    }
}
