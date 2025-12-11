using System;
using TempleOfDoom.Core;
using TempleOfDoom.Core.DTOs;
using TempleOfDoom.Core.Doors;

namespace TempleOfDoom.Logic
{
    public class TileFactory
    {
        public Tile CreateTile(string type)
        {
            switch (type.ToLower())
            {
                case TileTypes.Floor:
                    return new FloorTile();
                case TileTypes.Wall:
                    return new WallTile();
                case TileTypes.ConveyorBelt:
                    return new ConveyorBeltTile();
                default:
                    throw new ArgumentException($"Unknown tile type: {type}");
                }
        }

        public IDoor CreateDoorLogic(DoorDto[] doorDtos)
        {
            IDoor door = new BasicDoor();
            if (doorDtos != null)
            {
                foreach (var doorDto in doorDtos)
                {
                    switch (doorDto.Type?.ToLower())
                    {
                        case TileTypes.Colored:
                            door = new ColoredDoor(door, doorDto.Color ?? "red");
                            break;
                        case TileTypes.Toggle:
                            door = new ToggleDoor(door);
                            break;
                        case TileTypes.ClosingGate:
                            door = new ClosingGate(door);
                            break;
                    }
                }
            }
            return door;
        }

        public Tile CreateDoorTile(IDoor doorLogic, int targetRoomId, int targetX, int targetY)
        {
            var tile = new DoorTile(doorLogic)
            {
                TargetRoomId = targetRoomId,
                TargetX = targetX,
                TargetY = targetY
            };
            return tile;
        }
    }       
}
