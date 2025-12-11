
namespace TempleOfDoom.Core
{
    public class Room
    {
        public int Id { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        private IGameObject[,] tiles;

        public Room(int id, int width, int height)
        {
            Id = id;
            Width = width;
            Height = height;
            tiles = new IGameObject[width, height];
        }

        public IGameObject? GetTile(int x, int y)
        {
            if (IsValidCoordinate(x, y))
            {
                return tiles[x, y];
            }
            return null; 
        }

        public void SetTile(int x, int y, IGameObject tile)
        {
            if (IsValidCoordinate(x, y))
            {
                tiles[x, y] = tile;
            }
        }

        private bool IsValidCoordinate(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
    }
}
