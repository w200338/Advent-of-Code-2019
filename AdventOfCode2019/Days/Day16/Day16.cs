using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2019.Days.Day16
{
    public class Day16 : IDay
    {
        private List<string> inputs = new List<string>();
        List<int> inputNumbers = new List<int>();

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

            //inputs[0] = "12345678";
            //inputs[0] = "80871224585914546619083218645595";
            //inputs[0] = "03036732577212944063491565474664";

            inputNumbers = inputs[0].ToCharArray().AsParallel().Select(c => int.Parse(c.ToString())).ToList();
        }

        /// <inheritdoc />
        public string Part1()
        {
            // copy input
            List<int> stepNumbers = new List<int>(inputNumbers);
            List<int> currentNumbers = new List<int>();
            // steps
            for (int i = 1; i <= 100; i++)
            {
                // every input
                for (int j = 1; j <= inputNumbers.Count; j++)
                {
                    /*
                    // create pattern
                    List<int> pattern = new List<int>();
                    while (pattern.Count < inputNumbers.Count + 1)
                    {
                        for (int k = 1; k <= j; k++)
                        {
                            pattern.Add(0);
                        }

                        for (int k = 1; k <= j; k++)
                        {
                            pattern.Add(1);
                        }

                        for (int k = 1; k <= j; k++)
                        {
                            pattern.Add(0);
                        }

                        for (int k = 1; k <= j; k++)
                        {
                            pattern.Add(-1);
                        }
                    }

                    // shift left by one
                    pattern.RemoveAt(0);
                    */

                    // calculate one line
                    long accumulator = 0;
                    for (int k = 0; k < stepNumbers.Count; k++)
                    {
                        accumulator += stepNumbers[k] * CalcMultiplier(j, k); // pattern[k];
                    }

                    accumulator = Math.Abs(accumulator) % 10;

                    currentNumbers.Add((int)accumulator);
                }

                // overwrite first list
                stepNumbers = new List<int>(currentNumbers);
                currentNumbers.Clear();

                Console.WriteLine($"after {i} phases {stepNumbers[0]}{stepNumbers[1]}{stepNumbers[2]}{stepNumbers[3]}{stepNumbers[4]}{stepNumbers[5]}{stepNumbers[6]}{stepNumbers[7]}");
            }

            return $"after 100 phases: {stepNumbers[0]}{stepNumbers[1]}{stepNumbers[2]}{stepNumbers[3]}{stepNumbers[4]}{stepNumbers[5]}{stepNumbers[6]}{stepNumbers[7]}";
        }

        /// <inheritdoc />
        public string Part2()
        {
            //Console.WriteLine($"LeastCommonMultiple {inputNumbers.Count} and {4} is {LeastCommonMultiple(inputNumbers.Count, 4)}");


            // get first 7 digits of input as offset
            int offset = 0;
            for (int i = 0; i < 7; i++)
            {
                offset *= 10;
                offset += inputNumbers[i];
            }

            // copy input 10_000 times
            List<int> stepNumbers = new List<int>();
            for (int i = 0; i < 10_000; i++)
            {
                stepNumbers.AddRange(inputNumbers);
            }

            // 100 steps
            for (int i = 1; i <= 100; i++)
            {
                // coming from https://old.reddit.com/r/adventofcode/comments/ebai4g/2019_day_16_solutions/fb3lts9/
                // first half of the list is incorrect
                // calculate last digit the slow way
                long accumulator = 0;
                for (int k = 0; k < stepNumbers.Count; k++)
                {
                    accumulator += stepNumbers[k] * CalcMultiplier(stepNumbers.Count, k);
                }

                accumulator = Math.Abs(accumulator) % 10;
                stepNumbers[stepNumbers.Count - 1] = (int)accumulator;

                // new[i] = (prev[i] + new[i + 1]) % 10
                for (int j = stepNumbers.Count - 2; j >= 0; j--)
                {
                    stepNumbers[j] = (stepNumbers[j] + stepNumbers[j + 1]) % 10;
                }

                Console.WriteLine($"after {i} phases {stepNumbers[0]}{stepNumbers[1]}{stepNumbers[2]}{stepNumbers[3]}{stepNumbers[4]}{stepNumbers[5]}{stepNumbers[6]}{stepNumbers[7]}");
            }

            Console.WriteLine($"message offset {offset}");

            // higher than 24680246
            return $"after 100 phases: {stepNumbers[offset + 0]}{stepNumbers[offset + 1]}{stepNumbers[offset + 2]}{stepNumbers[offset + 3]}{stepNumbers[offset + 4]}{stepNumbers[offset + 5]}{stepNumbers[offset + 6]}{stepNumbers[offset + 7]}";
        }

        private int CalcMultiplier(int indexInInput, int index)
        {
            index++;
            index %= 4 * indexInInput;

            if (index < indexInInput)
            {
                return 0;
            }

            if (index < indexInInput * 2)
            {
                return 1;
            }

            if (index < indexInInput * 3)
            {
                return 0;
            }

            return -1;
        }
    }
}