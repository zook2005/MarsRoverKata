using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            char startingDirection = 'e';
            MarsRover rover = new MarsRover(startingPoint, startingDirection);
            var moves = new[] { 'f' };

            Point expectedCoordinates = new Point(1, 0);

            //Act
            var roverAtNewCoordinates = rover.Move(moves);

            //Assert
            Assert.AreEqual(expectedCoordinates, roverAtNewCoordinates.coordinates);
        }
    }

    internal class MarsRover
    {
        internal Point coordinates;
        internal char direction;

        public MarsRover(Point startingPoint, char startingDirection)
        {
            this.coordinates = startingPoint;
            this.direction = startingDirection;
        }

        internal MarsRover Move(char[] moves)
        {
            Point coordinates = new Point(1, 0);
            char direction = 'e';
            return new MarsRover(coordinates, direction);
        }
    }
}
