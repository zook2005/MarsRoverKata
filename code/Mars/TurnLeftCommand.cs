namespace Mars
{
    internal class TurnLeftCommand : TurnCommand
    {
        protected override MarsRover.CardinalDirection CalcNewDirection(MarsRover.CardinalDirection direction)
        {
            switch (direction)
            {
                case MarsRover.CardinalDirection.East:
                    return MarsRover.CardinalDirection.North;
                case MarsRover.CardinalDirection.South:
                    return MarsRover.CardinalDirection.East;
                case MarsRover.CardinalDirection.West:
                    return MarsRover.CardinalDirection.South;
                case MarsRover.CardinalDirection.North:
                    return MarsRover.CardinalDirection.West;
                default:
                    throw new RoverException($"enum member '{direction}' does not have a corresponding switch case");
            }
        }
    }
}