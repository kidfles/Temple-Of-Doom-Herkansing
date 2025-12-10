using System.IO;
using System.Text.Json;
using TempleOfDoom.Core;
using TempleOfDoom.Core.DTOs;

namespace TempleOfDoom.Logic
{
    public class JsonLevelLoader : ILevelLoader
    {
        public Level LoadLevel(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"The level file was not found: {path}");
            }

            string jsonContent = File.ReadAllText(path);
            LevelDto levelDto = JsonSerializer.Deserialize<LevelDto>(jsonContent);

            Level level = new Level();
            TileFactory tileFactory = new TileFactory();

            foreach (var roomDto in levelDto.rooms)
            {
                Room room = new Room(roomDto.id, roomDto.width, roomDto.height);

                // Default fill with floor
                for (int x = 0; x < roomDto.width; x++)
                {
                    for (int y = 0; y < roomDto.height; y++)
                    {
                        room.SetTile(x, y, tileFactory.CreateTile("floor"));
                    }
                }
                
                // Add walls? The user requirement says "Grid of objects". 
                // Usually floors are bounded by walls, but the DTO doesn't specify walls explicitly in a grid format like a map string.
                // Assuming default is floor, and we overlay special tiles.

                if (roomDto.specialFloorTiles != null)
                {
                    foreach (var specialTile in roomDto.specialFloorTiles)
                    {
                        Tile tile = tileFactory.CreateTile(specialTile.type);
                        if (tile is ConveyorBeltTile conveyor)
                        {
                            conveyor.Direction = specialTile.direction;
                        }
                        room.SetTile(specialTile.x, specialTile.y, tile);
                    }
                }

                level.Rooms.Add(room);
            }

            return level;
        }
    }
}
