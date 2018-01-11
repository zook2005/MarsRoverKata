using System.Windows;

namespace MarsTests
{
    internal class MoveForwardCommand : MoveCommand
    {
        public override RoverPosition CalcNewPosition(Point coordinates, MarsRover.CardinalDirection direction)
        {
            Vector move = CalcNextMove(direction);

            var newCoordinates = Point.Add(coordinates, move);

            return new RoverPosition(newCoordinates, direction);
        }
    }
}