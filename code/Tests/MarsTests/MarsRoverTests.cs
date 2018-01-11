using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;

namespace MarsTests
{
    [TestFixture]
    public class MarsRoverTests
    {
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
            var roverAtNewCoordinates = rover.Move(moves);

            //Assert
            Assert.AreEqual(expectedCoordinates, roverAtNewCoordinates.coordinates);
            Assert.AreEqual(expectedStartingDirection, roverAtNewCoordinates.direction);
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
            var roverAtNewCoordinates = rover.Move(moves);

            //Assert
            Assert.AreEqual(expectedCoordinates, roverAtNewCoordinates.coordinates);
            Assert.AreEqual(expectedStartingDirection, roverAtNewCoordinates.direction);
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
            var roverAtNewCoordinates = rover.Move(moves);

            //Assert
            Assert.AreEqual(expectedCoordinates, roverAtNewCoordinates.coordinates);
            Assert.AreEqual(expectedStartingDirection, roverAtNewCoordinates.direction);
        }

        [Test]
        public void CrossingOverEdgeOfGrid_0MaxYf_00()
        {
            //Arrange
            const int MaxY = 9; //so grid's height is 10 squares
            Point startingPoint = new Point(0, MaxY); //(0,9)
            MarsRover.CardinalDirection startingDirection = MarsRover.CardinalDirection.North;
            MarsRover rover = new MarsRover(startingPoint, startingDirection);
            var moves = new[] { 'f' };

            Point expectedCoordinates = new Point(0, 0); //crossing the grid leads to Y coordinate 0

            //Act
            var roverAtNewCoordinates = rover.Move(moves);

            //Assert
            Assert.AreEqual(expectedCoordinates, roverAtNewCoordinates.coordinates);
        }

        [Test]
        public void RespondToObstacle()
        {
            //Arrange
            Point startingPoint = new Point(0, 0);
            MarsRover.CardinalDirection startingDirection = MarsRover.CardinalDirection.North;
            MarsRover rover = new MarsRover(startingPoint, startingDirection);
            var moves = new[] { 'f' };

            Point expectedCoordinates = startingPoint;
            MarsRover.CardinalDirection expectedStartingDirection = startingDirection;

            var obstacleCoords = new Point(0, 1);
            rover.obstacles = new List<Point>() { obstacleCoords };

            //Act
            var roverAtNewCoordinates = rover.Move(moves);

            //Assert
            Assert.AreEqual(expectedCoordinates, roverAtNewCoordinates.coordinates); //rover did not move
            Assert.AreEqual("obstacle detected", roverAtNewCoordinates.Status.RoverStatus);
            Assert.AreEqual(expectedStartingDirection, roverAtNewCoordinates.direction);//rover did not move
            Assert.AreEqual(obstacleCoords, roverAtNewCoordinates.Status.obstacleCoordinates);
            Assert.AreEqual(Status.Code.Fail, roverAtNewCoordinates.Status.code);
        }

        [Test]
        public void TestRoverPositionEquality()
        {
            var position1 = new RoverPosition(new Point(1, 1), MarsRover.CardinalDirection.East);
            var position2 = new RoverPosition(new Point(1, 1), MarsRover.CardinalDirection.East);
            Assert.IsTrue(position1.GetHashCode().Equals(position2.GetHashCode()));
            Assert.IsTrue(position1.Equals(position2));
        }

        internal class MarsRover
        {
            const int MAXX = 9;
            const int MAXY = 9;

            internal Point coordinates { get { return Position.coordinates; } }
            internal CardinalDirection direction { get { return Position.direction; } }
            private int maxY;
            private int maxX;
            internal List<Point> obstacles = new List<Point>();

            public Status Status { get; internal set; }
            public RoverPosition Position { get; private set; }

            public MarsRover(Point startingPoint, CardinalDirection startingDirection, int maxX = MAXX, int maxY = MAXY)
            {
                this.maxX = maxX;
                this.maxY = maxY;
                this.Position = new RoverPosition(startingPoint, startingDirection);
                Status = new Status();
            }

            internal MarsRover Move(char[] moves)
            {
                Point coordinates = this.coordinates;
                CardinalDirection direction = this.direction;

                foreach (char commandChar in moves)
                {
                    IRoverCommand command = CommandFactory(commandChar);
                    RoverPosition newPosition = command.CalcNewPosition(coordinates, direction);

                    newPosition.coordinates.X = newPosition.coordinates.X % (maxX + 1); // we use modulo to stay in the grid. the grid's size is maxX+1
                    newPosition.coordinates.Y = newPosition.coordinates.Y % (maxY + 1); // we use modulo to stay in the grid. the grid's size is maxY+1

                    if (obstacles.Contains(newPosition.coordinates))
                    {
                        Status status = new Status(Status.Code.Fail)
                        {
                            RoverStatus = "obstacle detected",
                            obstacleCoordinates = newPosition.coordinates
                        };

                        return new MarsRover(coordinates, direction) { Status = status };
                    }

                    direction = newPosition.direction;
                    coordinates = newPosition.coordinates;
                }
                return new MarsRover(coordinates, direction);
            }

            internal enum CardinalDirection
            {
                North,
                East,
                South,
                West
            }

            internal IRoverCommand CommandFactory(char command)
            {
                command = char.ToLower(command); //normalizing input
                switch (command)
                {
                    case 'f':
                        return new MoveForwardCommand();
                    case 'b':
                        return new MoveBackwardCommand();
                    case 'r':
                        return new TurnRightCommand();
                    case 'l':
                        return new TurnLeftCommand();
                }
                return null;
            }
        }

        public class Status
        {
            internal string RoverStatus;
            internal Point obstacleCoordinates;
            internal Code code;
            public Status() : this(Code.OK)
            {
                RoverStatus = "OK";
            }
            public Status(Code code)
            {

                this.code = code;
            }
            public enum Code { OK, Fail }
        }

        internal class RoverPosition
        {
            internal MarsRover.CardinalDirection direction;
            internal Point coordinates;

            public RoverPosition(Point coordinates, MarsRover.CardinalDirection direction)
            {
                this.coordinates = coordinates;
                this.direction = direction;
            }

            public override bool Equals(Object obj)
            {
                return obj is RoverPosition && this == (RoverPosition)obj;
            }

            public override int GetHashCode()
            {
                return Tuple.Create(coordinates, direction).GetHashCode();
            }

            public static bool operator ==(RoverPosition x, RoverPosition y)
            {
                return x.coordinates == y.coordinates && x.direction == y.direction;
            }

            public static bool operator !=(RoverPosition x, RoverPosition y)
            {
                return !(x == y);
            }
        }

        internal class TurnRightCommand : TurnCommand
        {
            protected override MarsRover.CardinalDirection CalcNewDirection(MarsRover.CardinalDirection direction)
            {
                switch (direction)
                {
                    case MarsRover.CardinalDirection.North:
                        return MarsRover.CardinalDirection.East;
                    case MarsRover.CardinalDirection.East:
                        return MarsRover.CardinalDirection.South;
                    case MarsRover.CardinalDirection.South:
                        return MarsRover.CardinalDirection.West;
                    case MarsRover.CardinalDirection.West:
                        return MarsRover.CardinalDirection.North;
                    default:
                        throw new ArgumentOutOfRangeException($"enum member '{direction}' does not have a corresponding switch case");
                }
            }
        }

        internal class TurnLeftCommand : TurnCommand
        {
            protected override MarsRover.CardinalDirection CalcNewDirection(MarsRover.CardinalDirection direction)
            {
                switch (direction)
                {
                    case MarsRover.CardinalDirection.East:
                        return MarsRover.CardinalDirection.North;
                    case MarsRover.CardinalDirection.South:
                        return MarsRover.CardinalDirection.East;
                    case MarsRover.CardinalDirection.West:
                        return MarsRover.CardinalDirection.South;
                    case MarsRover.CardinalDirection.North:
                        return MarsRover.CardinalDirection.West;
                    default:
                        throw new ArgumentOutOfRangeException($"enum member '{direction}' does not have a corresponding switch case");
                }
            }
        }

        internal class MoveForwardCommand : MoveCommand
        {
            public override RoverPosition CalcNewPosition(Point coordinates, MarsRover.CardinalDirection direction)
            {
                Vector move = CalcNextMove(coordinates, direction);

                var newCoordinates = Point.Add(coordinates, move);

                return new RoverPosition(newCoordinates, direction);
            }
        }
        internal class MoveBackwardCommand : MoveCommand
        {
            public override RoverPosition CalcNewPosition(Point coordinates, MarsRover.CardinalDirection direction)
            {
                Vector move = CalcNextMove(coordinates, direction);

                var newCoordinates = Point.Subtract(coordinates, move); //we subtract the 'move' cause we are going backward

                return new RoverPosition(newCoordinates, direction);
            }
        }

        internal abstract class TurnCommand : IRoverCommand
        {
            public RoverPosition CalcNewPosition(Point coordinates, MarsRover.CardinalDirection direction)
            {
                var newDirectaion = CalcNewDirection(direction);
                return new RoverPosition(coordinates, newDirectaion);
            }
            protected abstract MarsRover.CardinalDirection CalcNewDirection(MarsRover.CardinalDirection direction);
        }

        internal abstract class MoveCommand : IRoverCommand
        {
            public abstract RoverPosition CalcNewPosition(Point coordinates, MarsRover.CardinalDirection direction);

            protected Vector CalcNextMove(Point coordinates, MarsRover.CardinalDirection newDirectaion)
            {
                Vector newCoordinates = CardinlDirectionToMoveDictionary[newDirectaion];
                return newCoordinates;
            }

            protected static ReadOnlyDictionary<MarsRover.CardinalDirection, Vector> CardinlDirectionToMoveDictionary = new ReadOnlyDictionary<MarsRover.CardinalDirection, Vector>(new Dictionary<MarsRover.CardinalDirection, Vector>
            {
                { MarsRover.CardinalDirection.North,    new Vector(0, 1)    },
                { MarsRover.CardinalDirection.East,     new Vector(1, 0)    },
                { MarsRover.CardinalDirection.South,    new Vector(0, -1)   },
                { MarsRover.CardinalDirection.West,     new Vector(-1, 0)   }
            });
        }

        internal interface IRoverCommand
        {
            RoverPosition CalcNewPosition(Point coordinates, MarsRover.CardinalDirection direction);
        }
    }
}
