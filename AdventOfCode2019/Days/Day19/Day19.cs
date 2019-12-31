using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using  AdventOfCode2019.Tools.Vectors;

namespace AdventOfCode2019.Days.Day19
{
    public class Day19 : IDay
    {
        private List<string> inputs = new List<string>();
        private List<long> program = new List<long>();

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

            program = inputs[0].Split(',').Select(long.Parse).ToList();
        }

        /// <inheritdoc />
        public string Part1()
        {
            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.ExecuteProgram();

            List<List<char>> area = new List<List<char>>();

            int total = 0;
            for (int i = 0; i < 50; i++)
            {
                area.Add(new List<char>());

                for (int j = 0; j < 50; j++)
                {
                    vm.Reset();
                    vm.AddInput(j); // x
                    vm.AddInput(i); // y

                    vm.ResumeProgram();

                    if (vm.GetLastOutput() == 0)
                    {
                        area[i].Add('.');
                        Console.Write('.');
                    }
                    else
                    {
                        area[i].Add('#');
                        Console.Write('#');
                        total++;
                    }
                }

                Console.WriteLine();
            }

            return $"{total} points are affected";
        }

        /// <inheritdoc />
        public string Part2()
        {
            IntCodeVM2 vm = new IntCodeVM2(program);

            // walk along top edge
            int x = 100; // offset 
            int y = 0;
            while (true)
            {
                if (IsInTractorBeam(x, y, vm)) // check top-right
                {
                    if (IsInTractorBeam(x - 99, y + 99, vm)) // check bottom left
                    {
                        return $"{(x - 99) * 10_000 + y}";
                    }

                    x++; // move right
                }
                else
                {
                    y++; // move down
                }
            }

            /* way too low
            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.ExecuteProgram();
            List<List<char>> area = new List<List<char>>();
            int total = 0;
            for (int i = 0; i < 10_000; i++)
            {
                area.Add(new List<char>());
                int currentRunningWidth = 0;
                bool beamFound = false;

                for (int j = 0; j < 10_000; j++)
                {
                    vm.Reset();
                    vm.AddInput(j); // x
                    vm.AddInput(i); // y

                    vm.ResumeProgram();

                    if (vm.GetLastOutput() == 0)
                    {
                        area[i].Add('.');
                        currentRunningWidth = 0;

                        // stop if . again
                        if (beamFound)
                        {
                            break;
                        }
                    }
                    else
                    {
                        area[i].Add('#');
                        currentRunningWidth++;
                        beamFound = true;
                    }

                    // if width ok
                    if (currentRunningWidth >= 100)
                    {
                        // check if 100 above 100 tiles to the left is a #
                        if (area[i - 99].Count > j)
                        {
                            if (area[i - 99][j - 99] == '#')
                            {
                                int shortestDistance = int.MaxValue;
                                Vector2Int shortestPoint = Vector2Int.Zero;

                                for (int k = i - 99; k <= i; k++)
                                {
                                    for (int l = j - 99; l <= j; l++)
                                    {
                                        if (Vector2Int.DistanceManhattan(Vector2Int.Zero, new Vector2Int(l, k)) < shortestDistance)
                                        {
                                            shortestPoint = new Vector2Int(l, k);
                                        }
                                    }
                                }

                                return $"{shortestPoint.X * 10_000 + shortestPoint.Y}";

                                //return $"{(j - 99) * 10_000 + i}";
                            }
                        }
                    }
                }

                //Console.WriteLine();
            }
            */

            //return $"{total} points are affected";
        }

        private bool IsInTractorBeam(int x, int y, IntCodeVM2 vm)
        {
            vm.Reset();
            vm.AddInput(x);
            vm.AddInput(y);
            vm.ExecuteProgram();

            return vm.GetLastOutput() == 1;
        }
    }
}