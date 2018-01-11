using System.Windows;

namespace Mars
{
    public class Grid
    {
        const int WIDTH = 10;
        const int HEIGHT = 10;

        private int _width;
        private int _height;

        public Grid(int width = WIDTH, int height = HEIGHT)
        {
            //invalid input
            if (width <= 0 || height <= 0)
            {
                width = WIDTH;
                height = HEIGHT;
            }

            this._width = width;
            this._height = height;
        }

        internal Point NormalizeCoordinates(Point coords)
        {
            var normalizedCoords = new Point();

            // we use modulo twice in order to stay in the grid
            normalizedCoords.X = (coords.X % _width + _width) % _width;
            normalizedCoords.Y = (coords.Y % _height + _height) % _height;

            return normalizedCoords;
        }
    }
}