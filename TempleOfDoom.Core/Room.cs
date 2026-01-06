
namespace TempleOfDoom.Core
{
    // Een kamer in de tempel. Bestaat uit een grid van tegels.
    public class Room
    {
        public int Id { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        
        // Het grid met alle tegels.
        private IGameObject[,] tiles;

        public Room(int id, int width, int height)
        {
            Id = id;
            Width = width;
            Height = height;
            Height = height;
            tiles = new IGameObject[width, height];
            Enemies = new List<IEnemy>();
        }

        public List<IEnemy> Enemies { get; set; }

        // Vraag veilig een tegel op.
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
