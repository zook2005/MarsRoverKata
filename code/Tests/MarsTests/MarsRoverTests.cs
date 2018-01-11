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
            Assert.AreEqual("obstacle detected", rover.Status.StatusMessage);
            Assert.AreEqual(obstacleCoords, rover.Status.ObstacleCoordinates);
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

        internal class MarsRover
        {
            internal Point Coordinates { get { return Position.Coordinates; } }
            internal CardinalDirection Direction { get { return Position.Direction; } }
            private Grid _map;

            public RoverStatus Status { get; private set; }
            public RoverPosition Position { get; private set; }
            public IObstacleDetector ObstacleDetector { get; internal set; }

            public MarsRover(Point startingPoint, CardinalDirection startingDirection, Grid map = null, IObstacleDetector obstacleDetector = null)
            {
                this.Position = new RoverPosition(startingPoint, startingDirection);
                this._map = map ?? new Grid();
                this.ObstacleDetector = obstacleDetector;
                Status = new RoverStatus();
            }

            internal void Move(char[] moves)
            {
                foreach (char commandChar in moves)
                {
                    IRoverCommand command = CommandFactory(commandChar);

                    RoverPosition newPosition = command.CalcNewPosition(this.Coordinates, this.Direction);

                    var normalizeCoords = _map.NormalizeCoordinates(newPosition.Coordinates);
                    RoverPosition normalizePosition = new RoverPosition(normalizeCoords, newPosition.Direction);

                    if (ObstacleDetector != null && ObstacleDetector.IsObstacleDetected(normalizePosition.Coordinates))
                    {
                        RoverStatus status = new RoverStatus(RoverStatus.RoverStatusCode.Fail)
                        {
                            StatusMessage = "obstacle detected",
                            ObstacleCoordinates = normalizePosition.Coordinates
                        };

                        this.Status = status;
                        break; //don't move and report error by changing own status
                    }

                    this.Position = normalizePosition;
                }
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

        public class RoverStatus
        {
            public enum RoverStatusCode { Ok, Fail }

            internal string StatusMessage { get; set; }
            internal Point ObstacleCoordinates { get; set; }
            internal RoverStatusCode StatusCode { get; set; }

            public RoverStatus() : this(RoverStatusCode.Ok)
            {
                StatusMessage = "OK";
            }

            public RoverStatus(RoverStatusCode code)
            {
                this.StatusCode = code;
            }
        }

        internal class RoverPosition
        {
            internal MarsRover.CardinalDirection Direction { get; private set; }
            internal Point Coordinates { get; private set; }

            public RoverPosition(Point coordinates, MarsRover.CardinalDirection direction)
            {
                this.Coordinates = coordinates;
                this.Direction = direction;
            }

            public override bool Equals(Object obj)
            {
                return obj is RoverPosition && this == (RoverPosition)obj;
            }

            public override int GetHashCode()
            {
                return Tuple.Create(Coordinates, Direction).GetHashCode();
            }

            public static bool operator ==(RoverPosition x, RoverPosition y)
            {
                return x.Coordinates == y.Coordinates && x.Direction == y.Direction;
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
                return new RoverPosition(coordinates, newDirectaion); //rover only turns
            }
            protected abstract MarsRover.CardinalDirection CalcNewDirection(MarsRover.CardinalDirection direction);
        }

        internal abstract class MoveCommand : IRoverCommand
        {
            public abstract RoverPosition CalcNewPosition(Point coordinates, MarsRover.CardinalDirection direction);

            protected Vector CalcNextMove(Point coordinates, MarsRover.CardinalDirection newDirectaion)
            {
                Vector newCoordinates = CardinalDirectionToMoveDictionary[newDirectaion];
                return newCoordinates;
            }

            protected static ReadOnlyDictionary<MarsRover.CardinalDirection, Vector> CardinalDirectionToMoveDictionary = new ReadOnlyDictionary<MarsRover.CardinalDirection, Vector>(new Dictionary<MarsRover.CardinalDirection, Vector>
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

    [Serializable]
    internal class RoverException : Exception
    {
        public RoverException()
        {
        }

        public RoverException(string message) : base(message)
        {
        }

        public RoverException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RoverException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    internal class Grid
    {
        const int WIDTH = 10;
        const int HEIGHT = 10;

        private int _width;
        private int _height;

        public Grid(int width = WIDTH, int height = HEIGHT)
        {
            this._width = width;
            this._height = height;
        }

        internal Point NormalizeCoordinates(Point coords)
        {
            var normalizedCoords = new Point();

            // we use modulo to stay in the grid
            normalizedCoords.X = coords.X % (_width);
            normalizedCoords.Y = coords.Y % (_height);

            return normalizedCoords;
        }
    }

    internal class ObstacleDetector : IObstacleDetector
    {
        private List<Point> _obstacleList;

        public ObstacleDetector(List<Point> obstacleList)
        {
            this._obstacleList = obstacleList ?? new List<Point>();
        }

        public bool IsObstacleDetected(Point obstacleCoords)
        {
            return _obstacleList.Contains(obstacleCoords);
        }
    }

    internal interface IObstacleDetector
    {
        bool IsObstacleDetected(Point obstacleCoords);
    }
}
