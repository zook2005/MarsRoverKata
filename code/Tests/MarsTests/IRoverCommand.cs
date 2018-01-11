using System.Windows;

namespace MarsTests
{
    internal interface IRoverCommand
    {
        RoverPosition CalcNewPosition(Point coordinates, MarsRover.CardinalDirection direction);
    }
}