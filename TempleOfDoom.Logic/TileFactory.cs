using System;
using TempleOfDoom.Core;
using TempleOfDoom.Core.DTOs;
using TempleOfDoom.Core.Doors;

namespace TempleOfDoom.Logic
{
    // Factory Pattern: De centrale plek waar we tegels maken.
    // Dit zorgt ervoor dat de rest van de code niet hoeft te weten HOE je precies een tegel of deur maakt.
    public class TileFactory
    {
        public Tile CreateTile(string type)
        {
            switch (type.ToLower())
            {
                // Gewone tegels.
                case TileTypes.Floor:
                    return new FloorTile();
                case TileTypes.Wall:
                    return new WallTile();
                // Module B: Hier maken we de lopende band aan.
                case TileTypes.ConveyorBelt:
                    return new ConveyorBeltTile();
                default:
                    throw new ArgumentException($"Unknown tile type: {type}");
                }
        }

        // Decorator Pattern: Hier bouwen we de 'ui' van decorators om de deur heen.
        public IDoor CreateDoorLogic(DoorDto[] doorDtos)
        {
            IDoor door = new BasicDoor(); // Begin altijd met een simpele deur.
            if (doorDtos != null)
            {
                foreach (var doorDto in doorDtos)
                {
                    // Wrap de deur in extra functionaliteit (Decorators).
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
            // Stop de (mogelijk versierde) deur-logica in de tegel.
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
