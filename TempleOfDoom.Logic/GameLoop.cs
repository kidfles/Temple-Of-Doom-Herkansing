using System.Collections.Generic;
using TempleOfDoom.Core;

namespace TempleOfDoom.Logic
{
    public class GameLoop
    {
        private List<IGameObserver> observers;

        public GameLoop()
        {
            observers = new List<IGameObserver>();
        }

        public void RegisterObserver(IGameObserver observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
        }

        public void UnregisterObserver(IGameObserver observer)
        {
            if (observers.Contains(observer))
            {
                observers.Remove(observer);
            }
        }

        public void TriggerGameTick()
        {
            foreach (var observer in observers)
            {
                observer.OnGameTick();
            }
        }
    }
}
