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
        public void MoveMultipleSteps_f_10()
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
    }

    internal class MarsRover
    {
        internal Point coordinates;
        internal CardinalDirection direction;

        public MarsRover(Point startingPoint, CardinalDirection startingDirection)
        {
            this.coordinates = startingPoint;
            this.direction = startingDirection;
        }

        internal MarsRover Move(char[] moves)
        {
            Point coordinates = this.coordinates;
            CardinalDirection direction = this.direction;

            foreach (char commandChar in moves)
            {
                BaseCommand command = CommandFactory(commandChar);
                RoverState newState = command.CalcNewState(coordinates, direction);

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

        internal BaseCommand CommandFactory(char command)
        {
            switch (command)
            {
                case 'f':
                    return new MoveForwardCommand();
            }
            return null;
        }
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

        public static ReadOnlyDictionary<MarsRover.CardinalDirection, Vector> CardinlDirectionToMoveDictionary = new ReadOnlyDictionary<MarsRover.CardinalDirection, Vector>(new Dictionary<MarsRover.CardinalDirection, Vector>
        {
            { MarsRover.CardinalDirection.North,    new Vector(0, 1)    },
            { MarsRover.CardinalDirection.East,     new Vector(1, 0)    },
            { MarsRover.CardinalDirection.South,    new Vector(0, -1)   },
            { MarsRover.CardinalDirection.West,     new Vector(-1, 0)   }
        });
    }

    internal class MoveForwardCommand : BaseCommand
    {
        protected override MarsRover.CardinalDirection CalcNewDirection(MarsRover.CardinalDirection direction)
        {
            MarsRover.CardinalDirection newDirection = direction; //walking forward, direction has not changed
            return newDirection;
        }
    }

    internal abstract class BaseCommand
    {
        public RoverState CalcNewState(Point coordinates, MarsRover.CardinalDirection direction)
        {
            var newDirectaion = CalcNewDirection(direction);
            Vector move = CalcNextMove(coordinates, newDirectaion);

            var newCoordinates = Point.Add(coordinates, move);

            return new RoverState(newCoordinates, newDirectaion);
        }

        protected abstract MarsRover.CardinalDirection CalcNewDirection(MarsRover.CardinalDirection direction);

        protected Vector CalcNextMove(Point coordinates, MarsRover.CardinalDirection newDirectaion)
        {
            Vector newCoordinates = RoverState.CardinlDirectionToMoveDictionary[newDirectaion];
            return newCoordinates;
        }
    }
}
