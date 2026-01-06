using System;
using TempleOfDoom.Core;
using CODE_TempleOfDoom_DownloadableContent;

namespace TempleOfDoom.Logic.Enemies
{
    // Adapter Pattern: De DLL verwacht een 'IField', maar wij hebben 'Room'.
    // Deze klasse vertaalt onze Room naar iets wat de DLL vijand begrijpt.
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

        // De DLL vraagt om de buurman. Wij zoeken die in ons grid.
        public IField? GetNeighbour(int direction)
        {
            int targetX = _x;
            int targetY = _y;

            // Mapping: 0=Noord, 1=Oost, 2=Zuid, 3=West
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

        // Kan de vijand hier staan? Wij checken 'IsWalkable'.
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
