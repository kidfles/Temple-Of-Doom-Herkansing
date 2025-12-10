using System;
using System.Collections.Generic;
using TempleOfDoom.Core;

namespace TempleOfDoom.Logic
{
    public class GameLoop
    {
        private List<IGameObserver> observers;
        private Level level;
        
        public DateTime StartTime { get; private set; }
        public TimeSpan ElapsedTime => DateTime.Now - StartTime;
        public bool IsGameOver { get; private set; }
        public bool HasWon { get; private set; }

        public GameLoop(Level level)
        {
            this.level = level;
            observers = new List<IGameObserver>();
            StartTime = DateTime.Now;
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
            if (level.Player.Lives <= 0)
            {
                IsGameOver = true;
            }
            else if (level.Player.StonesCollected >= level.TotalStones && level.TotalStones > 0)
            {
                HasWon = true;
                IsGameOver = true;
            }

            foreach (var observer in observers)
            {
                observer.OnGameTick();
            }
        }

        public void HandleInput(ConsoleKey key)
        {
            if (IsGameOver) return;

            Player player = level.Player;
            int targetX = player.X;
            int targetY = player.Y;

            switch (key)
            {
                case ConsoleKey.W: targetY--; break;
                case ConsoleKey.S: targetY++; break;
                case ConsoleKey.A: targetX--; break;
                case ConsoleKey.D: targetX++; break;
                default: return; // Ignore other keys
            }

            ProcessMove(player, targetX, targetY);
            TriggerGameTick();
        }

        private void ProcessMove(Player player, int targetX, int targetY)
        {
            if (targetX >= 0 && targetX < level.CurrentRoom.Width &&
                targetY >= 0 && targetY < level.CurrentRoom.Height)
            {
                var targetTile = level.CurrentRoom.GetTile(targetX, targetY);
                if (targetTile.IsWalkable(player))
                {
                    player.X = targetX;
                    player.Y = targetY;

                    targetTile.Interact(player);

                    CheckRoomSwitch(level, player);

                    var currentTile = level.CurrentRoom.GetTile(player.X, player.Y);
                    if (currentTile is ConveyorBeltTile conveyor)
                    {
                        int slideX = player.X;
                        int slideY = player.Y;
                        
                        var vector = conveyor.GetTransportVector();
                        slideX += vector.x;
                        slideY += vector.y;
                        
                        if (slideX >= 0 && slideX < level.CurrentRoom.Width &&
                            slideY >= 0 && slideY < level.CurrentRoom.Height)
                        {
                            var slideTile = level.CurrentRoom.GetTile(slideX, slideY);
                            if (slideTile.IsWalkable(player))
                            {
                                player.X = slideX;
                                player.Y = slideY;
                                slideTile.Interact(player); 
                                CheckRoomSwitch(level, player);
                            }
                        }
                    }
                }
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
                }
            }
        }
    }
}
