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
                            if (tile is Tile concreteTile) 
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

                    if (connection.doors != null && connection.doors.Length > 0)
                    {
                    if (connection.doors != null && connection.doors.Length > 0)
                    {
                        IDoor sharedDoorLogic = tileFactory.CreateDoorLogic(connection.doors);

                        if (connection.NORTH.HasValue && connection.SOUTH.HasValue)
                        {
                            int r1Id = connection.NORTH.Value;
                            int r2Id = connection.SOUTH.Value;
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
                        else if (connection.WEST.HasValue && connection.EAST.HasValue)
                        {
                             int r1Id = connection.WEST.Value;
                             int r2Id = connection.EAST.Value;
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

                }
            }

            if (levelDto.player != null)
            {
                level.Player = new Player
                {
                    X = levelDto.player.startX,
                    Y = levelDto.player.startY,
                    CurrentRoomId = levelDto.player.startRoomId,
                };
                
                level.CurrentRoom = level.Rooms.Find(r => r.Id == level.Player.CurrentRoomId);
            }

            return level;
        }
    }
}
