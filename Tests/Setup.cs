using System;
using BlockadeSimulatorCollisionMechanics;

namespace Tests
{
    public class TestSetup
    {
        protected Ship _playerOneShip;
        protected Ship _playerTwoShip;
        protected Ship _playerThreeShip;
        protected Rock _rock;

        public TestSetup WithShip(Ship ship)
        {
            _playerOneShip = ship;
            return this;
        }

        public TestSetup AndWithShip(Ship ship)
        {
            if (_playerTwoShip == null)
                _playerTwoShip = ship;
            else
                _playerThreeShip = ship;
            return this;
        }

        public TestSetup WithARock(RelativeDirection direction, Player player = Player.Player1)
        {
            _rock = new Rock();
            var ship = player == Player.Player1 ? _playerOneShip : _playerTwoShip;
            if(direction == RelativeDirection.FrontLeft && ship.FacingDirection == FacingDirection.Right)
            {
                _rock.XPos = ToTheRightOf(ship);
                _rock.YPos = Above(ship);
            }
            else if(direction == RelativeDirection.Behind && ship.FacingDirection == FacingDirection.Up)
            {
                _rock.XPos = ship.XPos;
                _rock.YPos = Below(ship);
            }
            else
            { 
                throw new Exception("WithARock - direction not supported");
            }
            return this;
        }

        private int ToTheRightOf(Ship ship)
        {
            return ship.XPos + 1;
        }

        private int Above(Ship ship)
        {
            return ship.YPos - 1;
        }

        private int Below(Ship ship)
        {
            return ship.YPos + 1;
        }

        public Board GetBoard()
        {
            return new Board() {
                PlayerOneShip = _playerOneShip,
                PlayerTwoShip = _playerTwoShip,
                PlayerThreeShip = _playerThreeShip,
                Rock = _rock };
        }
    }
}