using System.Collections.Generic;
using System.Windows;

namespace MarsTests
{
    internal class ObstacleDetector : IObstacleDetector
    {
        private List<Point> _obstacleList;

        public ObstacleDetector(List<Point> obstacleList)
        {
            this._obstacleList = obstacleList ?? new List<Point>();
        }

        public bool IsObstacleDetected(Point obstacleCoords)
        {
            return _obstacleList.Contains(obstacleCoords);
        }
    }
}