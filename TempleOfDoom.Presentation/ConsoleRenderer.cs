using System;
using TempleOfDoom.Core;
using TempleOfDoom.Logic;

namespace TempleOfDoom.Presentation
{
    public class ConsoleRenderer : IGameObserver
    {
        private Level level;
        private GameLoop gameLoop;
        
        // Define a fixed viewport size to ensure we always clear enough space
        private const int VIEWPORT_HEIGHT = 20; 
        private const int VIEWPORT_WIDTH = 60;

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
            if (p == null) return;
            // PadRight ensures the string overrides old text if numbers shrink
            Console.WriteLine(new string('=', VIEWPORT_WIDTH));
            Console.WriteLine($" LIVES: {p.Lives}  |  STONES: {p.StonesCollected}/{level.TotalStones}  |  TIME: {gameLoop.ElapsedTime:mm\\:ss}".PadRight(VIEWPORT_WIDTH));
            Console.WriteLine($" KEYS: {p.GetKeyList()}".PadRight(VIEWPORT_WIDTH));
            Console.WriteLine(new string('=', VIEWPORT_WIDTH));
        }

        private void RenderRoom(Room room)
        {
            int drawnLines = 0;

            // 1. Top Wall
            Console.Write("#"); 
            for (int i = 0; i < room.Width; i++) Console.Write("#");
            // Fill the rest of the line with space to erase old ghost walls
            Console.WriteLine("#".PadRight(VIEWPORT_WIDTH - room.Width - 1)); 
            drawnLines++;

            // 2. Room Content
            for (int y = 0; y < room.Height; y++)
            {
                Console.Write("#"); // Left Wall
                for (int x = 0; x < room.Width; x++)
                {
                    // Draw Player
                    if (level.Player != null && level.Player.CurrentRoomId == room.Id && level.Player.X == x && level.Player.Y == y)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write('@');
                        Console.ResetColor();
                    }
                    else
                    {
                        // Draw Enemies
                        var enemy = room.Enemies.Find(e => e.X == x && e.Y == y);
                        if (enemy != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write('E');
                            Console.ResetColor();
                        }
                        else
                        {
                            // Draw Tiles/Items
                            IGameObject? tile = room.GetTile(x, y);
                            char sprite = tile != null ? tile.GetSprite() : ' ';
                            Console.Write(sprite);
                        }
                    }
                }
                // Right Wall + Clear remaining line width
                Console.WriteLine("#".PadRight(VIEWPORT_WIDTH - room.Width - 1));
                drawnLines++;
            }

            // 3. Bottom Wall
            Console.Write("#");
            for (int i = 0; i < room.Width; i++) Console.Write("#");
            Console.WriteLine("#".PadRight(VIEWPORT_WIDTH - room.Width - 1));
            drawnLines++;
            
            // 4. Aggressive Clearing (The Anti-Ghosting Fix)
            // Fill the remaining vertical space of the viewport with blank lines
            for (int i = drawnLines; i < VIEWPORT_HEIGHT; i++)
            {
                Console.WriteLine(new string(' ', VIEWPORT_WIDTH));
            }
        }

        private void RenderEndScreen()
        {
            Console.Clear(); // It is okay to clear once at the end
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
