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
            if (level.Player == null) return;

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

            if (level.CurrentRoom != null)
            {
                foreach (var enemy in level.CurrentRoom.Enemies)
                {
                    enemy.OnGameTick();
                }
                CheckEnemyCollisions(level.Player);
            }
        }

        private void CheckEnemyCollisions(Player player)
        {
            if (level.CurrentRoom == null) return;
            
            var hittingEnemy = level.CurrentRoom.Enemies.Find(e => e.X == player.X && e.Y == player.Y);
            if (hittingEnemy != null)
            {
                player.Lives--;
                if (player.Lives <= 0)
                {
                    IsGameOver = true;
                }
            }
        }

        public void HandleInput(ConsoleKey key)
        {
            if (IsGameOver) return;

            if (level.Player == null) return;
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
            var room = level.CurrentRoom;
            if (room == null) return;

            if (targetX >= 0 && targetX < room.Width &&
                targetY >= 0 && targetY < room.Height)
            {
                var targetTile = room.GetTile(targetX, targetY);
                if (targetTile != null && targetTile.IsWalkable(player))
                {
                    player.X = targetX;
                    player.Y = targetY;

                    targetTile.Interact(player);
                    
                    CheckEnemyCollisions(player);

                    CheckRoomSwitch(level, player);

                    var currentTile = room.GetTile(player.X, player.Y);
                    if (currentTile is ConveyorBeltTile conveyor)
                    {
                        int slideX = player.X;
                        int slideY = player.Y;
                        
                        var vector = conveyor.GetTransportVector();
                        slideX += vector.x;
                        slideY += vector.y;
                        
                        if (slideX >= 0 && slideX < room.Width &&
                            slideY >= 0 && slideY < room.Height)
                        {
                            var slideTile = room.GetTile(slideX, slideY);
                            if (slideTile != null && slideTile.IsWalkable(player))
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
            if (level.CurrentRoom != null && player.CurrentRoomId != level.CurrentRoom.Id)
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
