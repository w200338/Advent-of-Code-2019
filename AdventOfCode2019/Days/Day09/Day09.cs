using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2019.Days.Day09
{
    public class Day09 : IDay
    {
        private List<string> inputs = new List<string>();
        private List<long> program;

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
            //inputs[0] = "104,1125899906842624,99";
            //inputs[0] = "1102,34915192,34915192,7,4,7,99,0";
            //inputs[0] = "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99";

            program = inputs[0].Split(',').Select(long.Parse).ToList();
        }

        /// <inheritdoc />
        public string Part1()
        {
            //IntCodeVM intCodeVM = new IntCodeVM(program);
            IntCodeVM2 intCodeVM = new IntCodeVM2(program);
            //intCodeVM.ResizeMemory(int.MaxValue >> 4);
            intCodeVM.AddInput(1);

            intCodeVM.ExecuteProgram();

            return $"{intCodeVM.GetLastOutput()}";

            // 2158221668 too low
            // 1187721666102244 is too high
        }

        /// <inheritdoc />
        public string Part2()
        {
            IntCodeVM intCodeVM = new IntCodeVM(program);
            intCodeVM.ResizeMemory(int.MaxValue >> 4);
            intCodeVM.AddInput(2);

            intCodeVM.ExecuteProgram();

            return $"{intCodeVM.GetLastOutput()}";
        }
    }
}