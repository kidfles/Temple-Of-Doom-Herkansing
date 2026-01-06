using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TempleOfDoom.Core.DTOs
{
    // DTO (Data Transfer Object): Deze klassen zijn PUUR voor het inlezen van de JSON file.
    // Hier zit GEEN logica in, alleen data structuren die matchen met de JSON.
    public class LevelDto
    {
        [JsonPropertyName("rooms")]
        public RoomDto[]? Rooms { get; set; }
        
        [JsonPropertyName("connections")]
        public ConnectionDto[]? Connections { get; set; }
        
        [JsonPropertyName("player")]
        public PlayerDto? Player { get; set; }
    }

    public class PlayerDto
    {
        [JsonPropertyName("startRoomId")]
        public int StartRoomId { get; set; }
        
        [JsonPropertyName("startX")]
        public int StartX { get; set; }
        
        [JsonPropertyName("startY")]
        public int StartY { get; set; }
        
        [JsonPropertyName("lives")]
        public int Lives { get; set; }
    }

    public class RoomDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        
        [JsonPropertyName("width")]
        public int Width { get; set; }
        
        [JsonPropertyName("height")]
        public int Height { get; set; }
        
        [JsonPropertyName("items")]
        public ItemDto[]? Items { get; set; }
        
        [JsonPropertyName("specialFloorTiles")]
        public SpecialTileDto[]? SpecialFloorTiles { get; set; }
        
        [JsonPropertyName("enemies")]
        public EnemyDto[]? Enemies { get; set; }
    }

    public class ItemDto
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        
        [JsonPropertyName("damage")]
        public int Damage { get; set; }
        
        [JsonPropertyName("x")]
        public int X { get; set; }
        
        [JsonPropertyName("y")]
        public int Y { get; set; }
        
        [JsonPropertyName("color")]
        public string? Color { get; set; }
    }

    // Module B: Specifieke data voor Module B tegels (zoals conveyor belts).
    public class SpecialTileDto
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        
        [JsonPropertyName("direction")]
        public string? Direction { get; set; }
        
        [JsonPropertyName("x")]
        public int X { get; set; }
        
        [JsonPropertyName("y")]
        public int Y { get; set; }
    }

    public class EnemyDto
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        
        [JsonPropertyName("x")]
        public int X { get; set; }
        
        [JsonPropertyName("y")]
        public int Y { get; set; }
        
        [JsonPropertyName("minX")]
        public int MinX { get; set; }
        
        [JsonPropertyName("minY")]
        public int MinY { get; set; }
        
        [JsonPropertyName("maxX")]
        public int MaxX { get; set; }
        
        [JsonPropertyName("maxY")]
        public int MaxY { get; set; }
    }

    // Module B: Module B portals en deuren.
    public class ConnectionDto
    {
        [JsonPropertyName("NORTH")]
        public int? North { get; set; }

        [JsonPropertyName("SOUTH")]
        public int? South { get; set; }
        
        [JsonPropertyName("WEST")]
        public int? West { get; set; }
        
        [JsonPropertyName("EAST")]
        public int? East { get; set; }
        
        [JsonPropertyName("doors")]
        public DoorDto[]? Doors { get; set; }
        
        [JsonPropertyName("portal")]
        public PortalDto[]? Portals { get; set; }
    }

    public class DoorDto
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        
        [JsonPropertyName("color")]
        public string? Color { get; set; }
        
        [JsonPropertyName("no_of_stones")]
        public int NoOfStones { get; set; }
    }

    public class PortalDto
    {
        [JsonPropertyName("roomId")]
        public int RoomId { get; set; }
        
        [JsonPropertyName("x")]
        public int X { get; set; }
        
        [JsonPropertyName("y")]
        public int Y { get; set; }
    }
}
