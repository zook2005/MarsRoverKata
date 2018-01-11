using System.Windows;

namespace Mars
{
    internal interface IObstacleDetector
    {
        bool IsObstacleDetected(Point obstacleCoords);
    }
}