using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2019.Tools.Vectors;

namespace AdventOfCode2019.Tools._2DShapes
{
    public class LineInt
    {
        /// <summary>
        /// First point
        /// </summary>
        public Vector2Int PointA { get; set; }

        /// <summary>
        /// Second point
        /// </summary>
        public Vector2Int PointB { get; set; }

        /// <summary>
        /// Length of line
        /// </summary>
        public double Length => Vector2Int.Distance(PointA, PointB);

        /// <summary>
        /// Manhattan distance between points
        /// </summary>
        public int LengthManhattan => Vector2Int.DistanceManhattan(PointA, PointB);

        /// <summary>
        /// Create a line
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public LineInt(Vector2Int a, Vector2Int b)
        {
            PointA = a;
            PointB = b;
        }

        /// <summary>
        /// Check if point is on line
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsOnLine(Vector2Int point)
        {
            /*
             return true anytime C lies perfectly on the line between A en B
             A-C------B
            
            */
            return Math.Abs((Vector2Int.Distance(PointA, point) + Vector2Int.Distance(point, PointB)) - Length) < 0.000_001;
        }

        /// <summary>
        /// Where do two lines intersect?
        /// </summary>
        /// <param name="AB"></param>
        /// <param name="CD"></param>
        /// <returns></returns>
        public static Vector2Int Intersects(LineInt AB, LineInt CD)
        {
            double deltaACy = AB.PointA.Y - CD.PointA.Y;
            double deltaDCx = CD.PointB.X - CD.PointA.X;
            double deltaACx = AB.PointA.X - CD.PointA.X;
            double deltaDCy = CD.PointB.Y - CD.PointA.Y;
            double deltaBAx = AB.PointB.X - AB.PointA.X;
            double deltaBAy = AB.PointB.Y - AB.PointA.Y;

            double denominator = deltaBAx * deltaDCy - deltaBAy * deltaDCx;
            double numerator = deltaACy * deltaDCx - deltaACx * deltaDCy;

            if (denominator == 0)
            {
                if (numerator == 0)
                {
                    // collinear. Potentially infinite intersection points.
                    // Check and return one of them.
                    if (AB.PointA.X >= CD.PointA.X && AB.PointA.X <= CD.PointB.X)
                    {
                        return AB.PointA;
                    }
                    else if (CD.PointA.X >= AB.PointA.X && CD.PointA.X <= AB.PointB.X)
                    {
                        return CD.PointA;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                { // parallel
                    return null;
                }
            }

            double r = numerator / denominator;
            if (r < 0 || r > 1)
            {
                return null;
            }

            double s = (deltaACy * deltaBAx - deltaACx * deltaBAy) / denominator;
            if (s < 0 || s > 1)
            {
                return null;
            }

            return new Vector2Int((int)(AB.PointA.X + r * deltaBAx), (int)(AB.PointA.Y + r * deltaBAy));
        }
    }
}
