using TempleOfDoom.Core.Doors;

namespace TempleOfDoom.Core
{
    // Decorator Pattern context: Deze tegel 'wrapt' de IDoor logica.
    public class DoorTile : Tile
    {
        // De daadwerkelijke deur-logica zit in deze decorator chain.
        private IDoor _doorLogic;

        public int TargetRoomId { get; set; }
        public int TargetX { get; set; }
        public int TargetY { get; set; }

        public DoorTile(IDoor doorLogic)
        {
            _doorLogic = doorLogic;
        }

        public override bool Interact(Player player)
        {
            base.Interact(player);
            _doorLogic.Interact(player); // Trigger de deur (openen, sluiten, etc).
             
             // Als de deur open is en leidt naar een andere kamer, ga erheen.
             if (TargetRoomId != 0) 
             {
                 player.CurrentRoomId = TargetRoomId;
                 player.X = TargetX;
                 player.Y = TargetY;
             }
             return false;
        }

        // Mag je erdoor? Vraag het aan de deur-logica.
        public override bool IsWalkable(Player? player)
        {
            return _doorLogic.CanEnter(player);
        }

        protected override char GetBaseSprite()
        {
            return _doorLogic.GetSprite();
        }
    }
}
