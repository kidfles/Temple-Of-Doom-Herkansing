using System;
using System.IO;
using TempleOfDoom.Core;
using TempleOfDoom.Logic;
using TempleOfDoom.Presentation;

namespace TempleOfDoom
{
    class Program
    {
        static void Main(string[] args)
        {
            string levelFile = "TempleOfDoom_Extended_B_2122.json";
            
            // Validate file existence
            if (!File.Exists(levelFile))
            {
                if (File.Exists("../../../" + levelFile))
                {
                    levelFile = "../../../" + levelFile;
                }
                else
                {
                    Console.WriteLine("Level file not found!");
                    return;
                }
            }

            JsonLevelLoader loader = new JsonLevelLoader();
            Level level = loader.LoadLevel(levelFile);

            if (level.Rooms.Count == 0)
            {
                Console.WriteLine("No rooms loaded!");
                return;
            }
            

            if (level.Player == null)
            {
                Console.WriteLine("Player not found in level!");
                return;
            }

            Player player = level.Player;
            if (level.CurrentRoom == null)
            {
                 level.CurrentRoom = level.Rooms[0];
            }

            GameLoop gameLoop = new GameLoop();
            ConsoleRenderer renderer = new ConsoleRenderer(level);
            gameLoop.RegisterObserver(renderer);

            // Initial Draw
            gameLoop.TriggerGameTick();

            bool running = true;
            while (running)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                int targetX = player.X;
                int targetY = player.Y;

                switch (keyInfo.Key)
                {
                    case ConsoleKey.W: targetY--; break;
                    case ConsoleKey.S: targetY++; break;
                    case ConsoleKey.A: targetX--; break;
                    case ConsoleKey.D: targetX++; break;
                    case ConsoleKey.Escape: running = false; break;
                }

                if (!running) break;

                // Process movement and interaction if target is valid
                if (targetX >= 0 && targetX < level.CurrentRoom.Width &&
                    targetY >= 0 && targetY < level.CurrentRoom.Height)
                {
                    var targetTile = level.CurrentRoom.GetTile(targetX, targetY);
                    if (targetTile.IsWalkable(player))
                    {
                        player.X = targetX;
                        player.Y = targetY;

                        targetTile.Interact(player);

                        gameLoop.CheckRoomSwitch(level, player);

                        // Handle conveyor belt logic
                        var currentTile = level.CurrentRoom.GetTile(player.X, player.Y);
                        if (currentTile is ConveyorBeltTile conveyor)
                        {
                            int slideX = player.X;
                            int slideY = player.Y;
                            
                            switch (conveyor.Direction)
                            {
                                case Direction.NORTH: slideY--; break;
                                case Direction.EAST: slideX++; break;
                                case Direction.SOUTH: slideY++; break;
                                case Direction.WEST: slideX--; break;
                            }
                            
                            if (slideX >= 0 && slideX < level.CurrentRoom.Width &&
                                slideY >= 0 && slideY < level.CurrentRoom.Height)
                            {
                                var slideTile = level.CurrentRoom.GetTile(slideX, slideY);
                                if (slideTile.IsWalkable(player))
                                {
                                    player.X = slideX;
                                    player.Y = slideY;
                                    slideTile.Interact(player); 
                                    gameLoop.CheckRoomSwitch(level, player);
                                }
                            }
                        }
                    }
                }

                gameLoop.TriggerGameTick();
            }
        }
    }
}
