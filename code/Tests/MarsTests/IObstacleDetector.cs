using System.Windows;

namespace MarsTests
{
    internal interface IObstacleDetector
    {
        bool IsObstacleDetected(Point obstacleCoords);
    }
}