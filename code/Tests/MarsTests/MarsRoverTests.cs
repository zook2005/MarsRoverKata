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
            var roverAtNewCoordinates = rover.Move(moves); //implement this new test!

            //Assert
            Assert.AreEqual(expectedCoordinates, roverAtNewCoordinates.coordinates); //rover did not move
            Assert.AreEqual(expectedStartingDirection, roverAtNewCoordinates.direction);//rover did not move
            Assert.AreEqual("obstacle detected", roverAtNewCoordinates.Status.RoverStatus);
            Assert.AreEqual(obstacleCoords, roverAtNewCoordinates.Status.obstacleCoordinates);
        }

        [Test]
        public void TestRoverStateEquality()
        {
            var state1 = new RoverState(new Point(1, 1), MarsRover.CardinalDirection.East);
            var state2 = new RoverState(new Point(1, 1), MarsRover.CardinalDirection.East);
            Assert.IsTrue(state1.GetHashCode().Equals(state2.GetHashCode()));
            Assert.IsTrue(state1.Equals(state2));
        }

        internal class MarsRover
        {
            const int MAXX = 9;
            const int MAXY = 9;

            internal Point coordinates;
            internal CardinalDirection direction;
            private int maxY;
            private int maxX;
            internal List<Point> obstacles = new List<Point>();

            public Status Status { get; internal set; }

            public MarsRover(Point startingPoint, CardinalDirection startingDirection, int maxX = MAXX, int maxY = MAXY)
            {
                this.maxX = maxX;
                this.maxY = maxY;
                this.coordinates = startingPoint;
                this.direction = startingDirection;
            }

            internal MarsRover Move(char[] moves)
            {
                Point coordinates = this.coordinates;
                CardinalDirection direction = this.direction;

                foreach (char commandChar in moves)
                {
                    IRoverCommand command = CommandFactory(commandChar);
                    RoverState newState = command.CalcNewState(coordinates, direction);

                    newState.coordinates.X = newState.coordinates.X % (maxX + 1); // we use modulo to stay in the grid. the grid's size is maxX+1
                    newState.coordinates.Y = newState.coordinates.Y % (maxY + 1); // we use modulo to stay in the grid. the grid's size is maxY+1

                    if (obstacles.Contains(newState.coordinates))
                    {
                        Status status = new Status()
                        {
                            RoverStatus = "obstacle detected",
                            obstacleCoordinates = newState.coordinates
                        };

                        return new MarsRover(coordinates, direction) { Status = status };
                    }

                    direction = newState.direction;
                    coordinates = newState.coordinates;
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
            internal Point obstacleCoordinates;

            internal string RoverStatus;
        }

        internal class RoverState
        {
            internal MarsRover.CardinalDirection direction;
            internal Point coordinates;

            public RoverState(Point coordinates, MarsRover.CardinalDirection direction)
            {
                this.coordinates = coordinates;
                this.direction = direction;
            }

            public override bool Equals(Object obj)
            {
                return obj is RoverState && this == (RoverState)obj;
            }

            public override int GetHashCode()
            {
                return Tuple.Create(coordinates, direction).GetHashCode();
            }

            public static bool operator ==(RoverState x, RoverState y)
            {
                return x.coordinates == y.coordinates && x.direction == y.direction;
            }

            public static bool operator !=(RoverState x, RoverState y)
            {
                return !(x == y);
            }

            public static ReadOnlyDictionary<MarsRover.CardinalDirection, Vector> CardinlDirectionToMoveDictionary = new ReadOnlyDictionary<MarsRover.CardinalDirection, Vector>(new Dictionary<MarsRover.CardinalDirection, Vector>
        {
            { MarsRover.CardinalDirection.North,    new Vector(0, 1)    },
            { MarsRover.CardinalDirection.East,     new Vector(1, 0)    },
            { MarsRover.CardinalDirection.South,    new Vector(0, -1)   },
            { MarsRover.CardinalDirection.West,     new Vector(-1, 0)   }
        });
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
            public override RoverState CalcNewState(Point coordinates, MarsRover.CardinalDirection direction)
            {
                var newDirectaion = CalcNewDirection(direction);
                Vector move = CalcNextMove(coordinates, newDirectaion);

                var newCoordinates = Point.Add(coordinates, move);

                return new RoverState(newCoordinates, newDirectaion);
            }

            protected override MarsRover.CardinalDirection CalcNewDirection(MarsRover.CardinalDirection direction)
            {
                MarsRover.CardinalDirection newDirection = direction; //walking forward, direction has not changed
                return newDirection;
            }
        }
        internal class MoveBackwardCommand : MoveCommand
        {
            public override RoverState CalcNewState(Point coordinates, MarsRover.CardinalDirection direction)
            {
                var newDirectaion = CalcNewDirection(direction);
                Vector move = CalcNextMove(coordinates, newDirectaion);

                var newCoordinates = Point.Add(coordinates, move);

                return new RoverState(newCoordinates, newDirectaion);
            }

            protected override MarsRover.CardinalDirection CalcNewDirection(MarsRover.CardinalDirection direction)
            {
                switch (direction)
                {
                    case MarsRover.CardinalDirection.East:
                        return MarsRover.CardinalDirection.West;
                    case MarsRover.CardinalDirection.South:
                        return MarsRover.CardinalDirection.North;
                    case MarsRover.CardinalDirection.West:
                        return MarsRover.CardinalDirection.East;
                    case MarsRover.CardinalDirection.North:
                        return MarsRover.CardinalDirection.South;
                    default:
                        throw new ArgumentOutOfRangeException($"enum member '{direction}' does not have a corresponding switch case");
                }
            }
        }

        internal abstract class TurnCommand : IRoverCommand
        {
            public RoverState CalcNewState(Point coordinates, MarsRover.CardinalDirection direction)
            {
                var newDirectaion = CalcNewDirection(direction);
                return new RoverState(coordinates, newDirectaion);
            }
            protected abstract MarsRover.CardinalDirection CalcNewDirection(MarsRover.CardinalDirection direction);
        }

        internal abstract class MoveCommand : IRoverCommand
        {
            public abstract RoverState CalcNewState(Point coordinates, MarsRover.CardinalDirection direction);
            protected abstract MarsRover.CardinalDirection CalcNewDirection(MarsRover.CardinalDirection direction);

            protected Vector CalcNextMove(Point coordinates, MarsRover.CardinalDirection newDirectaion)
            {
                Vector newCoordinates = RoverState.CardinlDirectionToMoveDictionary[newDirectaion];
                return newCoordinates;
            }
        }

        internal interface IRoverCommand
        {
            RoverState CalcNewState(Point coordinates, MarsRover.CardinalDirection direction);
        }
    }
}
