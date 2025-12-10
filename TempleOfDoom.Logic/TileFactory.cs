using System;
using TempleOfDoom.Core;

namespace TempleOfDoom.Logic
{
    public class TileFactory
    {
        public Tile CreateTile(string type)
        {
            switch (type.ToLower())
            {
                case "floor":
                    return new FloorTile();
                case "wall":
                    return new WallTile();
                default:
                    throw new ArgumentException($"Unknown tile type: {type}");
            }
        }
    }
}
