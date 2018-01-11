using System;
using System.Windows;

namespace MarsTests
{
    internal class RoverPosition
    {
        internal MarsRover.CardinalDirection Direction { get; private set; }
        internal Point Coordinates { get; private set; }

        public RoverPosition(Point coordinates, MarsRover.CardinalDirection direction)
        {
            this.Coordinates = coordinates;
            this.Direction = direction;
        }

        public override bool Equals(Object obj)
        {
            return obj is RoverPosition && this == (RoverPosition)obj;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Coordinates, Direction).GetHashCode();
        }

        public static bool operator ==(RoverPosition x, RoverPosition y)
        {
            return x.Coordinates == y.Coordinates && x.Direction == y.Direction;
        }

        public static bool operator !=(RoverPosition x, RoverPosition y)
        {
            return !(x == y);
        }
    }
}