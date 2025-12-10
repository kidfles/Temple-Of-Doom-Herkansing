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
                case "floor":
                    return new FloorTile();
                case "wall":
                    return new WallTile();
                case "conveyor belt":
                    return new ConveyorBeltTile();
                default:
                    throw new ArgumentException($"Unknown tile type: {type}");
                }

        public Tile CreateDoor(DoorDto[] doorDtos)
        {
            IDoor door = new BasicDoor();
            if (doorDtos != null)
            {
                foreach (var doorDto in doorDtos)
                {
                    switch (doorDto.type.ToLower())
                    {
                        case "colored":
                            door = new ColoredDoor(door, doorDto.color);
                            break;
                        case "toggle":
                            door = new ToggleDoor(door);
                            break;
                        case "closing gate":
                            door = new ClosingGate(door);
                            break;
                    }
                }
            }
            return new DoorTile(door);
        }
    }       
}
