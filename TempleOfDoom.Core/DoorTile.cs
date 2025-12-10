using TempleOfDoom.Core.Doors;

namespace TempleOfDoom.Core
{
    public class DoorTile : Tile
    {
        private IDoor _doorLogic;

        public DoorTile(IDoor doorLogic)
        {
            _doorLogic = doorLogic;
        }

        public override void Interact(Player player)
        {
            _doorLogic.Interact(player);
        }

        public override bool IsWalkable()
        {
            // We need to pass player to IsWalkable? 
            // The Tile.IsWalkable() signature is just bool IsWalkable(). 
            // But IDoor.CanEnter(Player p) requires Player.
            // Problem: The base Tile.IsWalkable() doesn't take params.
            // Solution: We might need to change Tile.IsWalkable() signature or 
            // assume for pathfinding it's walkable, but the actual movement logic checks Interact/Enter?
            
            // Wait, the Requirement says "Forward IsWalkable calls to _doorLogic.CanEnter".
            // If Tile class signature is fixed, we have a mismatch.
            // I should check Tile.cs signature again.
            // Tile.cs: public abstract bool IsWalkable();
            
            // I will implement it as returning true generally OR I need to change the recursive definition.
            // Actually, if I can't access player here, I can't check inventory.
            
            // Workaround: Return true here, but the Movement Logic in Game (Program/Controller) 
            // should check `CanEnter(player)` if the tile is a DoorTile?
            // OR I change Tile.cs IsWalkable to take a Player? 
            // "IsWalkable()" implies static property usually. 
            // But for a door, it depends on the player.
            
            // I will update Tile.cs to IsWalkable(Player player) ? 
            // That's a breaking change for other tiles but easy fix. 
            // Let's assume standard game design: Can I walk there? Yes/No for *me*.
            
            // I will update Tile.cs IsWalkable() -> IsWalkable(Player player). I'll include that in a separate step or just do it now if I can't compile.
            // For this specific file write, I'll temporarily put return true and fix it in next steps.
            return true; 
        }

        // Helper for the actual interaction check
        public bool CanEnter(Player player)
        {
            return _doorLogic.CanEnter(player);
        }

        protected override char GetBaseSprite()
        {
            return _doorLogic.GetSprite();
        }
    }
}
