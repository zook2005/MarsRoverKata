using NUnit.Framework;
using System.Collections.Generic;
using System.Windows;
using Mars;

namespace MarsTests
{
    [TestFixture]
    public class MarsRoverTests
    {
        [Test]
        public void TestNewMoveFunction()
        {
            //Arrange
            Point startingPoint = new Point(0, 0);
            MarsRover.CardinalDirection startingDirection = MarsRover.CardinalDirection.East;
            MarsRover rover = new MarsRover(startingPoint, startingDirection);
            var moves = new[] { 'f' };

            Point expectedCoordinates = new Point(1, 0);
            MarsRover.CardinalDirection expectedStartingDirection = MarsRover.CardinalDirection.East;

            //Act
            rover.Move(moves);

            //Assert
            Assert.AreEqual(expectedCoordinates, rover.Coordinates);
            Assert.AreEqual(expectedStartingDirection, rover.Direction);
            Assert.AreEqual(RoverStatus.RoverStatusCode.Ok, rover.Status.StatusCode);
        }

        [Test]
        public void MoveOneStepForward_f_10()
        {
            //Arrange
            Point startingPoint = new Point(0, 0);
            MarsRover.CardinalDirection startingDirection = MarsRover.CardinalDirection.East;
            MarsRover rover = new MarsRover(startingPoint, startingDirection);
            var moves = new[] { 'f' };

            Point expectedCoordinates = new Point(1, 0);
            MarsRover.CardinalDirection expectedStartingDirection = MarsRover.CardinalDirection.East;

            //Act
            rover.Move(moves);

            //Assert
            Assert.AreEqual(expectedCoordinates, rover.Coordinates);
            Assert.AreEqual(expectedStartingDirection, rover.Direction);
            Assert.AreEqual(RoverStatus.RoverStatusCode.Ok, rover.Status.StatusCode);
        }

        [Test]
        public void MoveOneStepBacward_b_00()
        {
            //Arrange
            Point startingPoint = new Point(0, 1);
            MarsRover.CardinalDirection startingDirection = MarsRover.CardinalDirection.North;
            MarsRover rover = new MarsRover(startingPoint, startingDirection);
            var moves = new[] { 'b' };

            Point expectedCoordinates = new Point(0, 0);
            MarsRover.CardinalDirection expectedStartingDirection = MarsRover.CardinalDirection.North;

            //Act
            rover.Move(moves);

            //Assert
            Assert.AreEqual(expectedCoordinates, rover.Coordinates);
            Assert.AreEqual(expectedStartingDirection, rover.Direction);
            Assert.AreEqual(RoverStatus.RoverStatusCode.Ok, rover.Status.StatusCode);
        }

        [Test]
        public void TurnRight_00N_00E()
        {
            //Arrange
            Point startingPoint = new Point(0, 0);
            MarsRover.CardinalDirection startingDirection = MarsRover.CardinalDirection.North;
            MarsRover rover = new MarsRover(startingPoint, startingDirection);
            var moves = new[] { 'r' };

            Point expectedCoordinates = new Point(0, 0);
            MarsRover.CardinalDirection expectedStartingDirection = MarsRover.CardinalDirection.East;

            //Act
            rover.Move(moves);

            //Assert
            Assert.AreEqual(expectedCoordinates, rover.Coordinates);
            Assert.AreEqual(expectedStartingDirection, rover.Direction);
            Assert.AreEqual(RoverStatus.RoverStatusCode.Ok, rover.Status.StatusCode);
        }

        [Test]
        public void CrossingOverEdgeOfGrid_0MaxYf_00()
        {
            //Arrange
            const int width = 10;
            const int height = 10;

            Point startingPoint = new Point(0, width - 1); //(0,9)
            MarsRover.CardinalDirection startingDirection = MarsRover.CardinalDirection.North;

            Grid map = new Grid(width, height);
            MarsRover rover = new MarsRover(startingPoint, startingDirection, map, null);
            var moves = new[] { 'f' };

            Point expectedCoordinates = new Point(0, 0); //crossing the grid leads to Y coordinate 0

            //Act
            rover.Move(moves);

            //Assert
            Assert.AreEqual(expectedCoordinates, rover.Coordinates);
            Assert.AreEqual(RoverStatus.RoverStatusCode.Ok, rover.Status.StatusCode);
        }
        /// <summary>
        /// input: coords:(0,0) ; direction: North ; command: 'b'
        /// expected output: coords:(0,MaxY) 
        /// </summary>
        [Test]
        public void CrossingOverEdgeOfGrid_00Nb_0MaxY()
        {
            //Arrange
            const int width = 10;
            const int height = 10;

            Point startingPoint = new Point(0, 0); //(0,9)
            MarsRover.CardinalDirection startingDirection = MarsRover.CardinalDirection.North;

            Grid map = new Grid(width, height);
            MarsRover rover = new MarsRover(startingPoint, startingDirection, map, null);
            var moves = new[] { 'b' };

            Point expectedCoordinates = new Point(0, width - 1); //crossing the grid leads to Y coordinate 0

            //Act
            rover.Move(moves);

            //Assert
            Assert.AreEqual(expectedCoordinates, rover.Coordinates);
            Assert.AreEqual(RoverStatus.RoverStatusCode.Ok, rover.Status.StatusCode);
        }

        [Test]
        public void RespondToObstacle()
        {
            //Arrange
            Point startingPoint = new Point(0, 0);
            var obstacleCoords = new Point(0, 1);
            var obstacles = new List<Point>() { obstacleCoords };
            MarsRover.CardinalDirection startingDirection = MarsRover.CardinalDirection.North;
            var ObstacleDetector = new ObstacleDetector(obstacles);
            MarsRover rover = new MarsRover(startingPoint, startingDirection, null, ObstacleDetector);

            var moves = new[] { 'f' };

            Point expectedCoordinates = startingPoint;
            MarsRover.CardinalDirection expectedStartingDirection = startingDirection;

            //Act
            rover.Move(moves);

            //Assert
            Assert.AreEqual(expectedCoordinates, rover.Coordinates); //rover did not move
            Assert.AreEqual(expectedStartingDirection, rover.Direction);//rover did not move
            Assert.AreEqual($"obstacle detected at: ({obstacleCoords})", rover.Status.StatusMessage);
            Assert.AreEqual(RoverStatus.RoverStatusCode.Fail, rover.Status.StatusCode);
        }

        [Test]
        public void TestRoverPositionEquality()
        {
            var position1 = new RoverPosition(new Point(1, 1), MarsRover.CardinalDirection.East);
            var position2 = new RoverPosition(new Point(1, 1), MarsRover.CardinalDirection.East);
            Assert.IsTrue(position1.GetHashCode().Equals(position2.GetHashCode()));
            Assert.IsTrue(position1.Equals(position2));
        }

        [Test]
        public void DetectObstacleTest()
        {
            //Arrange
            var obstacleCoords = new Point(0, 1);
            IObstacleDetector obstacleDetector = new ObstacleDetector(new List<Point>() { obstacleCoords });

            //Act
            var obstacleDetected = obstacleDetector.IsObstacleDetected(obstacleCoords);

            //Assert
            Assert.IsTrue(obstacleDetected, $"failed to identify obstacle at coordinates: {obstacleCoords}");
        }

        [Test]
        public void NormalizeCoordinatesTest()
        {
            //Arrange
            const int width = 10;
            const int height = 10;

            Grid map = new Grid(width, height);
            Point expectedCoordinates = new Point(0, 1);

            //Act
            Point normalizedCoords = map.NormalizeCoordinates(new Point(width, height + 1));

            //Assert
            Assert.AreEqual(expectedCoordinates, normalizedCoords);
        }

        [Test]
        public void NormalizeCoordinatesTest_NegativeCoord()
        {
            //Arrange
            const int width = 10;
            const int height = 10;

            Grid map = new Grid(width, height);
            Point expectedCoordinates = new Point(0, 9);

            //Act
            Point normalizedCoords = map.NormalizeCoordinates(new Point(width, -1));

            //Assert
            Assert.AreEqual(expectedCoordinates, normalizedCoords);
        }

        [Test]
        public void CommandFactory_InvalidCommand_Null()
        {
            //Arrange
            Point startingPoint = new Point(0, 0);
            MarsRover.CardinalDirection startingDirection = MarsRover.CardinalDirection.North;
            MarsRover rover = new MarsRover(startingPoint, startingDirection, null, null);

            //Act
            IRoverCommand cmd = rover.CommandFactory('?');

            //Assert
            Assert.IsNull(cmd);
        }

        [Test]
        public void Move_InvalidCommand_Error()
        {
            //Arrange
            Point startingPoint = new Point(0, 0);
            MarsRover.CardinalDirection startingDirection = MarsRover.CardinalDirection.North;
            MarsRover rover = new MarsRover(startingPoint, startingDirection, null, null);

            //Act
            rover.Move(new[] { '?' });

            //Assert
            Assert.AreEqual(RoverStatus.RoverStatusCode.Error, rover.Status.StatusCode);
        }
    }
}