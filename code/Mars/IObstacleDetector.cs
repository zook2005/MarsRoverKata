using System.Windows;

namespace Mars
{
    public interface IObstacleDetector
    {
        bool IsObstacleDetected(Point obstacleCoords);
    }
}