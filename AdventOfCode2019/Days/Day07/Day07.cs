using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Days.Day07
{
    public class Day07 : IDay
    {
        private List<string> inputs = new List<string>();
        private List<int> program = new List<int>();

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

            // TODO: test
            //inputs[0] = "3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5";

            program = inputs[0].Split(',').Select(int.Parse).ToList();
        }

        /// <inheritdoc />
        public string Part1()
        {
            IntCodeVM intCodeVM;

            // get all possible combinations (not very efficient)
            int searchSpace = 4;
            int highestOutput = 0;
            for (int i = 0; i <= searchSpace; i++)
            {
                intCodeVM = new IntCodeVM(program);
                intCodeVM.AddInput(i);
                intCodeVM.AddInput(0);
                intCodeVM.ExecuteProgram();
                int firstOutput = (int)intCodeVM.GetOutputs()[0];

                for (int j = 0; j <= searchSpace; j++)
                {
                    if (j == i)
                    {
                        continue;
                    }

                    intCodeVM = new IntCodeVM(program);
                    intCodeVM.AddInput(j);
                    intCodeVM.AddInput(firstOutput);
                    intCodeVM.ExecuteProgram();
                    int secondOutput = (int)intCodeVM.GetOutputs()[0];

                    for (int k = 0; k <= searchSpace; k++)
                    {
                        if (k == i || k == j)
                        {
                            continue;
                        }

                        intCodeVM = new IntCodeVM(program);
                        intCodeVM.AddInput(k);
                        intCodeVM.AddInput(secondOutput);
                        intCodeVM.ExecuteProgram();
                        int thirdOutput = (int)intCodeVM.GetOutputs()[0]; ;

                        for (int l = 0; l <= searchSpace; l++)
                        {
                            if (l == i || l == j || l == k)
                            {
                                continue;
                            }

                            intCodeVM = new IntCodeVM(program);
                            intCodeVM.AddInput(l);
                            intCodeVM.AddInput(thirdOutput);
                            intCodeVM.ExecuteProgram();
                            int fourthOutput = (int)intCodeVM.GetOutputs()[0];

                            for (int m = 0; m <= searchSpace; m++)
                            {
                                if (m == i || m == j || m == k || m == l)
                                {
                                    continue;
                                }

                                intCodeVM = new IntCodeVM(program);
                                intCodeVM.AddInput(m);
                                intCodeVM.AddInput(fourthOutput);
                                intCodeVM.ExecuteProgram();
                                int output = (int)intCodeVM.GetOutputs()[0];

                                if (output > highestOutput)
                                {
                                    highestOutput = output;
                                    Console.WriteLine($"New highest: {highestOutput} at {i} {j} {k} {l} {m}");
                                }
                            }
                        }
                    }
                }
            }

            return $"{highestOutput}";
        }

        /// <inheritdoc />
        public string Part2()
        {
            IntCodeVM intCodeVM = new IntCodeVM(program);

            // get all possible combinations (not very efficient)
            int searchSpaceTop = 9;
            int searchSpaceBottom = 5;
            int highestOutput = 0;
            for (int i = searchSpaceBottom; i <= searchSpaceTop; i++)
            {
                for (int j = searchSpaceBottom; j <= searchSpaceTop; j++)
                {
                    if (j == i)
                    {
                        continue;
                    }

                    for (int k = searchSpaceBottom; k <= searchSpaceTop; k++)
                    {
                        if (k == i || k == j)
                        {
                            continue;
                        }

                        for (int l = searchSpaceBottom; l <= searchSpaceTop; l++)
                        {
                            if (l == i || l == j || l == k)
                            {
                                continue;
                            }

                            for (int m = searchSpaceBottom; m <= searchSpaceTop; m++)
                            {
                                if (m == i || m == j || m == k || m == l)
                                {
                                    continue;
                                }

                                IntCodeVM firstVM = new IntCodeVM(program);
                                firstVM.AddInput(i);

                                IntCodeVM secondVM = new IntCodeVM(program);
                                secondVM.AddInput(j);

                                IntCodeVM thirdVM = new IntCodeVM(program);
                                thirdVM.AddInput(k);

                                IntCodeVM fourthVM = new IntCodeVM(program);
                                fourthVM.AddInput(l);

                                IntCodeVM fifthVM = new IntCodeVM(program);
                                fifthVM.AddInput(m);

                                bool isRunning = true;
                                int firstInput = 0;

                                // keep running till last VM finishes
                                while (isRunning)
                                {
                                    firstVM.AddInput(firstInput);
                                    firstVM.ResumeProgram();
                                    int firstOutput = (int)firstVM.GetLastOutput();

                                    secondVM.AddInput(firstOutput);
                                    secondVM.ResumeProgram();
                                    int secondOutput = (int)secondVM.GetLastOutput();

                                    thirdVM.AddInput(secondOutput);
                                    thirdVM.ResumeProgram();
                                    int thirdOutput = (int)thirdVM.GetLastOutput();

                                    fourthVM.AddInput(thirdOutput);
                                    fourthVM.ResumeProgram();
                                    int fourthOutput = (int)fourthVM.GetLastOutput();

                                    fifthVM.AddInput(fourthOutput);
                                    isRunning = (fifthVM.ResumeProgram() == IntCodeVM.HaltCode.Finished);
                                    firstInput = (int)fifthVM.GetLastOutput();
                                }

                                if (firstInput > highestOutput)
                                {
                                    highestOutput = firstInput;
                                    Console.WriteLine($"New highest: {highestOutput} at {i} {j} {k} {l} {m}");
                                }
                            }
                        }
                    }
                }
            }

            return $"{highestOutput}";

            //3909195 is too low
        }
    }
}