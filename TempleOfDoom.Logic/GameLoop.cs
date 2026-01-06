using System;
using System.Collections.Generic;
using TempleOfDoom.Core;

namespace TempleOfDoom.Logic
{
    // Observer Pattern: Dit is het 'Subject'. Het beheert de state van de game en stuurt updates.
    public class GameLoop
    {
        private List<IGameObserver> observers; // Lijst met iedereen die luistert (UI, vijanden).
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

        // Observer Pattern: Nieuwe luisteraar toevoegen.
        public void RegisterObserver(IGameObserver observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
        }

        // Observer Pattern: Luisteraar verwijderen.
        public void UnregisterObserver(IGameObserver observer)
        {
            if (observers.Contains(observer))
            {
                observers.Remove(observer);
            }
        }

        // Observer Pattern: De 'Notify' methode. Roept OnGameTick aan op alle observers.
        public void TriggerGameTick()
        {
            if (level.Player == null) return;

            // Check win/verlies condities.
            if (level.Player.Lives <= 0)
            {
                IsGameOver = true;
            }
            else if (level.Player.StonesCollected >= level.TotalStones && level.TotalStones > 0)
            {
                HasWon = true;
                IsGameOver = true;
            }

            // Vertel iedereen dat er een tick is geweest.
            foreach (var observer in observers)
            {
                observer.OnGameTick();
            }

            if (level.CurrentRoom != null)
            {
                // Vijanden zijn ook observers (soort van), trigger hun logica.
                foreach (var enemy in level.CurrentRoom.Enemies)
                {
                    enemy.OnGameTick();
                }
                CheckEnemyCollisions(level.Player);
            }
        }

        // Check of de speler tegen een vijand aanloopt.
        private void CheckEnemyCollisions(Player player)
        {
            if (level.CurrentRoom == null) return;
            
            // Simpele check: als X en Y gelijk zijn, is er botsing.
            var hittingEnemy = level.CurrentRoom.Enemies.Find(e => e.X == player.X && e.Y == player.Y);
            if (hittingEnemy != null)
            {
                player.Lives--; // Au! Leven eraf.
                if (player.Lives <= 0)
                {
                    IsGameOver = true;
                }
            }
        }

        // Verwerk de toetsaanslagen van de gebruiker.
        public void HandleInput(ConsoleKey key)
        {
            if (IsGameOver) return; 

            if (level.Player == null) return;
            Player player = level.Player;
            int targetX = player.X;
            int targetY = player.Y;

            // Bepaal de nieuwe positie op basis van WASD.
            switch (key)
            {
                case ConsoleKey.W: targetY--; break;
                case ConsoleKey.S: targetY++; break;
                case ConsoleKey.A: targetX--; break;
                case ConsoleKey.D: targetX++; break;
                default: return; 
            }

            // Probeer te bewegen en update daarna de game state.
            ProcessMove(player, targetX, targetY);
            TriggerGameTick();
        }

        // De kern logica voor bewegen: mag ik hierheen? Is er interactie?
        private void ProcessMove(Player player, int targetX, int targetY)
        {
            var room = level.CurrentRoom;
            if (room == null) return;

            // Check of we binnen de muren blijven.
            if (targetX >= 0 && targetX < room.Width &&
                targetY >= 0 && targetY < room.Height)
            {
                var targetTile = room.GetTile(targetX, targetY);
                // Mag ik op deze tegel staan?
                if (targetTile != null && targetTile.IsWalkable(player))
                {
                    player.X = targetX;
                    player.Y = targetY;

                    // Interactie met de tegel (bijv. item oppakken).
                    targetTile.Interact(player);
                    
                    CheckEnemyCollisions(player);

                    CheckRoomSwitch(level, player);

                    // Module B: Hier checken we of je op een transportband staat.
                    var currentTile = room.GetTile(player.X, player.Y);
                    if (currentTile is ConveyorBeltTile conveyor)
                    {
                        int slideX = player.X;
                        int slideY = player.Y;
                        
                        var vector = conveyor.GetTransportVector();
                        slideX += vector.x;
                        slideY += vector.y;
                        
                        // Check of we niet de map af glijden.
                        if (slideX >= 0 && slideX < room.Width &&
                            slideY >= 0 && slideY < room.Height)
                        {
                            var slideTile = room.GetTile(slideX, slideY);
                            // Alleen glijden als de volgende tegel ook walkable is.
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

        // Helper functie om te kijken of de speler naar een andere kamer is geteleporteerd.
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
