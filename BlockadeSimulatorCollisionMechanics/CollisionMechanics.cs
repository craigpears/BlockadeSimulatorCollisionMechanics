using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockadeSimulatorCollisionMechanics
{
    public class CollisionMechanics
    {
        public static CollisionResults Calculate(Board board)
        {
            var results = new CollisionResults()
            {
                ResultingBoard = board
            };

            if (board.Rock != null)
            {
                CalculateRockCollisions(board, results);
            }

            if (PlayerOneIsTurningLeft(board))
                RotateShipLeft(board.PlayerOneShip);

            var shipsMovingThroughOtherShipsPosition = GetShipsMovingThroughOtherShipsPosition(board);
            if (shipsMovingThroughOtherShipsPosition.Any())
            {
                results.Collisions.Add(new Collision { CollisionPhase = CollisionPhase.PhaseOne });
                
                if(shipsMovingThroughOtherShipsPosition.Count() == 1)
                {
                    var shipMoving = shipsMovingThroughOtherShipsPosition.Single();
                    if (SpaceToBeBumpedToForPlayerTwoIsEmpty(board)
                    && shipMoving == board.PlayerOneShip
                    && shipMoving.Moving == Move.Forward)
                    {
                        if (PlayerOneIsBigger(board))
                        {
                            PlayerOneWinsPlayerTwosSpace(board);
                        }

                        if (!PlayerTwoIsBigger(board))
                        {
                            BumpPlayerTwo(board);
                        }
                    }
                }             
            }
            else if (BothShipsContendingSameSquare(board))
            {
                Ship winningShip = null;
                if (PlayerOneIsBigger(board)) winningShip = board.PlayerOneShip;
                if (PlayerTwoIsBigger(board)) winningShip = board.PlayerTwoShip;

                results.Collisions.Add(
                    new Collision {
                        CollisionPhase = CollisionPhase.PhaseOne,
                        WinningShip = winningShip
                    }
                );
            }

            if (!results.Collisions.Any(x => x.WinningShip != board.PlayerOneShip))
            {
                if (board.PlayerOneShip.Moving == Move.Forward)
                {
                    board.PlayerOneShip.YPos++;
                }

                if (PlayerOneIsTurningLeft(board))
                {
                    SuccesfullyMovePlayerOneLeft(board);
                }
            }

            if(board.PlayerTwoShip != null && !results.Collisions.Any(x => x.WinningShip != board.PlayerTwoShip))
            {
                if (board.PlayerTwoShip.Moving == Move.Forward)
                {
                    board.PlayerTwoShip.YPos--;
                }
            }

            return results;
        }

        private static int SuccesfullyMovePlayerOneLeft(Board board)
        {
            return board.PlayerOneShip.YPos++;
        }

        private static bool SpaceToBeBumpedToForPlayerTwoIsEmpty(Board board)
        {
            var targetXPos = board.PlayerTwoShip.XPos;
            var targetYPos = board.PlayerTwoShip.YPos + 1;
            var boardObjects = new List<BoardObject> {
                board.Rock,
                board.PlayerThreeShip
            }
            .Where(x => x != null)
            .ToList();

            return !boardObjects.Any(x => InTargetSpot(x, targetXPos, targetYPos));
        }

        private static bool InTargetSpot(BoardObject x, int targetXPos, int targetYPos)
        {
            return x.XPos == targetXPos && x.YPos == targetYPos;
        }

        private static void PlayerOneWinsPlayerTwosSpace(Board board)
        {
            board.PlayerOneShip.XPos = board.PlayerTwoShip.XPos;
            board.PlayerOneShip.YPos = board.PlayerTwoShip.YPos;
        }

        private static bool PlayerOneIsBigger(Board board)
        {
            return board.PlayerOneShip.Size > board.PlayerTwoShip.Size;
        }

        private static bool PlayerTwoIsBigger(Board board)
        {
            return board.PlayerOneShip.Size < board.PlayerTwoShip.Size;
        }

        private static int BumpPlayerTwo(Board board)
        {
            return board.PlayerTwoShip.YPos++;
        }

        private static bool PlayerTwoIsNotMoving(Board board)
        {
            return board.PlayerTwoShip.Moving == Move.Empty;
        }

        private static List<Ship> GetShipsMovingThroughOtherShipsPosition(Board board)
        {
            var ships = new List<Ship>();
            if (board.PlayerTwoShip == null)
            {
                return ships;
            }

            var playerOneTargetX = board.PlayerOneShip.XPos;
            var playerOneTargetY = board.PlayerOneShip.YPos + 1;

            var playerTwoTargetX = board.PlayerTwoShip.XPos;
            var playerTwoTargetY = board.PlayerTwoShip.YPos - 1;

            var playerOneIsInTarget = InTargetSpot(board.PlayerOneShip, playerTwoTargetX, playerTwoTargetY);
            var playerTwoIsInTarget = InTargetSpot(board.PlayerTwoShip, playerOneTargetX, playerOneTargetY);

            var playerOneMovingToPlayerTwo = board.PlayerOneShip.Moving != Move.Empty && playerTwoIsInTarget;
            var playerTwoMovingToPlayerOne = board.PlayerTwoShip.Moving != Move.Empty && playerOneIsInTarget;

            if (playerOneMovingToPlayerTwo) ships.Add(board.PlayerOneShip);
            if (playerTwoMovingToPlayerOne) ships.Add(board.PlayerTwoShip);


            return ships;
        }



        private static bool BothShipsContendingSameSquare(Board board)
        {
            if (board.PlayerTwoShip == null) return false;

            var playerOneTargetX = board.PlayerOneShip.XPos;
            var playerOneTargetY = board.PlayerOneShip.YPos + 1;

            var playerTwoTargetX = board.PlayerTwoShip.XPos;
            var playerTwoTargetY = board.PlayerTwoShip.YPos - 1;

            var sameXTarget = playerOneTargetX == playerTwoTargetX;
            var sameYTarget = playerOneTargetY == playerTwoTargetY;

            return sameXTarget && sameYTarget;
        }

        private static bool PlayerOneIsTurningLeft(Board board)
        {
            return board.PlayerOneShip.Moving == Move.Left;
        }

        private static void RotateShipLeft(Ship ship)
        {
            switch (ship.FacingDirection)
            {
                case FacingDirection.Up:
                    ship.FacingDirection = FacingDirection.Left;
                    break;
                case FacingDirection.Right:
                    ship.FacingDirection = FacingDirection.Up;
                    break;
                case FacingDirection.Down:
                    ship.FacingDirection = FacingDirection.Right;
                    break;
                case FacingDirection.Left:
                    ship.FacingDirection = FacingDirection.Down;
                    break;
                default:
                    throw new Exception("Rotate ship left - unsupported direction");
            }
        }

        private static void CalculateRockCollisions(Board board, CollisionResults results)
        {
            board.PlayerOneShip.XPos = 2;
            if(PlayerOneIsTurningLeft(board))
                results.Collisions.Add(new Collision { CollisionPhase = CollisionPhase.PhaseTwo });
        }
    }
}
