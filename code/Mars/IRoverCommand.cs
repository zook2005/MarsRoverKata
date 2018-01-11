using System.Windows;

namespace Mars
{
    internal interface IRoverCommand
    {
        RoverPosition CalcNewPosition(Point coordinates, MarsRover.CardinalDirection direction);
    }
}