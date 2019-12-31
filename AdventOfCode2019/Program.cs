using System;
using System.Collections.Generic;
using AdventOfCode2019.Days;
using AdventOfCode2019.Days.Day01;
using AdventOfCode2019.Days.Day02;
using AdventOfCode2019.Days.Day03;
using AdventOfCode2019.Days.Day04;
using AdventOfCode2019.Days.Day05;
using AdventOfCode2019.Days.Day06;
using AdventOfCode2019.Days.Day07;
using AdventOfCode2019.Days.Day08;
using AdventOfCode2019.Days.Day09;
using AdventOfCode2019.Days.Day10;
using AdventOfCode2019.Days.Day11;
using AdventOfCode2019.Days.Day12;
using AdventOfCode2019.Days.Day13;
using AdventOfCode2019.Days.Day14;
using AdventOfCode2019.Days.Day15;
using AdventOfCode2019.Days.Day16;
using AdventOfCode2019.Days.Day17;
using AdventOfCode2019.Days.Day18;
using AdventOfCode2019.Days.Day19;
using AdventOfCode2019.Days.Day20;
using AdventOfCode2019.Days.Day21;
using AdventOfCode2019.Days.Day22;
using AdventOfCode2019.Days.Day23;
using AdventOfCode2019.Days.Day24;
using AdventOfCode2019.Days.Day25;

namespace AdventOfCode2019
{
    static class Program
    {
        /// <summary>
        /// List of all days
        /// </summary>
        private static List<IDay> days;

        /// <summary>
        /// Shortcut to current day
        /// </summary>
        private static bool shortcut = false;

        /// <summary>
        /// Calculate time of day
        /// </summary>
        private static bool calculateTime = true;

        /// <summary>
        /// Run all solutions
        /// </summary>
        private static bool runAll = false;

        /// <summary>
        /// Run benchmark of IntCodeVm before running anything
        /// </summary>
        private static bool runBenchmark = false;

        public static void Main(string[] args)
        {
            GenerateDaysList();

            // benchmark
            if (runBenchmark)
            {
                Benchmark();
            }

            // run all days and record time
            if (runAll)
            {
                RunAllDays();
            }

            Console.WriteLine("Advent of code 2019 c#");

            // if shortcutting to current day
            int currentDay;
            if (shortcut)
            {
                currentDay = DateTime.Now.Day;
                if (currentDay > 25)
                {
                    currentDay = 1;
                    Console.WriteLine("It's past day 25, auto selecting day 1");
                }
            }
            else
            {
                currentDay = GetDayInput();
            }

            // for calculating required time
            DateTime startTime = DateTime.MinValue, part1Time = DateTime.MinValue, part2Time = DateTime.MinValue;
            if (calculateTime)
            {
                startTime = DateTime.Now;
            }

            Console.WriteLine($"Running day {currentDay}");

            // get input from file
            Console.WriteLine("Reading input");
            days[currentDay - 1].ReadInput();

            // run part 1
            Console.WriteLine("Running part 1");
            Console.WriteLine(days[currentDay - 1].Part1());
            Console.WriteLine();

            if (calculateTime)
            {
                part1Time = DateTime.Now;
            }

            // run part 2
            Console.WriteLine("Running part 2");
            Console.WriteLine(days[currentDay - 1].Part2());
            Console.WriteLine();

            if (calculateTime)
            {
                part2Time = DateTime.Now;
                Console.WriteLine("\nTime taken");
                Console.WriteLine($"Part 1: {(part1Time - startTime).TotalSeconds:0.000} seconds");
                Console.WriteLine($"part 2: {(part2Time - part1Time).TotalSeconds:0.000} seconds");
                Console.WriteLine($"Total: {(part2Time - startTime).TotalSeconds:0.000} seconds");
            }

            // wait for another keypress
            Console.Write("Press key to close...");
            Console.ReadKey();
        }

        /// <summary>
        /// Run some IntCodeVM benchmarks
        /// </summary>
        private static void Benchmark()
        {
            Console.WriteLine("Running benchmark");

            DateTime benchmarkStart = DateTime.Now;
            IntCodeBenchmark.BenchmarkSumOfPrimes(100_000);
            IntCodeBenchmark.BenchmarkSumOfPrimes(2_000_000);
            IntCodeBenchmark.BenchmarkAckermann(3, 6);
            IntCodeBenchmark.BenchmarkISqrt(130);
            IntCodeBenchmark.BenchmarkISqrt(1_300_000);
            IntCodeBenchmark.BenchmarkDivMod(1024, 3);
            IntCodeBenchmark.BenchmarkDivMod(1_024_000, 3);
            IntCodeBenchmark.BenchmarkPrimeFactor(19_338_240);
            IntCodeBenchmark.BenchmarkPrimeFactor(2_147_483_647);
            IntCodeBenchmark.BenchmarkPrimeFactor(19_201_644_899);
            DateTime benchmarkEnd = DateTime.Now;

            Console.WriteLine($"All benchmarks and setups took {(benchmarkEnd - benchmarkStart).TotalSeconds:F3}s\n");
        }

        /// <summary>
        /// Run all days one after the other
        /// </summary>
        private static void RunAllDays()
        {
            DateTime start = DateTime.Now;

            for (int i = 0; i < 25; i++)
            {
                days[i].ReadInput();
                days[i].Part1();
                days[i].Part2();
            }

            DateTime end = DateTime.Now;

            Console.WriteLine($"Part 1: {(end - start).TotalSeconds:F3} seconds");
        }

        /// <summary>
        /// Generate the list of all days
        /// </summary>
        private static void GenerateDaysList()
        {
            days = new List<IDay>(25)
            {
                new Day01(),
                new Day02(),
                new Day03(),
                new Day04(),
                new Day05(),
                new Day06(),
                new Day07(),
                new Day08(),
                new Day09(),
                new Day10(),
                new Day11(),
                new Day12(),
                new Day13(),
                new Day14(),
                new Day15(),
                new Day16(),
                new Day17(),
                new Day18(),
                new Day19(),
                new Day20(),
                new Day21(),
                new Day22(),
                new Day23(),
                new Day24(),
                new Day25()
            };
        }

        /// <summary>
        /// Get day to run from user
        /// </summary>
        /// <returns></returns>
        private static int GetDayInput()
        {
            int output = 0;

            while (output == 0)
            {
                Console.WriteLine("Input day: ");

                try
                {
                    output = Convert.ToInt32(Console.ReadLine());
                    if (output < 0 || output > 25)
                    {
                        output = 0;
                    }
                }
                catch (FormatException)
                {

                }
            }

            return output;
        }
    }
}