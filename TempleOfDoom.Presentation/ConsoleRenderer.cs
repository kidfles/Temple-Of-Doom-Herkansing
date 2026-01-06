using System;
using TempleOfDoom.Core;
using TempleOfDoom.Logic;

namespace TempleOfDoom.Presentation
{
    // Observer Pattern: Dit is de view, die luistert naar updates van de GameLoop.
    public class ConsoleRenderer : IGameObserver
    {
        private Level level;
        private GameLoop gameLoop;
        
        // Vaste grootte van het scherm zodat we geen flikkering krijgen.
        private const int VIEWPORT_HEIGHT = 20; 
        private const int VIEWPORT_WIDTH = 60;

        public ConsoleRenderer(Level level, GameLoop loop)
        {
            this.level = level;
            this.gameLoop = loop;
            Console.CursorVisible = false;
        }

        // Observer Pattern: Deze functie wordt aangeroepen door de GameLoop bij elke tick.
        public void OnGameTick()
        {
            Console.SetCursorPosition(0, 0); // Begin linksboven met tekenen.
            
            if (gameLoop.IsGameOver)
            {
                RenderEndScreen();
            }
            else if (level.CurrentRoom != null)
            {
                RenderHUD(); // Teken scores en levens.
                RenderRoom(level.CurrentRoom); // Teken de kamer zelf.
            }
        }

        private void RenderHUD()
        {
            var p = level.Player;
            if (p == null) return;
            // PadRight zorgt ervoor dat oude tekst wordt overschreven als de cijfers krimpen (voorkomt gekke visuals).
            Console.WriteLine(new string('=', VIEWPORT_WIDTH));
            Console.WriteLine($" LIVES: {p.Lives}  |  STONES: {p.StonesCollected}/{level.TotalStones}  |  TIME: {gameLoop.ElapsedTime:mm\\:ss}".PadRight(VIEWPORT_WIDTH));
            Console.WriteLine($" KEYS: {p.GetKeyList()}".PadRight(VIEWPORT_WIDTH));
            Console.WriteLine(new string('=', VIEWPORT_WIDTH));
        }

        // Teken de hele kamer regel voor regel.
        private void RenderRoom(Room room)
        {
            int drawnLines = 0;

            // 1. Bovenmuur tekenen
            Console.Write("#"); 
            for (int i = 0; i < room.Width; i++) Console.Write("#");
            // Vul de rest van de regel met spaties om oude 'ghost' muren weg te poetsen.
            Console.WriteLine("#".PadRight(VIEWPORT_WIDTH - room.Width - 1)); 
            drawnLines++;

            // 2. Inhoud van de kamer
            for (int y = 0; y < room.Height; y++)
            {
                Console.Write("#"); // Linkermuur
                for (int x = 0; x < room.Width; x++)
                {
                    // Teken de Speler (heeft prioriteit)
                    if (level.Player != null && level.Player.CurrentRoomId == room.Id && level.Player.X == x && level.Player.Y == y)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write('@');
                        Console.ResetColor();
                    }
                    else
                    {
                        // Teken Vijanden (hebben prioriteit boven tegels)
                        var enemy = room.Enemies.Find(e => e.X == x && e.Y == y);
                        if (enemy != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write('E');
                            Console.ResetColor();
                        }
                        else
                        {
                            // Teken Tegels/Items (als er niks anders staat)
                            IGameObject? tile = room.GetTile(x, y);
                            char sprite = tile != null ? tile.GetSprite() : ' ';
                            Console.Write(sprite);
                        }
                    }
                }
                // Rechtermuur + de rest van de regel schoonvegen.
                Console.WriteLine("#".PadRight(VIEWPORT_WIDTH - room.Width - 1));
                drawnLines++;
            }

            // 3. Ondermuur tekenen
            Console.Write("#");
            for (int i = 0; i < room.Width; i++) Console.Write("#");
            Console.WriteLine("#".PadRight(VIEWPORT_WIDTH - room.Width - 1));
            drawnLines++;
            
            // 4. Agressief Schoonmaken (Anti-Ghosting Fix)
            // Vul alle resterende regels van de viewport met lege regels zodat we geen oude troep zien.
            for (int i = drawnLines; i < VIEWPORT_HEIGHT; i++)
            {
                Console.WriteLine(new string(' ', VIEWPORT_WIDTH));
            }
        }

        // Als het spel klaar is (gewonnen of verloren), tekenen we dit schermpje.
        private void RenderEndScreen()
        {
            Console.Clear(); // Hier mag een clear wel, want het scherm verandert toch niet meer constant.
            if (gameLoop.HasWon)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\n   VICTORY! Je hebt alle stenen gevonden!\n");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\n   GAME OVER! Je bent dood.\n");
            }
            Console.ResetColor();
            Console.WriteLine($"   Eindtijd: {gameLoop.ElapsedTime:mm\\:ss}");
            Console.WriteLine("   Druk op ESC om af te sluiten.");
        }
    }
}
