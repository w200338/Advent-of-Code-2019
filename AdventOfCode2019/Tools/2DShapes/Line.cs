using System;
using AdventOfCode2019.Tools.Vectors;

namespace AdventOfCode2019.Tools._2DShapes
{
    public class Line
    {
        /// <summary>
        /// First point
        /// </summary>
        public Vector2 PointA { get; set; }

        /// <summary>
        /// Second point
        /// </summary>
        public Vector2 PointB { get; set; }

        /// <summary>
        /// Length of line
        /// </summary>
        public double Length => Vector2.Distance(PointA, PointB);

        /// <summary>
        /// Create a line
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Line(Vector2 a, Vector2 b)
        {
            PointA = a;
            PointB = b;
        }

        /// <summary>
        /// Check if point is on line
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsOnLine(Vector2 point)
        {
            /*
             return true anytime C lies perfectly on the line between A en B
             A-C------B
            
            */
            return Math.Abs((Vector2.Distance(PointA, point) + Vector2.Distance(point, PointB)) - Length) < 0.000_001;
        }
    }
}
