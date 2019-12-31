using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2019.Days.Day25
{
    public class Day25 : IDay
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
            // set to true to play the text adventure
            bool userPlay = false;


            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.ExecuteProgram();

            if (userPlay)
            {
                while (true)
                {
                    // output
                    foreach (long l in vm.GetOutputs())
                    {
                        if (l < 255)
                        {
                            Console.Write((char)l);
                        }
                        else
                        {
                            Console.Write(l);
                        }
                    }

                    vm.ClearOutput();

                    // input
                    string inputLine = Console.ReadLine();

                    ASCIIHelper helper = new ASCIIHelper();
                    helper.AddLine(inputLine);

                    vm.AddInput(helper.Convert());
                    vm.ResumeProgram();
                }
            }
            else
            {
                // list of inputs to get to the end
                List<string> inputs = new List<string>()
                {
                    "south",
                    "take food ration",
                    "west",
                    "north",
                    "north",
                    "east",
                    "take astrolabe",
                    "west",
                    "south",
                    "south",
                    "east",
                    "north",
                    "east",
                    "south",
                    "take weather machine",
                    "west",
                    "take ornament",
                    "east",
                    "north",
                    "east",
                    "east",
                    "east",
                    "south"
                };

                ASCIIHelper helper = new ASCIIHelper();
                foreach (string input in inputs)
                {
                    helper.AddLine(input);
                }

                vm.AddInput(helper.Convert());
                vm.ResumeProgram();

                List<long> output = vm.GetOutputs();
                output.RemoveAt(output.Count - 1); // remove last \n
                int lastLineStart = output.LastIndexOf(10); // get last sentence

                // convert characters to a string
                StringBuilder lastLine = new StringBuilder();
                for (int i = lastLineStart + 1; i < output.Count; i++)
                {
                    if (output[i] < 255)
                    {
                        lastLine.Append((char) output[i]);
                    }
                    else
                    {
                        lastLine.Append(output[i]);
                    }
                }

                // get the number from the last line
                return Regex.Match(lastLine.ToString(), @"\d+").Value;
            }

            return $"";
        }

        /// <inheritdoc />
        public string Part2()
        {
            return $"";
        }
    }
}