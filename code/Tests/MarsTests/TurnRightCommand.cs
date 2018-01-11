namespace MarsTests
{
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
                    throw new RoverException($"enum member '{direction}' does not have a corresponding switch case");
            }
        }
    }
}