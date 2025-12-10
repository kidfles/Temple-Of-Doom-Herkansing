using System.IO;
using System.Text.Json;
using TempleOfDoom.Core;
using TempleOfDoom.Core.DTOs;
using TempleOfDoom.Core.Items;
using TempleOfDoom.Core.Doors;

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

            foreach (var roomDto in levelDto.Rooms)
            {
                Room room = new Room(roomDto.Id, roomDto.Width, roomDto.Height);

                // Default fill with floor
                for (int x = 0; x < roomDto.Width; x++)
                {
                    for (int y = 0; y < roomDto.Height; y++)
                    {
                        room.SetTile(x, y, tileFactory.CreateTile(TileTypes.Floor));
                    }
                }

                if (roomDto.SpecialFloorTiles != null)
                {
                    foreach (var specialTile in roomDto.SpecialFloorTiles)
                    {
                        Tile tile = tileFactory.CreateTile(specialTile.Type);
                        if (tile is ConveyorBeltTile conveyor)
                        {
                            if (System.Enum.TryParse<Direction>(specialTile.Direction, true, out var dir))
                            {
                                conveyor.Direction = dir;
                            }
                        }
                        room.SetTile(specialTile.X, specialTile.Y, tile);
                    }
                }

                if (roomDto.Items != null)
                {
                    foreach (var itemDto in roomDto.Items)
                    {
                        Item item = null;
                        switch (itemDto.Type.ToLower())
                        {
                            case TileTypes.SankaraStone:
                                item = new SankaraStone();
                                level.TotalStones++;
                                break;
                            case TileTypes.Key:
                                item = new Key { Color = itemDto.Color };
                                break;
                            case TileTypes.BoobyTrap:
                                item = new BoobyTrap { Damage = itemDto.Damage };
                                break;
                            case TileTypes.DisappearingBoobyTrap:
                                item = new DisappearingBoobyTrap { Damage = itemDto.Damage };
                                break;
                        }

                        if (item != null)
                        {
                            var tile = room.GetTile(itemDto.X, itemDto.Y);
                            if (tile is Tile concreteTile)
                            {
                                concreteTile.CurrentItem = item;
                            }
                        }
                    }
                }

                level.Rooms.Add(room);
            }

            if (levelDto.Connections != null)
            {
                foreach (var connection in levelDto.Connections)
                {
                    if (connection.Portals != null && connection.Portals.Length == 2)
                    {
                        var portal1 = connection.Portals[0];
                        var portal2 = connection.Portals[1];

                        PortalTile tile1 = new PortalTile
                        {
                            TargetRoomId = portal2.RoomId,
                            TargetX = portal2.X,
                            TargetY = portal2.Y
                        };

                        PortalTile tile2 = new PortalTile
                        {
                            TargetRoomId = portal1.RoomId,
                            TargetX = portal1.X,
                            TargetY = portal1.Y
                        };

                        Room room1 = level.Rooms.Find(r => r.Id == portal1.RoomId);
                        if (room1 != null)
                        {
                            room1.SetTile(portal1.X, portal1.Y, tile1);
                        }

                        Room room2 = level.Rooms.Find(r => r.Id == portal2.RoomId);
                        if (room2 != null)
                        {
                            room2.SetTile(portal2.X, portal2.Y, tile2);
                        }
                    }

                    if (connection.Doors != null)
                    {
                        // Create shared door logic
                        IDoor sharedDoorLogic = tileFactory.CreateDoorLogic(connection.Doors);

                        if (connection.North.HasValue && connection.South.HasValue)
                        {
                            int r1Id = connection.North.Value;
                            int r2Id = connection.South.Value;
                            Room r1 = level.Rooms.Find(r => r.Id == r1Id);
                            Room r2 = level.Rooms.Find(r => r.Id == r2Id);

                            if (r1 != null && r2 != null)
                            {
                                int x1 = r1.Width / 2;
                                int y1 = r1.Height - 1;

                                int x2 = r2.Width / 2;
                                int y2 = 0;

                                // Door in R1 -> Targets R2
                                Tile d1 = tileFactory.CreateDoorTile(sharedDoorLogic, r2Id, x2, y2);
                                r1.SetTile(x1, y1, d1);

                                // Door in R2 -> Targets R1
                                Tile d2 = tileFactory.CreateDoorTile(sharedDoorLogic, r1Id, x1, y1);
                                r2.SetTile(x2, y2, d2);
                            }
                        }
                        else if (connection.West.HasValue && connection.East.HasValue)
                        {
                            int r1Id = connection.West.Value;
                            int r2Id = connection.East.Value;
                            Room r1 = level.Rooms.Find(r => r.Id == r1Id);
                            Room r2 = level.Rooms.Find(r => r.Id == r2Id);

                            if (r1 != null && r2 != null)
                            {
                                int x1 = r1.Width - 1;
                                int y1 = r1.Height / 2;

                                int x2 = 0;
                                int y2 = r2.Height / 2;

                                // Door in R1 -> Targets R2
                                Tile d1 = tileFactory.CreateDoorTile(sharedDoorLogic, r2Id, x2, y2);
                                r1.SetTile(x1, y1, d1);

                                // Door in R2 -> Targets R1
                                Tile d2 = tileFactory.CreateDoorTile(sharedDoorLogic, r1Id, x1, y1);
                                r2.SetTile(x2, y2, d2);
                            }
                        }
                    }
                }
            }

            if (levelDto.Player != null)
            {
                level.Player = new Player
                {
                    X = levelDto.Player.StartX,
                    Y = levelDto.Player.StartY,
                    CurrentRoomId = levelDto.Player.StartRoomId,
                    Lives = levelDto.Player.Lives > 0 ? levelDto.Player.Lives : 3
                };

                level.CurrentRoom = level.Rooms.Find(r => r.Id == level.Player.CurrentRoomId);
            }

            return level;
        }
    }
}
