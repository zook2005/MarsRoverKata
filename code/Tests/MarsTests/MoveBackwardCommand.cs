using System.Windows;

namespace MarsTests
{
    internal class MoveBackwardCommand : MoveCommand
    {
        public override RoverPosition CalcNewPosition(Point coordinates, MarsRover.CardinalDirection direction)
        {
            Vector move = CalcNextMove(direction);

            var newCoordinates = Point.Subtract(coordinates, move); //we subtract the 'move' cause we are going backward

            return new RoverPosition(newCoordinates, direction);
        }
    }
}