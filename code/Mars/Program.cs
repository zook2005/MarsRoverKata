using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mars
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Mars!");
            Console.WriteLine();
            Console.WriteLine("use ctrl+c to quit at any time!");
            Console.WriteLine();
            try
            {
                var obstacleList = new List<Point>() { new Point(1, 1) };

                var rover = new MarsRover(new Point(0, 0), MarsRover.CardinalDirection.East, new Grid(8, 8), new ObstacleDetector(obstacleList));
                Console.WriteLine($"your {rover.Position}.");
                Console.WriteLine($"Watch out for obstacles" + (obstacleList.Count > 0 ? $" at: {string.Join(",", obstacleList.Select(p => $"({p.ToString()})"))} " : "") + "!");
                Console.WriteLine();

                while (true)
                {
                    Console.WriteLine("enter a list of coma separated commands to move around mars:");
                    var commands = Console.ReadLine();
                    var commandsList = commands.ToCharArray().Where(c => c != ',');
                    rover.Move(commandsList.ToArray());

                    switch (rover.Status.StatusCode)
                    {
                        case RoverStatus.RoverStatusCode.Ok:
                            Console.WriteLine($"rover is in position: {rover.Position}");
                            break;
                        case RoverStatus.RoverStatusCode.Fail:
                            Console.WriteLine($"rover failed to move. rover is in position: {rover.Position}");
                            Console.WriteLine($"rover status is: {rover.Status.StatusMessage}");
                            break;
                        case RoverStatus.RoverStatusCode.Error:
                            Console.WriteLine($"rover encountered an error: {rover.Status.StatusMessage}");
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"fatal error: {e}");
            }

            Console.Write("\nPress any key to continue... ");
            Console.ReadLine();
        }
    }
}
