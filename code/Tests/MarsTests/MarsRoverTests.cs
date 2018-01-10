using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
