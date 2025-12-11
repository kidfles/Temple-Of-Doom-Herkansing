using System;
using TempleOfDoom.Core;
using CODE_TempleOfDoom_DownloadableContent;

namespace TempleOfDoom.Logic.Enemies
{
    public class RoomAdapter : IField
    {
        private Room _room;
        private int _x;
        private int _y;

        public RoomAdapter(Room room, int x, int y)
        {
            _room = room;
            _x = x;
            _y = y;
        }

        public IField? GetNeighbour(int direction)
        {
            int targetX = _x;
            int targetY = _y;

            // Mapping: 0=North, 1=East, 2=South, 3=West
            switch (direction)
            {
                case 0: targetY--; break;
                case 1: targetX++; break;
                case 2: targetY++; break;
                case 3: targetX--; break;
                default: return null;
            }

            if (targetX >= 0 && targetX < _room.Width &&
                targetY >= 0 && targetY < _room.Height)
            {
                return new RoomAdapter(_room, targetX, targetY);
            }

            return null;
        }

        public bool CanEnter
        {
            get
            {
                var tile = _room.GetTile(_x, _y);
                return tile != null && tile.IsWalkable((Player?)null);
            }
        }
        
        public IPlacable? Item { get; set; }
    }
}
