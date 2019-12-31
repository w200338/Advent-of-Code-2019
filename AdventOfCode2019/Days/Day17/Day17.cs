using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2019.Tools.Vectors;

namespace AdventOfCode2019.Days.Day17
{
    public class Day17 : IDay
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

            List<long> outputs = vm.GetOutputs();
            
            int line = 0;
            int posInLine = 0;

            List<Vector2Int> scaffold = new List<Vector2Int>();

            foreach (long c in outputs)
            {
                switch (c)
                {
                    // new line
                    case 10:
                        Console.WriteLine();
                        line++;
                        posInLine = 0;
                        continue;
                        break;

                    // #
                    case 35:
                        Console.Write("#");
                        scaffold.Add(new Vector2Int(posInLine, line));
                        break;

                    // .
                    case 46:
                    Console.Write(".");
                        break;

                    // <
                    case 60:
                        Console.Write("<");
                        break;

                    // >
                    case 62:
                        Console.Write(">");
                        break;

                    // ^
                    case 94:
                        Console.Write("^");
                        break;

                    // v
                    case 118:
                        Console.Write("v");
                        break;

                    // x
                    case 120:
                        Console.Write("x");
                        Console.WriteLine("");
                        return "It's dead Jim!";
                        break;
                }

                posInLine++;
            }

            // check for intersections
            int sumOfAlignment = 0;
            foreach (Vector2Int vector in scaffold)
            {
                if (scaffold.Contains(new Vector2Int(vector.X + 1, vector.Y)) && 
                    scaffold.Contains(new Vector2Int(vector.X - 1, vector.Y)) && 
                    scaffold.Contains(new Vector2Int(vector.X, vector.Y + 1)) && 
                    scaffold.Contains(new Vector2Int(vector.X, vector.Y - 1)))
                {
                    Console.WriteLine($"Intersection at {vector.X} {vector.Y}");
                    sumOfAlignment += vector.X * vector.Y;
                }
            }

            return $"Sum {sumOfAlignment}";
        }

        /// <inheritdoc />
        public string Part2()
        {
            IntCodeVM2 vm = new IntCodeVM2(program);
            vm.SetMemory(0, 2); // put into different mode

            // input
            // L,12, L,12, R,12,
            // L,12, L,12, R,12
            // L,8, L,8, R,12, L,8, L,8,
            // L,10, R,8, R,12,
            // L,10, R,8, R,12,
            // L,12, L,12, R,12
            // L,8, L,8, R,12, L,8, L,8,
            // L,10, R,8, R,12,
            // L,12, L,12, R,12,
            // L,8, L,8, R,12, L,8, L,8
            // 
            // A, A, B, C, C, A, B, C, A, B
            // A L,12, L,12, R,12
            // B L,8, L,8, R,12, L,8, L,8
            // C L,10, R,8, R,12,

            List<long> inputs = new List<long>()
            {
                65, 44, 65, 44, 66, 44, 67, 44, 67, 44, 65, 44, 66, 44, 67, 44, 65, 44, 66, 10,
                76, 44, 49, 50, 44, 76, 44, 49, 50, 44, 82, 44, 49, 50, 10,
                76, 44, 56, 44, 76, 44, 56, 44, 82, 44, 49, 50, 44, 76, 44, 56, 44, 76, 44, 56, 10,
                76, 44, 49, 48, 44, 82, 44, 56, 44, 82, 44, 49, 50, 10,
                110, 10
            };

            foreach (long l in inputs)
            {
                vm.AddInput(l);
            }

            vm.ExecuteProgram();

            List<long> outputs = vm.GetOutputs();

            int line = 0;
            int posInLine = 0;

            List<Vector2Int> scaffold = new List<Vector2Int>();
            long total = 0;


            foreach(long c in outputs)
            {
                switch (c)
                {
                    // new line
                    case 10:
                        Console.WriteLine();
                        line++;
                        posInLine = 0;
                        continue;
                        break;

                    // #
                    case 35:
                        Console.Write("#");
                        scaffold.Add(new Vector2Int(posInLine, line));
                        break;

                    // .
                    case 46:
                        Console.Write(".");
                        break;

                    // <
                    case 60:
                        Console.Write("<");
                        break;

                    // >
                    case 62:
                        Console.Write(">");
                        break;

                    // ^
                    case 94:
                        Console.Write("^");
                        break;

                    // v
                    case 118:
                        Console.Write("v");
                        break;

                        
                    // x
                    case 120:
                        Console.Write("x");
                        //Console.WriteLine("");
                        //return "It's dead Jim!";
                        break;
                        

                    default:
                        if ( c < 255)
                        {
                            Console.Write((char)c);
                        }
                        else
                        {
                            Console.WriteLine($"\n{c}");
                        }
                        total += c;
                        break;
                }

                posInLine++;
            }

            // higher than 5624
            //var test = outputs.Skip(3121).ToList();

            return $"total {total}";
        }

        private enum Tile
        {
            Empty,
            Scaffold,
            Robot
        }
    }
}