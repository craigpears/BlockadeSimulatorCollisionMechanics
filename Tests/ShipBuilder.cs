using BlockadeSimulatorCollisionMechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class ShipBuilder
    {
        protected Ship _ship = new Ship();

        public ShipBuilder LargeShip(bool isTrue = true)
        {
            if(isTrue) _ship.Size = ShipSize.Large;
            return this;
        }

        public ShipBuilder At(int xPos, int yPos)
        {
            _ship.XPos = xPos;
            _ship.YPos = yPos;
            return this;
        }

        public ShipBuilder Facing(FacingDirection direction)
        {
            _ship.FacingDirection = direction;
            return this;
        }

        public ShipBuilder Moving(Move move)
        {
            _ship.Moving = move;
            return this;
        }

        public ShipBuilder NotMoving()
        {
            _ship.Moving = Move.Empty;
            return this;
        }

        public Ship GetShip()
        {
            return _ship;
        }
    }
}
