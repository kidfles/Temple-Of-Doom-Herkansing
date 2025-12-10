using System;
using System.Collections.Generic;
using System.Linq;
using TempleOfDoom.Core.Items;

namespace TempleOfDoom.Core
{
    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int CurrentRoomId { get; set; }
        public int Lives { get; set; }
        public int StonesCollected { get; set; }
        public List<Item> Inventory { get; set; }

        public Player()
        {
            Inventory = new List<Item>();
        }

        public bool HasKey(string color)
        {
            return Inventory.OfType<Key>().Any(k => k.Color.ToLower() == color.ToLower());
        }

        public string GetKeyList()
        {
            var keys = Inventory.OfType<Key>().Select(k => k.Color).ToArray();
            return keys.Length > 0 ? string.Join(", ", keys) : "None";
        }
    }
}
