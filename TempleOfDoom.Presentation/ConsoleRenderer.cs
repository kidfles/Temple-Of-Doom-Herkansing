using System;
using TempleOfDoom.Core;

namespace TempleOfDoom.Presentation
{
    public class ConsoleRenderer : IGameObserver
    {
        private Level level;

        public ConsoleRenderer(Level level)
        {
            this.level = level;
        }

        public void OnGameTick()
        {
            Console.Clear();
            if (level.CurrentRoom != null)
            {
                RenderRoom(level.CurrentRoom);
            }
        }

        private void RenderRoom(Room room)
        {
            for (int y = 0; y < room.Height; y++)
            {
                for (int x = 0; x < room.Width; x++)
                {
                   // Check if player is here
                   if (level.Player != null && level.Player.CurrentRoomId == room.Id && level.Player.X == x && level.Player.Y == y)
                   {
                       Console.Write('@');
                   }
                   else
                   {
                       IGameObject tile = room.GetTile(x, y);
                       if (tile != null)
                       {
                           Console.Write(tile.GetSprite());
                       }
                       else
                       {
                           Console.Write(' '); // Empty space if null
                       }
                   }
                }
                Console.WriteLine();
            }
        }
    }
}
