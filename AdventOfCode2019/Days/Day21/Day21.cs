using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Days.Day21
{
    public class Day21 : IDay
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

            // show prompt
            vm.ExecuteProgram();
            DrawASCIIOutput(vm.GetOutputs());
            vm.ClearOutput();

            // input program
            //J = (NOT A OR NOT B OR NOT C) AND D
            //
            //  @
            // ##ABCD###
            //
            // if D is ground and any of A,B,C aren't then jump
            ASCIIHelper helper = new ASCIIHelper();
            helper.AddLine("NOT A T");
            helper.AddLine("NOT B J");
            helper.AddLine("OR T J");
            helper.AddLine("NOT C T");
            helper.AddLine("OR T J");
            helper.AddLine("AND D J");

            helper.AddLine("WALK");

            vm.AddInput(helper.Convert());

            // output what it showed
            vm.ResumeProgram();

            if (vm.GetLastOutput() > 255)
            {
                Console.WriteLine($"\nhull damage taken: {vm.GetLastOutput()}");
            }
            else
            {
                DrawASCIIOutput(vm.GetOutputs());
            }

            return "";
        }

        /// <inheritdoc />
        public string Part2()
        {
            IntCodeVM2 vm = new IntCodeVM2(program);

            // show prompt
            vm.ExecuteProgram();
            DrawASCIIOutput(vm.GetOutputs());
            vm.ClearOutput();

            // input program
            // 
            //  @
            // ##ABCDEFGHI###
            //
            // if D is ground and any of A,B,C aren't then jump (p1)
            // invert T (now equal to C)
            // if ((I OR F) AND E) OR H are ground, then jump
            ASCIIHelper helper = new ASCIIHelper();
            helper.AddLine("NOT A T");
            helper.AddLine("NOT B J");
            helper.AddLine("OR T J");
            helper.AddLine("NOT C T");
            helper.AddLine("OR T J");
            helper.AddLine("AND D J");

            helper.AddLine("AND T T");

            helper.AddLine("OR I T");
            helper.AddLine("OR F T");
            helper.AddLine("AND E T");
            helper.AddLine("OR H T");
            helper.AddLine("AND T J");

            helper.AddLine("RUN");

            vm.AddInput(helper.Convert());

            // output what it showed
            vm.ResumeProgram();

            if (vm.GetLastOutput() > 255)
            {
                Console.WriteLine($"\nhull damage taken: {vm.GetLastOutput()}");
            }
            else
            {
                DrawASCIIOutput(vm.GetOutputs());
            }

            return "";
        }

        private void DrawASCIIOutput(List<long> outputs)
        {
            foreach (long c in outputs)
            {
                if (c <= 255)
                {
                    Console.Write((char)c);
                }
            }
        }
    }
}