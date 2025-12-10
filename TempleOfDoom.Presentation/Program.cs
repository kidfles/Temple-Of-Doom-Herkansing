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
            

            if (!File.Exists(levelFile))
            {
                 string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                 string localPath = Path.Combine(baseDir, levelFile);
                 
                 if (File.Exists(localPath))
                 {
                     levelFile = localPath;
                 }
                 else
                 {
                    Console.WriteLine($"Level file not found: {levelFile}");
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

            GameLoop gameLoop = new GameLoop(level);
            ConsoleRenderer renderer = new ConsoleRenderer(level);
            gameLoop.RegisterObserver(renderer);

            // Initial Draw
            gameLoop.TriggerGameTick();

            bool running = true;
            while (running)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    running = false;
                }
                else
                {
                    gameLoop.HandleInput(keyInfo.Key);
                }
            }
        }
    }
}
