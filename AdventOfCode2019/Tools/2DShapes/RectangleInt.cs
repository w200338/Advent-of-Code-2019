using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2019.Tools.Vectors;

namespace AdventOfCode2019.Tools._2DShapes
{
    public class RectangleInt
    {
        /// <summary>
        /// Position of Rectangle
        /// </summary>
        public Vector2Int Position { get; set; }

        /// <summary>
        /// Size of rectangle
        /// </summary>
        public Vector2Int Size { get; set; }

        /// <summary>
        /// Create a rectangle
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        public RectangleInt(Vector2Int position, Vector2Int size)
        {
            Position = position;
            Size = size;
        }

        /// <summary>
        /// Point is in rectangle
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsInRectangle(Vector2Int point)
        {
            return point.X >= Position.X && point.X <= Position.X + Size.X && point.Y >= Position.Y && point.Y <= Position.Y + Size.Y;
        }
    }
}
