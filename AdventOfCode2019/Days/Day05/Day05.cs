using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Days.Day05
{
    public class Day05 : IDay
    {
        private List<string> inputs = new List<string>();
        private List<int> intCode;

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

            // test
            //inputs[0] = "1002,4,3,4,33";

            // convert to intCode
            intCode = inputs[0].Split(',').Select(s => Convert.ToInt32(s)).ToList();
        }

        /// <inheritdoc />
        public string Part1()
        {
            IntCodeVM2 vm = new IntCodeVM2(intCode.Select(i => (long)i).ToList());

            vm.AddInput(1);
            vm.ExecuteProgram();

            //return $"{vm.GetMemory(4)}"; // test
            return $"{vm.GetLastOutput()}";

        }

        /// <inheritdoc />
        public string Part2()
        {
            IntCodeVM2 vm = new IntCodeVM2(intCode.Select(i => (long)i).ToList());

            vm.AddInput(5);
            vm.ExecuteProgram();

            return $"{vm.GetLastOutput()}";
        }
    }
}