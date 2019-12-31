using AdventOfCode2019.Tools.Vectors;

namespace AdventOfCode2019.Tools._2DShapes
{
    public class Rectangle
    {
        /// <summary>
        /// Position of Rectangle
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Size of rectangle
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        /// Create a rectangle
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        public Rectangle(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        /// <summary>
        /// Point is in rectangle
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsInRectangle(Vector2 point)
        {
            return point.X >= Position.X && point.X <= Position.X + Size.X && point.Y >= Position.Y && point.Y <= Position.Y + Size.Y;
        }
    }
}
