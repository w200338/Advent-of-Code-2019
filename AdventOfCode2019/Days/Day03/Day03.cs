using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2019.Tools._2DShapes;
using AdventOfCode2019.Tools.Vectors;

namespace AdventOfCode2019.Days.Day03
{
    public class Day03 : IDay
    {
        private List<string> inputs = new List<string>();
        private List<LineInt> lines1;
        private List<LineInt> lines2;

        /// <inheritdoc />
        public void ReadInput()
        {
            StreamReader reader = null;
            try
            {
                reader = new StreamReader($"Days/{GetType().Name}/input.txt");
                while (!reader.EndOfStream)
                {
                    inputs.Add(reader.ReadLine());
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                reader?.Close();
            }

            //inputs[0] = "R75,D30,R83,U83,L12,D49,R71,U7,L72";
            //inputs[1] = "U62,R66,U55,R34,D71,R55,D58,R83";

            // split lines into path changes
            List<string> path1 = inputs[0].Split(',').ToList();
            List<string> path2 = inputs[1].Split(',').ToList();

            // parse changes into lines
            lines1 = new List<LineInt>();
            Vector2Int position = Vector2Int.Zero;
            foreach (string s in path1)
            {
                Vector2Int newPosition = ParseChange(position, s);
                lines1.Add(new LineInt(position, newPosition));

                position = newPosition;
            }

            lines2 = new List<LineInt>();
            position = Vector2Int.Zero;
            foreach (string s in path2)
            {
                Vector2Int newPosition = ParseChange(position, s);
                lines2.Add(new LineInt(position, newPosition));

                position = newPosition;
            }
        }

        /// <inheritdoc />
        public string Part1()
        {
            int lowestDistance = int.MaxValue;
            foreach (LineInt line1 in lines1)
            {
                foreach (LineInt line2 in lines2)
                {
                    Vector2Int intersection = LineInt.Intersects(line1, line2);

                    if (intersection != null)
                    {
                        int distance = Vector2Int.DistanceManhattan(Vector2Int.Zero, intersection);
                        if (distance > 0 && distance < lowestDistance)
                        {
                            lowestDistance = distance;
                        }
                    }
                }
            }

            return $"{lowestDistance}";
        }

        /// <inheritdoc />
        public string Part2()
        {
            int lowestCombinedSteps = int.MaxValue;
            for (int i = 0; i < lines1.Count; i++)
            {
                for (int j = 0; j < lines2.Count; j++)
                {
                    Vector2Int intersection = LineInt.Intersects(lines1[i], lines2[j]);

                    if (intersection != null)
                    {
                        int distance = Vector2Int.DistanceManhattan(Vector2Int.Zero, intersection);
                        if (distance > 0)
                        {
                            int steps = Steps(i, j);
                            steps += Vector2Int.DistanceManhattan(intersection, lines1[i].PointA);
                            steps += Vector2Int.DistanceManhattan(intersection, lines2[j].PointA);

                            if (steps != 0 && steps < lowestCombinedSteps)
                            {
                                lowestCombinedSteps = steps;
                                Console.WriteLine($"new lowest: {lowestCombinedSteps}");
                            }
                        }
                    }
                }
            }

            return $"{lowestCombinedSteps}";
        }

        /// <summary>
        /// Parse change in direction
        /// </summary>
        /// <param name="currentPos"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private Vector2Int ParseChange(Vector2Int currentPos, string line)
        {
            int length = Convert.ToInt32(line.Substring(1));

            switch (line[0])
            {
                case 'L':
                    return currentPos + new Vector2Int(-length, 0);

                case 'R':
                    return currentPos + new Vector2Int(+length, 0);

                case 'U':
                    return currentPos + new Vector2Int(0, -length);

                case 'D':
                    return currentPos + new Vector2Int(0, +length);
            }

            return Vector2Int.Zero;
        }

        /// <summary>
        /// Steps taken to reach a point on the path
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <returns></returns>
        private int Steps(int index1, int index2)
        {
            int totalSteps = 0;

            for (int i = 0; i < index1; i++)
            {
                totalSteps += lines1[i].LengthManhattan;
            }

            for (int i = 0; i < index2; i++)
            {
                totalSteps += lines2[i].LengthManhattan;
            }

            return totalSteps;
        }
    }
}