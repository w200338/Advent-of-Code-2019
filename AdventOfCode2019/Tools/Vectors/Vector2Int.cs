using System;

namespace AdventOfCode2019.Tools.Vectors
{
    public class Vector2Int
    {
        /// <summary>
        /// Vector with all components 0
        /// </summary>
        public static Vector2Int Zero => new Vector2Int(0, 0);

        /// <summary>
        /// Vector with all components 1
        /// </summary>
        public static Vector2Int One => new Vector2Int(1, 1);

        public int X { get; set; }
        public int Y { get; set; }

        /// <summary>
        /// Create 2D vector
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Calculates distance between two Vector2Ints
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Distance(Vector2Int a, Vector2Int b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        /// <summary>
        /// Calculates manhattan distance between two Vector2Ints
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int DistanceManhattan(Vector2Int a, Vector2Int b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        /// <summary>
        /// Calculate dot product of two vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int DotProduct(Vector2Int a, Vector2Int b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        /// <summary>
        /// Add X and Y components of vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Subtract X and Y components of vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector2Int operator -(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Multiply X and Y components with a number
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector2Int operator *(Vector2Int a, int b)
        {
            return new Vector2Int(a.X * b, a.Y * b);
        }

        /// <summary>
        /// Create a vector from an angle in degrees
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector2Int FromAngle(double angle)
        {
            return new Vector2Int((int) Math.Cos(angle), (int) Math.Sin(angle));
        }

        /// <summary>
        /// Check if two Vectors are equal
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Vector2Int a, Vector2Int b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Check if two Vectors are not equal
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Vector2Int a, Vector2Int b)
        {
            return !(a == b);
        }

        public bool Equals(Vector2Int other)
        {
            return X == other?.X && Y == other.Y;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vector2Int) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public override string ToString()
        {
            return $"{X} {Y}";
        }
    }
}
