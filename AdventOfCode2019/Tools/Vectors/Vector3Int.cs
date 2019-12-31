using System;

namespace AdventOfCode2019.Tools.Vectors
{
    public class Vector3Int
    {
        /// <summary>
        /// Vector with all components 0
        /// </summary>
        public static Vector3Int Zero => new Vector3Int(0, 0, 0);

        /// <summary>
        /// Vector with all components 1
        /// </summary>
        public static Vector3Int One => new Vector3Int(1, 1, 1);

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        /// <summary>
        /// Creates a vector with values 0
        /// </summary>
        public Vector3Int()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        /// <summary>
        /// Create 3D int vector
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3Int(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Calculates distance between two Vector2
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Distance(Vector3Int a, Vector3Int b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2));
        }

        /// <summary>
        /// Calculates manhattan distance between two Vector3Ints
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int DistanceManhattan(Vector3Int a, Vector3Int b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);
        }

        /// <summary>
        /// Calculate dot product of two vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int DotProduct(Vector3Int a, Vector3Int b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        /// <summary>
        /// Add X and Y components of vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3Int operator +(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        /// Subtract X and Y components of vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3Int operator -(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        /// <summary>
        /// Multiply X, Y and Z components with a number
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3Int operator *(Vector3Int a, int b)
        {
            return new Vector3Int(a.X * b, a.Y * b, a.Z * b);
        }

        /// <summary>
        /// Cross product of two vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3Int operator *(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int()
            {
                X = a.Y * b.Z - a.Z * b.Y,
                Y = a.Z * b.X - a.X * b.Z,
                Z = a.X * b.Y - a.Y - b.X
            };
        }

        /// <summary>
        /// Check if two Vectors are equal
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Vector3Int a, Vector3Int b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Check if two Vectors are not equal
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Vector3Int a, Vector3Int b)
        {
            return !(a == b);
        }

        public bool Equals(Vector3Int other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vector3Int) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X;
                hashCode = (hashCode * 397) ^ Y;
                hashCode = (hashCode * 397) ^ Z;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{X} {Y} {Z}";
        }
    }
}
