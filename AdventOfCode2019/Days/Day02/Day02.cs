using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2019.Days.Day02
{
    public class Day02 : IDay
    {
        private List<string> inputs = new List<string>();
        private List<int> splitInput = new List<int>();
        private List<int> workingMemory = new List<int>();

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

            //inputs[0] = "1,0,0,0,99";

            string[] split = inputs[0].Split(',');
            foreach (string s in split)
            {
                splitInput.Add(Convert.ToInt32(s));
            }

            workingMemory = new List<int>(splitInput);
        }

        /// <inheritdoc />
        public string Part1()
        {
            //workingMemory[1] = 12;
            //workingMemory[2] = 2;

            int opCode = 0;
            int opCodeIndex = 0;
            opCode = workingMemory[opCodeIndex];

            bool hasEnded = false;
            while (!hasEnded)
            {
                int a, b, c;

                switch (opCode)
                {
                    case 1:
                        a = workingMemory[opCodeIndex + 1];
                        b = workingMemory[opCodeIndex + 2];
                        c = workingMemory[opCodeIndex + 3];
                        if (a >= workingMemory.Count || b >= workingMemory.Count || c >= workingMemory.Count)
                        {
                            hasEnded = true;
                            return "0";
                        }

                        workingMemory[c] = workingMemory[a] + workingMemory[b];

                        opCodeIndex += 4;
                        break;

                    case 2:
                        a = workingMemory[opCodeIndex + 1];
                        b = workingMemory[opCodeIndex + 2];
                        c = workingMemory[opCodeIndex + 3];
                        if (a >= workingMemory.Count || b >= workingMemory.Count || c >= workingMemory.Count)
                        {
                            hasEnded = true;
                            return "0";
                        }

                        workingMemory[c] = workingMemory[a] * workingMemory[b];

                        opCodeIndex += 4;
                        break;

                    case 99:
                        hasEnded = true;
                        break;

                    default:
                        hasEnded = true;
                        break;
                }

                opCode = workingMemory[opCodeIndex];
            }

            return $"{workingMemory[0]}";
        }

        /// <inheritdoc />
        public string Part2()
        {
            Reset();
            // 19690720

            // find right noun
            int noun = 1;
            int verb = 0;
            int programName = 100 * noun + verb;
            int outputOfProgram = 0;
            int searchSpace = 100;

            for (int i = 0; i < searchSpace; i++)
            {
                noun = i;
                for (int j = 0; j < searchSpace; j++)
                {
                    verb = j;
                    Reset();
                    workingMemory[1] = noun;
                    workingMemory[2] = verb;
                    
                    programName = 100 * noun + verb;

                    outputOfProgram = Convert.ToInt32(Part1());
                    if (outputOfProgram == 19690720)
                    {
                        Console.WriteLine($"{programName}");
                    }
                }
            }

            return $"nothing";
        }

        /// <summary>
        /// Reset memory
        /// </summary>
        private void Reset()
        {
            workingMemory = new List<int>(splitInput);
        }
    }
}