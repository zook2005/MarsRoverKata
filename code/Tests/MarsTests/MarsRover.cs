using System.Windows;

namespace MarsTests
{
    internal class MarsRover
    {
        internal Point Coordinates { get { return Position.Coordinates; } }
        internal CardinalDirection Direction { get { return Position.Direction; } }
        private Grid _map;

        public RoverStatus Status { get; private set; }
        public RoverPosition Position { get; private set; }
        public IObstacleDetector ObstacleDetector { get; private set; }

        public MarsRover(Point startingPoint, CardinalDirection startingDirection, Grid map = null, IObstacleDetector obstacleDetector = null)
        {
            this._map = map ?? new Grid();
            this.ObstacleDetector = obstacleDetector;
            Status = new RoverStatus();

            var normalizedStartingPoint = this._map.NormalizeCoordinates(startingPoint);
            this.Position = new RoverPosition(normalizedStartingPoint, startingDirection);
        }

        internal void Move(char[] moves)
        {
            try
            {
                foreach (char commandChar in moves)
                {
                    IRoverCommand command = CommandFactory(commandChar);
                    if (command == null) throw new RoverException($"invalid command character: {command}!");

                    RoverPosition newPosition = command.CalcNewPosition(this.Coordinates, this.Direction);

                    var normalizeCoords = _map.NormalizeCoordinates(newPosition.Coordinates);
                    RoverPosition normalizePosition = new RoverPosition(normalizeCoords, newPosition.Direction);

                    if (ObstacleDetector != null && ObstacleDetector.IsObstacleDetected(normalizePosition.Coordinates))
                    {
                        RoverStatus status = new RoverStatus(RoverStatus.RoverStatusCode.Fail)
                        {
                            StatusMessage = $"obstacle detected at: {Coordinates}",
                        };

                        this.Status = status;
                        break; //don't move and report error by changing own status
                    }

                    this.Position = normalizePosition;
                }
            }
            catch (RoverException e)
            {
                this.Status = new RoverStatus(RoverStatus.RoverStatusCode.Error)
                {
                    StatusMessage = e.Message
                };
            }
        }

        internal enum CardinalDirection
        {
            North,
            East,
            South,
            West
        }

        internal IRoverCommand CommandFactory(char command)
        {
            command = char.ToLower(command); //normalizing input
            switch (command)
            {
                case 'f':
                    return new MoveForwardCommand();
                case 'b':
                    return new MoveBackwardCommand();
                case 'r':
                    return new TurnRightCommand();
                case 'l':
                    return new TurnLeftCommand();
            }
            return null;
        }
    }
}