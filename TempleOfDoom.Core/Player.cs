using System;
using System.Collections.Generic;
using System.Linq;
using TempleOfDoom.Core.Items;

namespace TempleOfDoom.Core
{
    // Indiana Jhones. Houdt bij waar ie is en wat ie bij zich heeft.
    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int CurrentRoomId { get; set; }
        public int Lives { get; set; }
        public int StonesCollected { get; set; }
        
        // Rugzakje voor items (zoals sleutels).
        public List<Item> Inventory { get; set; }

        public Player()
        {
            Inventory = new List<Item>();
        }

        // Checkt of we een bepaalde sleutel op zak hebben.
        public bool HasKey(string color)
        {
            return Inventory.OfType<Key>().Any(k => k.Color.ToLower() == color.ToLower());
        }

        // Voor debug/display: welke sleutels hebben we?
        public string GetKeyList()
        {
            var keys = Inventory.OfType<Key>().Select(k => k.Color).ToArray();
            return keys.Length > 0 ? string.Join(", ", keys) : "None";
        }
    }
}
