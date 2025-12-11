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
            LevelDto? levelDto = JsonSerializer.Deserialize<LevelDto>(jsonContent);

            if (levelDto == null || levelDto.Rooms == null)
            {
                throw new Exception("Failed to load level: JSON content is invalid or empty.");
            }

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
                        if (specialTile.Type == null) continue; // Skip invalid data

                        Tile tile = tileFactory.CreateTile(specialTile.Type);
                        if (tile is ConveyorBeltTile conveyor && specialTile.Direction != null)
                        {
                            if (Enum.TryParse<Direction>(specialTile.Direction, true, out var dir))
                            {
                                conveyor.Direction = dir;
                            }
                        }
                        room.SetTile(specialTile.X, specialTile.Y, tile);
                    }
                }

                // 3. Safe Array Iteration (Items)
                if (roomDto.Items != null)
                {
                    foreach (var itemDto in roomDto.Items)
                    {
                        if (itemDto.Type == null) continue;

                        Item? item = null;
                        switch (itemDto.Type.ToLower())
                        {
                            case TileTypes.SankaraStone:
                                item = new SankaraStone();
                                level.TotalStones++;
                                break;
                            case TileTypes.Key:
                                item = new Key { Color = itemDto.Color ?? "unknown" };
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
                            var tile = room.GetTile(itemDto.X, itemDto.Y) as Tile;
                            if (tile != null)
                            {
                                tile.CurrentItem = item;
                            }
                        }
                    }
                }

                // 3.5 Load Enemies
                if (roomDto.Enemies != null)
                {
                    foreach (var enemyDto in roomDto.Enemies)
                    {
                        CODE_TempleOfDoom_DownloadableContent.Enemy? dllEnemy = null;
                        if (enemyDto.Type == "horizontal")
                        {
                            dllEnemy = new CODE_TempleOfDoom_DownloadableContent.HorizontallyMovingEnemy(1, enemyDto.X, enemyDto.Y, enemyDto.MinX, enemyDto.MaxX);
                        }
                        else if (enemyDto.Type == "vertical")
                        {
                            dllEnemy = new CODE_TempleOfDoom_DownloadableContent.VerticallyMovingEnemy(1, enemyDto.X, enemyDto.Y, enemyDto.MinY, enemyDto.MaxY);
                        }

                        if (dllEnemy != null)
                        {
                            dllEnemy.CurrentField = new TempleOfDoom.Logic.Enemies.RoomAdapter(room, enemyDto.X, enemyDto.Y);
                            
                            var adapter = new TempleOfDoom.Logic.Enemies.EnemyAdapter(dllEnemy);
                            room.Enemies.Add(adapter);
                        }
                    }
                }

                level.Rooms.Add(room);
            }

            // 4. Safe Array Iteration (Connections)
            if (levelDto.Connections != null)
            {
                foreach (var connection in levelDto.Connections)
                {
                    // Portals
                    if (connection.Portals != null && connection.Portals.Length == 2)
                    {
                        var portal1 = connection.Portals[0];
                        var portal2 = connection.Portals[1];

                        // Create two linked tiles
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

                        // Assign to rooms (safely)
                        var room1 = level.Rooms.Find(r => r.Id == portal1.RoomId);
                        if (room1 != null) room1.SetTile(portal1.X, portal1.Y, tile1);

                        var room2 = level.Rooms.Find(r => r.Id == portal2.RoomId);
                        if (room2 != null) room2.SetTile(portal2.X, portal2.Y, tile2);
                    }

                    // Doors
                    if (connection.Doors != null)
                    {
                        IDoor sharedDoorLogic = tileFactory.CreateDoorLogic(connection.Doors);

                        if (connection.North.HasValue && connection.South.HasValue)
                        {
                            int r1Id = connection.North.Value;
                            int r2Id = connection.South.Value;
                            var r1 = level.Rooms.Find(r => r.Id == r1Id);
                            var r2 = level.Rooms.Find(r => r.Id == r2Id);

                            if (r1 != null && r2 != null)
                            {
                                int x1 = r1.Width / 2;
                                int y1 = r1.Height - 1;
                                int x2 = r2.Width / 2;
                                int y2 = 0;

                                r1.SetTile(x1, y1, tileFactory.CreateDoorTile(sharedDoorLogic, r2Id, x2, y2));
                                r2.SetTile(x2, y2, tileFactory.CreateDoorTile(sharedDoorLogic, r1Id, x1, y1));
                            }
                        }
                        else if (connection.West.HasValue && connection.East.HasValue)
                        {
                            int r1Id = connection.West.Value;
                            int r2Id = connection.East.Value;
                            var r1 = level.Rooms.Find(r => r.Id == r1Id);
                            var r2 = level.Rooms.Find(r => r.Id == r2Id);

                            if (r1 != null && r2 != null)
                            {
                                int x1 = r1.Width - 1;
                                int y1 = r1.Height / 2;
                                int x2 = 0;
                                int y2 = r2.Height / 2;

                                r1.SetTile(x1, y1, tileFactory.CreateDoorTile(sharedDoorLogic, r2Id, x2, y2));
                                r2.SetTile(x2, y2, tileFactory.CreateDoorTile(sharedDoorLogic, r1Id, x1, y1));
                            }
                        }
                    }
                }
            }

            // 5. Player Initialization
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
            
            // 6. Final safety check
            if (level.CurrentRoom == null && level.Rooms.Count > 0)
            {
                level.CurrentRoom = level.Rooms[0];
            }

            return level;
        }
    }
}
