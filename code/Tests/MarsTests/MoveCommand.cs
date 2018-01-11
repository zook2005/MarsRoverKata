using System.Windows;

namespace MarsTests
{
    internal abstract class MoveCommand : IRoverCommand
    {
        public abstract RoverPosition CalcNewPosition(Point coordinates, MarsRover.CardinalDirection direction);

        protected Vector CalcNextMove(MarsRover.CardinalDirection direction)
        {
            switch (direction)
            {
                case MarsRover.CardinalDirection.North:
                    return new Vector(0, 1);
                case MarsRover.CardinalDirection.East:
                    return new Vector(1, 0);
                case MarsRover.CardinalDirection.South:
                    return new Vector(0, -1);
                case MarsRover.CardinalDirection.West:
                    return new Vector(-1, 0);
                default:
                    throw new RoverException($"enum member '{direction}' does not have a corresponding switch case");
            }
        }
    }
}