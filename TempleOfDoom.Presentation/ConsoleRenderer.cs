using System;
using TempleOfDoom.Core;
using TempleOfDoom.Logic;

namespace TempleOfDoom.Presentation
{
    public class ConsoleRenderer : IGameObserver
    {
        private Level level;
        private GameLoop gameLoop;

        public ConsoleRenderer(Level level, GameLoop loop)
        {
            this.level = level;
            this.gameLoop = loop;
            Console.CursorVisible = false;
        }

        public void OnGameTick()
        {
            Console.SetCursorPosition(0, 0);
            
            if (gameLoop.IsGameOver)
            {
                RenderEndScreen();
            }
            else if (level.CurrentRoom != null)
            {
                RenderHUD();
                RenderRoom(level.CurrentRoom);
            }
        }

        private void RenderHUD()
        {
            var p = level.Player;
            Console.WriteLine(new string('=', 50));
            Console.WriteLine($" LIVES: {p.Lives}  |  STONES: {p.StonesCollected}/{level.TotalStones}  |  TIME: {gameLoop.ElapsedTime:mm\\:ss}");
            Console.WriteLine($" KEYS: {p.GetKeyList()}");
            Console.WriteLine(new string('=', 50));
        }

        private void RenderRoom(Room room)
        {
            // Top Wall
            Console.Write("#"); 
            for (int i = 0; i < room.Width; i++) Console.Write("#");
            Console.WriteLine("#");

            for (int y = 0; y < room.Height; y++)
            {
                Console.Write("#"); // Left Wall
                for (int x = 0; x < room.Width; x++)
                {
                   if (level.Player.CurrentRoomId == room.Id && level.Player.X == x && level.Player.Y == y)
                   {
                       Console.ForegroundColor = ConsoleColor.Cyan;
                       Console.Write('@');
                       Console.ResetColor();
                   }
                   else
                   {
                       var enemy = room.Enemies.Find(e => e.X == x && e.Y == y);
                       if (enemy != null)
                       {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write('E');
                            Console.ResetColor();
                       }
                       else
                       {
                           IGameObject? tile = room.GetTile(x, y);
                           if (tile != null)
                           {
                               Console.Write(tile.GetSprite());
                           }
                           else
                           {
                               Console.Write(' ');
                           }
                       }
                   }
                }
                Console.WriteLine("#"); // Right Wall
            }

            // Bottom Wall
            Console.Write("#");
            for (int i = 0; i < room.Width; i++) Console.Write("#");
            Console.WriteLine("#");
            
            // Clear rest of screen
            Console.WriteLine(new string(' ', 50));
            Console.WriteLine(new string(' ', 50));
        }

        private void RenderEndScreen()
        {
            Console.Clear();
            if (gameLoop.HasWon)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\n   VICTORY! You have collected all stones!\n");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\n   GAME OVER! You have died.\n");
            }
            Console.ResetColor();
            Console.WriteLine($"   Final Time: {gameLoop.ElapsedTime:mm\\:ss}");
            Console.WriteLine("   Press ESC to exit.");
        }
    }
}
