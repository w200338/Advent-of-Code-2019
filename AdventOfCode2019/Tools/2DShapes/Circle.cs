using AdventOfCode2019.Tools.Vectors;

namespace AdventOfCode2019.Tools._2DShapes
{
    public class Circle
    {
        /// <summary>
        /// Position of the circle
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Radius of the cricle
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// Create a circle from a position and radius
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        public Circle(Vector2 position, double radius)
        {
            Position = position;
            Radius = radius;
        }

        /// <summary>
        /// Check if point is in circle
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsInCircle(Vector2 point)
        {
            return Vector2.Distance(point, Position) <= Radius;
        }
    }
}
