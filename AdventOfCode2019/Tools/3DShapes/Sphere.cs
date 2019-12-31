using AdventOfCode2019.Tools.Vectors;

namespace AdventOfCode2019.Tools._3DShapes
{
    public class Sphere
    {
        public Vector3 Position { get; set; }
        public double Radius { get; set; }

        /// <summary>
        /// Create sphere from a position and radius
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        public Sphere(Vector3 position, double radius)
        {
            this.Position = position;
            this.Radius = radius;
        }

        /// <summary>
        /// Create sphere from position and radius
        /// </summary>
        /// <param name="z"></param>
        /// <param name="radius"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Sphere(double x, double y, double z, double radius)
        {
            this.Position = new Vector3(x, y, z);
            this.Radius = radius;
        }

        /// <summary>
        /// Is point in sphere
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsInSphere(Vector3 point)
        {
            return Vector3.Distance(Position, point) < Radius;
        }
    }
}
