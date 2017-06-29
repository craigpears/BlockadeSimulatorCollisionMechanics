using BlockadeSimulatorCollisionMechanics;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Ships_turning_left_with_rocks_at_the_destination_should_collide_in_phase_2()
        {
            var board = new TestSetup()
                .WithShip(
                    new ShipBuilder()
                    .At(1,1)
                    .Facing(FacingDirection.Right)
                    .Moving(Move.Left)
                    .GetShip()
                )
                .WithARock(RelativeDirection.FrontLeft)
                .GetBoard();
            var collisionResults = CollisionMechanics.Calculate(board);

            Assert.AreEqual(CollisionPhase.PhaseTwo, collisionResults.Collisions.Single().CollisionPhase);
            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerOneShip.XPos);
            Assert.AreEqual(1, collisionResults.ResultingBoard.PlayerOneShip.YPos);
            Assert.AreEqual(FacingDirection.Up, collisionResults.ResultingBoard.PlayerOneShip.FacingDirection);
        }

        [Test]
        public void Ships_turning_left_into_an_empty_space_should_have_no_collisions()
        {
            var board = new TestSetup()
                .WithShip(
                    new ShipBuilder()
                    .At(1, 1)
                    .Facing(FacingDirection.Right)
                    .Moving(Move.Left)
                    .GetShip()
                )
                .GetBoard();
            var collisionResults = CollisionMechanics.Calculate(board);

            Assert.AreEqual(0, collisionResults.Collisions.Count());
            Assert.AreEqual(1, collisionResults.ResultingBoard.PlayerOneShip.XPos);
            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerOneShip.YPos);
            Assert.AreEqual(FacingDirection.Up, collisionResults.ResultingBoard.PlayerOneShip.FacingDirection);
        }

        [Test]
        public void Ships_facing_each_other_should_collide_but_not_move_when_moving_forwards_into_each_other()
        {
            var board = new TestSetup()
                .WithShip(
                    new ShipBuilder()
                    .At(2, 2)
                    .Facing(FacingDirection.Down)
                    .Moving(Move.Forward)
                    .GetShip()
                )
                .AndWithShip(
                    new ShipBuilder()
                    .At(2, 3)
                    .Facing(FacingDirection.Up)
                    .Moving(Move.Forward)
                    .GetShip()
                )
                .GetBoard();
            var collisionResults = CollisionMechanics.Calculate(board);

            Assert.AreEqual(CollisionPhase.PhaseOne, collisionResults.Collisions.Single().CollisionPhase);

            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerOneShip.XPos);
            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerOneShip.YPos);
            Assert.AreEqual(FacingDirection.Down, collisionResults.ResultingBoard.PlayerOneShip.FacingDirection);

            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerTwoShip.XPos);
            Assert.AreEqual(3, collisionResults.ResultingBoard.PlayerTwoShip.YPos);
            Assert.AreEqual(FacingDirection.Up, collisionResults.ResultingBoard.PlayerTwoShip.FacingDirection);
        }

        [Test]
        public void Ships_moving_into_a_static_ship_should_bump_it_out_of_its_space()
        {
            var board = new TestSetup()
                .WithShip(
                    new ShipBuilder()
                    .At(2, 2)
                    .Facing(FacingDirection.Down)
                    .Moving(Move.Forward)
                    .GetShip()
                )
                .AndWithShip(
                    new ShipBuilder()
                    .At(2, 3)
                    .Facing(FacingDirection.Up)
                    .NotMoving()
                    .GetShip()
                )
                .GetBoard();
            var collisionResults = CollisionMechanics.Calculate(board);

            Assert.AreEqual(CollisionPhase.PhaseOne, collisionResults.Collisions.Single().CollisionPhase);

            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerOneShip.XPos);
            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerOneShip.YPos);
            Assert.AreEqual(FacingDirection.Down, collisionResults.ResultingBoard.PlayerOneShip.FacingDirection);

            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerTwoShip.XPos);
            Assert.AreEqual(4, collisionResults.ResultingBoard.PlayerTwoShip.YPos);
            Assert.AreEqual(FacingDirection.Up, collisionResults.ResultingBoard.PlayerTwoShip.FacingDirection);
        }

        [Test]
        public void Ships_moving_into_a_static_ship_should_win_the_spot_if_bigger()
        {
            var board = new TestSetup()
                .WithShip(
                    new ShipBuilder()
                    .LargeShip()
                    .At(2, 2)
                    .Facing(FacingDirection.Down)
                    .Moving(Move.Forward)
                    .GetShip()
                )
                .AndWithShip(
                    new ShipBuilder()
                    .At(2, 3)
                    .Facing(FacingDirection.Up)
                    .NotMoving()
                    .GetShip()
                )
                .GetBoard();
            var collisionResults = CollisionMechanics.Calculate(board);

            Assert.AreEqual(CollisionPhase.PhaseOne, collisionResults.Collisions.Single().CollisionPhase);

            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerOneShip.XPos);
            Assert.AreEqual(3, collisionResults.ResultingBoard.PlayerOneShip.YPos);
            Assert.AreEqual(FacingDirection.Down, collisionResults.ResultingBoard.PlayerOneShip.FacingDirection);

            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerTwoShip.XPos);
            Assert.AreEqual(4, collisionResults.ResultingBoard.PlayerTwoShip.YPos);
            Assert.AreEqual(FacingDirection.Up, collisionResults.ResultingBoard.PlayerTwoShip.FacingDirection);
        }

        [Test]
        public void Ships_should_not_be_shoved_onto_rocks()
        {
            var board = new TestSetup()
                .WithShip(
                    new ShipBuilder()
                    .LargeShip()
                    .At(2, 2)
                    .Facing(FacingDirection.Down)
                    .Moving(Move.Forward)
                    .GetShip()
                )
                .AndWithShip(
                    new ShipBuilder()
                    .At(2, 3)
                    .Facing(FacingDirection.Up)
                    .NotMoving()
                    .GetShip()
                )
                .WithARock(RelativeDirection.Behind, Player.Player2)
                .GetBoard();
            var collisionResults = CollisionMechanics.Calculate(board);

            Assert.AreEqual(CollisionPhase.PhaseOne, collisionResults.Collisions.Single().CollisionPhase);

            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerOneShip.XPos);
            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerOneShip.YPos);
            Assert.AreEqual(FacingDirection.Down, collisionResults.ResultingBoard.PlayerOneShip.FacingDirection);

            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerTwoShip.XPos);
            Assert.AreEqual(3, collisionResults.ResultingBoard.PlayerTwoShip.YPos);
            Assert.AreEqual(FacingDirection.Up, collisionResults.ResultingBoard.PlayerTwoShip.FacingDirection);
        }

        [Test]
        public void Ships_should_not_be_shoved_onto_other_ships()
        {
            var board = new TestSetup()
                .WithShip(
                    new ShipBuilder()
                    .LargeShip()
                    .At(2, 2)
                    .Facing(FacingDirection.Down)
                    .Moving(Move.Forward)
                    .GetShip()
                )
                .AndWithShip(
                    new ShipBuilder()
                    .At(2, 3)
                    .Facing(FacingDirection.Up)
                    .NotMoving()
                    .GetShip()
                )
                .AndWithShip(
                    new ShipBuilder()
                    .At(2, 4)
                    .NotMoving()
                    .GetShip()
                )
                .GetBoard();
            var collisionResults = CollisionMechanics.Calculate(board);

            Assert.AreEqual(CollisionPhase.PhaseOne, collisionResults.Collisions.Single().CollisionPhase);

            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerOneShip.XPos);
            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerOneShip.YPos);
            Assert.AreEqual(FacingDirection.Down, collisionResults.ResultingBoard.PlayerOneShip.FacingDirection);

            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerTwoShip.XPos);
            Assert.AreEqual(3, collisionResults.ResultingBoard.PlayerTwoShip.YPos);
            Assert.AreEqual(FacingDirection.Up, collisionResults.ResultingBoard.PlayerTwoShip.FacingDirection);
        }

        [Test]
        public void Ships_should_not_be_able_to_bump_bigger_ships()
        {
            var board = new TestSetup()
               .WithShip(
                   new ShipBuilder()
                   .At(2, 2)
                   .Facing(FacingDirection.Down)
                   .Moving(Move.Forward)
                   .GetShip()
               )
               .AndWithShip(
                   new ShipBuilder()
                   .LargeShip()
                   .At(2, 3)
                   .Facing(FacingDirection.Up)
                   .NotMoving()
                   .GetShip()
               )
               .GetBoard();
            var collisionResults = CollisionMechanics.Calculate(board);

            Assert.AreEqual(CollisionPhase.PhaseOne, collisionResults.Collisions.Single().CollisionPhase);

            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerOneShip.XPos);
            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerOneShip.YPos);
            Assert.AreEqual(FacingDirection.Down, collisionResults.ResultingBoard.PlayerOneShip.FacingDirection);

            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerTwoShip.XPos);
            Assert.AreEqual(3, collisionResults.ResultingBoard.PlayerTwoShip.YPos);
            Assert.AreEqual(FacingDirection.Up, collisionResults.ResultingBoard.PlayerTwoShip.FacingDirection);
        }

        [Test]
        public void Ships_should_not_be_able_to_bump_when_turning_left()
        {
            var board = new TestSetup()
               .WithShip(
                   new ShipBuilder()
                   .At(2, 2)
                   .Facing(FacingDirection.Down)
                   .Moving(Move.Left)
                   .GetShip()
               )
               .AndWithShip(
                   new ShipBuilder()
                   .At(2, 3)
                   .Facing(FacingDirection.Up)
                   .NotMoving()
                   .GetShip()
               )
               .GetBoard();
            var collisionResults = CollisionMechanics.Calculate(board);

            Assert.AreEqual(CollisionPhase.PhaseOne, collisionResults.Collisions.Single().CollisionPhase);

            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerOneShip.XPos);
            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerOneShip.YPos);
            Assert.AreEqual(FacingDirection.Right, collisionResults.ResultingBoard.PlayerOneShip.FacingDirection);

            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerTwoShip.XPos);
            Assert.AreEqual(3, collisionResults.ResultingBoard.PlayerTwoShip.YPos);
            Assert.AreEqual(FacingDirection.Up, collisionResults.ResultingBoard.PlayerTwoShip.FacingDirection);
        }
        
        [Test]
        public void Ships_should_not_be_able_to_bump_when_turning_right()
        {
            var board = new TestSetup()
               .WithShip(
                   new ShipBuilder()
                   .At(2, 2)
                   .Facing(FacingDirection.Down)
                   .Moving(Move.Right)
                   .GetShip()
               )
               .AndWithShip(
                   new ShipBuilder()
                   .At(2, 3)
                   .Facing(FacingDirection.Up)
                   .NotMoving()
                   .GetShip()
               )
               .GetBoard();
            var collisionResults = CollisionMechanics.Calculate(board);

            Assert.AreEqual(CollisionPhase.PhaseOne, collisionResults.Collisions.Single().CollisionPhase);

            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerOneShip.XPos);
            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerOneShip.YPos);
            Assert.AreEqual(FacingDirection.Down, collisionResults.ResultingBoard.PlayerOneShip.FacingDirection);

            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerTwoShip.XPos);
            Assert.AreEqual(3, collisionResults.ResultingBoard.PlayerTwoShip.YPos);
            Assert.AreEqual(FacingDirection.Up, collisionResults.ResultingBoard.PlayerTwoShip.FacingDirection);
        }
        
        [Test]
        public void Ships_contending_the_same_open_space_both_fail_if_the_same_size()
        {
            var board = new TestSetup()
               .WithShip(
                   new ShipBuilder()
                   .At(2, 2)
                   .Facing(FacingDirection.Down)
                   .Moving(Move.Forward)
                   .GetShip()
               )
               .AndWithShip(
                   new ShipBuilder()
                   .At(2, 4)
                   .Facing(FacingDirection.Up)
                   .Moving(Move.Forward)
                   .GetShip()
               )
               .GetBoard();
            var collisionResults = CollisionMechanics.Calculate(board);

            Assert.AreEqual(CollisionPhase.PhaseOne, collisionResults.Collisions.Single().CollisionPhase);

            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerOneShip.XPos);
            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerOneShip.YPos);
            Assert.AreEqual(FacingDirection.Down, collisionResults.ResultingBoard.PlayerOneShip.FacingDirection);

            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerTwoShip.XPos);
            Assert.AreEqual(4, collisionResults.ResultingBoard.PlayerTwoShip.YPos);
            Assert.AreEqual(FacingDirection.Up, collisionResults.ResultingBoard.PlayerTwoShip.FacingDirection);
        }

        [Test]
        [TestCaseAttribute(Player.Player1)]
        [TestCaseAttribute(Player.Player2)]
        public void Ships_that_are_bigger_should_win_contended_spaces(Player playerWithLargeShip)
        {
            var board = new TestSetup()
               .WithShip(
                   new ShipBuilder()
                   .LargeShip(playerWithLargeShip == Player.Player1)
                   .At(2, 2)
                   .Facing(FacingDirection.Down)
                   .Moving(Move.Forward)
                   .GetShip()
               )
               .AndWithShip(
                   new ShipBuilder()
                   .LargeShip(playerWithLargeShip == Player.Player2)
                   .At(2, 4)
                   .Facing(FacingDirection.Up)
                   .Moving(Move.Forward)
                   .GetShip()
               )
               .GetBoard();
            var collisionResults = CollisionMechanics.Calculate(board);

            Assert.AreEqual(CollisionPhase.PhaseOne, collisionResults.Collisions.Single().CollisionPhase);

            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerOneShip.XPos);
            Assert.AreEqual(playerWithLargeShip == Player.Player1 ? 3 : 2, collisionResults.ResultingBoard.PlayerOneShip.YPos);
            Assert.AreEqual(FacingDirection.Down, collisionResults.ResultingBoard.PlayerOneShip.FacingDirection);

            Assert.AreEqual(2, collisionResults.ResultingBoard.PlayerTwoShip.XPos);
            Assert.AreEqual(playerWithLargeShip == Player.Player2 ? 3 : 4, collisionResults.ResultingBoard.PlayerTwoShip.YPos);
            Assert.AreEqual(FacingDirection.Up, collisionResults.ResultingBoard.PlayerTwoShip.FacingDirection);
        }

        [Test]
        [TestCaseAttribute(FacingDirection.Up, 1, 0)]
        [TestCaseAttribute(FacingDirection.Down, 1, 2)]
        [TestCaseAttribute(FacingDirection.Right, 2, 1)]
        [TestCaseAttribute(FacingDirection.Left, 0, 1)]
        public void Ships_moving_forward_should_be_affected_by_their_direction(FacingDirection facing, int expectedXPos, int expectedYPos)
        {
            var board = new TestSetup()
                .WithShip(
                    new ShipBuilder()
                    .At(1, 1)
                    .Facing(facing)
                    .Moving(Move.Forward)
                    .GetShip()
                )
                .GetBoard();
            var collisionResults = CollisionMechanics.Calculate(board);

            Assert.AreEqual(0, collisionResults.Collisions.Count());
            Assert.AreEqual(expectedXPos, collisionResults.ResultingBoard.PlayerOneShip.XPos);
            Assert.AreEqual(expectedYPos, collisionResults.ResultingBoard.PlayerOneShip.YPos);
            Assert.AreEqual(facing, collisionResults.ResultingBoard.PlayerOneShip.FacingDirection);
        }
    }
}
