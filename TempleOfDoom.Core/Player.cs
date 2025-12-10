using System;
using System.Collections.Generic;
using TempleOfDoom.Core.Items;

namespace TempleOfDoom.Core
{
    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int CurrentRoomId { get; set; }
        
        public List<Item> Inventory { get; set; }

        public Player()
        {
            Inventory = new List<Item>();
        }

        public bool HasKey(string color)
        {
            foreach (var item in Inventory)
            {
                if (item is Key key && key.Color.ToLower() == color.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
