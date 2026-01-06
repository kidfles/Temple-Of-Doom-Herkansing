using System.Collections.Generic;

namespace TempleOfDoom.Core
{
    // Het complete level object. Bevat alles: kamers, speler, en totaal aantal stenen.
    public class Level
    {
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
