using System.IO;
using System.Text.Json;
using TempleOfDoom.Core;
using TempleOfDoom.Core.DTOs;
using TempleOfDoom.Core.Items;

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
                            if (System.Enum.TryParse<Direction>(specialTile.direction, true, out var dir))
                            {
                                conveyor.Direction = dir;
                            }
                        }
                        room.SetTile(specialTile.x, specialTile.y, tile);
                    }
                }

                        room.SetTile(specialTile.x, specialTile.y, tile);
                    }
                }

                if (roomDto.items != null)
                {
                    foreach (var itemDto in roomDto.items)
                    {
                        Item item = null;
                        switch (itemDto.type.ToLower())
                        {
                            case "sankara stone":
                                item = new SankaraStone();
                                break;
                            case "key":
                                item = new Key { Color = itemDto.color };
                                break;
                            case "boobytrap":
                                item = new BoobyTrap { Damage = itemDto.damage };
                                break;
                            case "disappearing boobytrap":
                                item = new DisappearingBoobyTrap { Damage = itemDto.damage };
                                break;
                        }

                        if (item != null)
                        {
                            var tile = room.GetTile(itemDto.x, itemDto.y);
                            if (tile is Tile concreteTile) // Room.GetTile returns IGameObject, need cast to Tile to set CurrentItem
                            {
                                concreteTile.CurrentItem = item;
                            }
                        }
                    }
                }

                level.Rooms.Add(room);
            }

            if (levelDto.connections != null)
            {
                foreach (var connection in levelDto.connections)
                {
                    if (connection.portal != null && connection.portal.Length == 2)
                    {
                        var portal1 = connection.portal[0];
                        var portal2 = connection.portal[1];

                       
                        PortalTile tile1 = new PortalTile
                        {
                            TargetRoomId = portal2.roomId,
                            TargetX = portal2.x,
                            TargetY = portal2.y
                        };


                        PortalTile tile2 = new PortalTile
                        {
                            TargetRoomId = portal1.roomId,
                            TargetX = portal1.x,
                            TargetY = portal1.y
                        };

                    
                        Room room1 = level.Rooms.Find(r => r.Id == portal1.roomId);
                        if (room1 != null)
                        {
                            room1.SetTile(portal1.x, portal1.y, tile1);
                        }

                        Room room2 = level.Rooms.Find(r => r.Id == portal2.roomId);
                        if (room2 != null)
                        {
                            room2.SetTile(portal2.x, portal2.y, tile2);
                        }
                    }
                }
            }

            return level;
        }
    }
}
