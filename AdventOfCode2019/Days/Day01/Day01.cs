using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2019.Days.Day01
{
    public class Day01 : IDay
    {
        private List<int> inputs = new List<int>();
        
        /// <inheritdoc />
        public void ReadInput()
        {
            StreamReader reader = null;
            try
            {
                reader = new StreamReader($"Days/{GetType().Name}/input.txt");
                while (!reader.EndOfStream)
                {
                    inputs.Add(Convert.ToInt32(reader.ReadLine()));
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
        }

        /// <inheritdoc />
        public string Part1()
        {
            int output = 0;

            foreach (int input in inputs)
            {
                output += ((int) Math.Floor(input / 3.0)) - 2;
            }

            return $"{output}";
        }

        /// <inheritdoc />
        public string Part2()
        {
            int output = 0;

            foreach (int input in inputs)
            {
                int fuelForPort = RequiredFuel(input);
                output += fuelForPort;

                while (fuelForPort > 0)
                {
                    fuelForPort = RequiredFuel(fuelForPort);
                    output += fuelForPort;
                }
            }

            return output.ToString();
        }

        private int RequiredFuel(int mass)
        {
            int fuel = ((int)Math.Floor(mass / 3.0)) - 2;

            // can't have negative fuel
            if (fuel < 0)
            {
                return 0;
            }
            return fuel;
        }
    }
}
