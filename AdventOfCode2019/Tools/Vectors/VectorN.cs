﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019.Tools.Vectors
{
    public class VectorN
    {
        /// <summary>
        /// Amount of dimensions this vector contains
        /// </summary>
        public int Dimensions { get; }

        /// <summary>
        /// List of values for each dimension
        /// </summary>
        private List<double> Values { get; }

        /// <summary>
        /// Create a vector of N dimensions
        /// </summary>
        /// <param name="dimensions"></param>
        public VectorN(int dimensions)
        {
            Dimensions = dimensions;

            // make empty list of values
            Values = new List<double>(dimensions);

            for (int i = 0; i < dimensions; i++)
            {
                Values.Add(i);
            }
        }

        /// <summary>
        /// Get value of given dimension
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        public double GetDimension(int dimension)
        {
            if (dimension > 0 && dimension < Dimensions)
            {
                return Values[dimension];
            }

            return 0;
        }

        /// <summary>
        /// Set given dimension to given value
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetDimension(int dimension, double value)
        {
            if (dimension > 0 && dimension < Dimensions)
            {
                Values[dimension] = value;
            }
        }

        /// <summary>
        /// Distance between 2 vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Distance(VectorN a, VectorN b)
        {
            if (a.Dimensions != b.Dimensions)
            {
                return 0;
            }

            double sum = 0;
            for (int i = 0; i < a.Dimensions; i++)
            {
                sum += Math.Pow(a.GetDimension(i) - b.GetDimension(i), 2);
            }
            return Math.Sqrt(sum);
        }

        /// <summary>
        /// Manhattan distance between 2 vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double DistanceManhattan(VectorN a, VectorN b)
        {
            if (a.Dimensions != b.Dimensions)
            {
                return 0;
            }

            double sum = 0;
            for (int i = 0; i < a.Dimensions; i++)
            {
                sum += Math.Abs(a.GetDimension(i) - b.GetDimension(i));
            }
            return sum;
        }

        /// <summary>
        /// Calculate dot product of two vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double DotProduct(VectorN a, VectorN b)
        {
            if (a.Dimensions != b.Dimensions)
            {
                return 0;
            }

            double sum = 0;
            for (int i = 0; i < a.Dimensions; i++)
            {
                sum += a.GetDimension(i) * b.GetDimension(i);
            }
            return sum;
        }

        /// <summary>
        /// Add two vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static VectorN operator +(VectorN a, VectorN b)
        {
            if (a.Dimensions != b.Dimensions)
            {
                return null;
            }

            VectorN output = new VectorN(a.Dimensions);
            for (int i = 0; i < a.Dimensions; i++)
            {
                output.SetDimension(i, a.GetDimension(i) + b.GetDimension(i));
            }
            return output;
        }

        /// <summary>
        /// Subtract two vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static VectorN operator -(VectorN a, VectorN b)
        {
            if (a.Dimensions != b.Dimensions)
            {
                return null;
            }

            VectorN output = new VectorN(a.Dimensions);
            for (int i = 0; i < a.Dimensions; i++)
            {
                output.SetDimension(i, a.GetDimension(i) - b.GetDimension(i));
            }
            return output;
        }

        /// <summary>
        /// Multiply vector with a number
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static VectorN operator *(VectorN a, double b)
        {
            VectorN output = new VectorN(a.Dimensions);
            for (int i = 0; i < a.Dimensions; i++)
            {
                output.SetDimension(i, a.GetDimension(i) * b);
            }
            return output;
        }

        /// <summary>
        /// Cross product of two vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static VectorN operator *(VectorN a, VectorN b)
        {
            if (a.Dimensions != b.Dimensions)
            {
                return null;
            }

            VectorN output = new VectorN(a.Dimensions);
            for (int i = 0; i < a.Dimensions; i++)
            {
                // TODO: calculate cross product
                // https://math.stackexchange.com/questions/2371022/cross-product-in-higher-dimensions/2371039
                // fuck it, not happening
            }
            return output;
        }

        /// <summary>
        /// Check if two Vectors are equal
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(VectorN a, VectorN b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Check if two Vectors are not equal
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(VectorN a, VectorN b)
        {
            return !(a == b);
        }

        public bool Equals(VectorN other)
        {
            // check dimension count
            if (Dimensions != other.Dimensions)
            {
                return false;
            }

            // check if each dimension is close
            for (int i = 0; i < Dimensions; i++)
            {
                // stop after first which differs
                if (Math.Abs(GetDimension(i) - other.GetDimension(i)) > 0.000001)
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VectorN) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Dimensions * 397) ^ (Values != null ? Values.GetHashCode() : 0);
            }
        }
    }
}
