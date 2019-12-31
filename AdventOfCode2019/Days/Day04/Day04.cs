using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2019.Days.Day04
{
    public class Day04 : IDay
    {
        private List<string> inputs = new List<string>();

        private int lowerBound = 156218;
        private int upperBound = 652527;

        /// <inheritdoc />
        public void ReadInput()
        {
            /*
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
            */
            Console.WriteLine("Testing known inputs");
            Console.WriteLine(IsValid(111111));
            Console.WriteLine(IsValid(223450));
            Console.WriteLine(IsValid(123789));
            Console.WriteLine();
        }

        /// <inheritdoc />
        public string Part1()
        {
            int found = 0;
            for (int i = lowerBound; i <= upperBound; i++)
            {
                if (IsValid(i))
                {
                    found++;
                }
            }

            return $"{found}";
        }

        /// <inheritdoc />
        public string Part2()
        {
            int found = 0;
            for (int i = lowerBound; i <= upperBound; i++)
            {
                if (IsValid2(i))
                {
                    found++;
                }
            }

            return $"{found}";
        }

        private bool IsValid(int input)
        {
            List<int> foundNumbers = new List<int>();
            bool hasDuplicate = false;

            for (int j = 1; j < input.ToString().Length; j++)
            {
                if (Convert.ToInt32(input.ToString()[j]) < Convert.ToInt32(input.ToString()[j - 1]))
                {
                    return false;
                }

                if (Convert.ToInt32(input.ToString()[j]) == Convert.ToInt32(input.ToString()[j - 1]))
                {
                    hasDuplicate = true;
                }
            }

            return hasDuplicate;
        }

        private bool IsValid2(int input)
        {
            // less than 1289

            List<int> foundNumbers = new List<int>();
            int[] occurrences = new int[10];

            List<int> digits = new List<int>(GetDigits3(input));

            for (int i = 0; i < digits.Count; i++)
            {
                if (i > 0 && digits[i] < digits[i - 1])
                {
                    return false;
                }

                occurrences[digits[i]]++;
            }

            foreach (int occurrence in occurrences)
            {
                if (occurrence == 2)
                {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<int> GetDigits3(int source)
        {
            Stack<int> digits = new Stack<int>();
            while (source > 0)
            {
                var digit = source % 10;
                source /= 10;
                digits.Push(digit);
            }

            return digits;
        }
    }
}