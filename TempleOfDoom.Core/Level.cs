using System.Collections.Generic;

namespace TempleOfDoom.Core
{
    public class Level
    {
        // Domain object for the Level.
        public List<Room> Rooms { get; set; }
        public Room? CurrentRoom { get; set; }
        public Player? Player { get; set; }
        public int TotalStones { get; set; }

        public Level()
        {
            Rooms = new List<Room>();
        }
    }
}
