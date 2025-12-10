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

        public void CheckRoomSwitch(Level level, Player player)
        {
            if (player.CurrentRoomId != level.CurrentRoom.Id)
            {
                var newRoom = level.Rooms.Find(r => r.Id == player.CurrentRoomId);
                if (newRoom != null)
                {
                    level.CurrentRoom = newRoom;
                    TriggerGameTick();
                }
            }
        }
    }
}
