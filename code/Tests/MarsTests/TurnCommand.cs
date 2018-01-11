using System.Windows;

namespace MarsTests
{
    internal abstract class TurnCommand : IRoverCommand
    {
        public RoverPosition CalcNewPosition(Point coordinates, MarsRover.CardinalDirection direction)
        {
            var newDirectaion = CalcNewDirection(direction);
            return new RoverPosition(coordinates, newDirectaion); //rover only turns
        }
        protected abstract MarsRover.CardinalDirection CalcNewDirection(MarsRover.CardinalDirection direction);
    }
}