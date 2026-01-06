namespace TempleOfDoom.Core
{
    // Hier verzamelen we alle vaste namen (magic strings) zodat we geen typefouten maken bij het laden.
    public static class TileTypes
    {
        public const string Floor = "floor";
        public const string Wall = "wall";
        
        // Module B: Specifieke tile voor de lopende band logica.
        public const string ConveyorBelt = "conveyor belt";
        
        public const string SankaraStone = "sankara stone";
        public const string Key = "key";
        public const string BoobyTrap = "boobytrap";
        public const string DisappearingBoobyTrap = "disappearing boobytrap";
        
        // Door Decorator Types: Verschillende soorten deuren die we kunnen maken.
        public const string Colored = "colored";
        public const string Toggle = "toggle";
        public const string ClosingGate = "closing gate";
    }
}
