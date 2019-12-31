using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2019.Tools.Vectors;

namespace AdventOfCode2019.Days.Day11
{
    public class Day11 : IDay
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
            IntCodeVM2 intCodeVM = new IntCodeVM2(program);
            intCodeVM.AddInput(0);
            //intCodeVM.ResizeMemory(int.MaxValue >> 4);

            // positions and colors (1 = white, 0 = black)
            Dictionary<Vector2Int, int> paintedPanels = new Dictionary<Vector2Int, int>();
            paintedPanels.Add(Vector2Int.Zero, 0);

            // rotation
            Direction direction = Direction.Up;

            // keep running till it stops asking for input
            Vector2Int robotPos = Vector2Int.Zero;

            while (intCodeVM.ResumeProgram() == IntCodeVM2.HaltCode.WaitingForInput)
            {
                // get outputs
                int newColor = (int)intCodeVM.GetOutputs()[0];
                int directionChange = (int)intCodeVM.GetOutputs()[1];

                // paint
                if (paintedPanels.TryGetValue(robotPos, out int color))
                {
                    paintedPanels[robotPos] = newColor;
                }
                else
                {
                    paintedPanels.Add(robotPos, newColor);
                }

                // rotate
                if (directionChange == 0)
                {
                    direction = (direction - 1);
                    // left 90° from up is left
                    if ((int)direction < 0)
                    {
                        direction = Direction.Left;
                    }
                }
                else
                {
                    direction += 1;
                    // right 90° from left is up
                    if ((int)direction > 3)
                    {
                        direction = Direction.Up;
                    }
                }

                // move one panel forward
                switch (direction)
                {
                    case Direction.Up:
                        robotPos += new Vector2Int(0, -1);
                        break;

                    case Direction.Right:
                        robotPos += new Vector2Int(1, 0);
                        break;

                    case Direction.Down:
                        robotPos += new Vector2Int(0, 1);
                        break;

                    case Direction.Left:
                        robotPos += new Vector2Int(-1, 0);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                // input into VM
                if (paintedPanels.TryGetValue(robotPos, out color))
                {
                    intCodeVM.AddInput(color);
                }
                else
                {
                    intCodeVM.AddInput(0);
                }
                

                // clear output
                intCodeVM.ClearOutput();

                // run again
            }

            Console.WriteLine($"Last halt reason {intCodeVM.LastHaltReason.ToString()}");

            return $"Touched {paintedPanels.Count} panels";
            // ans = 2511
        }

        /// <inheritdoc />
        public string Part2()
        {
            IntCodeVM2 intCodeVM = new IntCodeVM2(program);
            intCodeVM.AddInput(1);                              // starting from a white panel now

            // positions and colors (1 = white, 0 = black)
            Dictionary<Vector2Int, int> paintedPanels = new Dictionary<Vector2Int, int>();
            paintedPanels.Add(Vector2Int.Zero, 0);

            // rotation
            Direction direction = Direction.Up;

            // keep running till it stops asking for input
            Vector2Int robotPos = Vector2Int.Zero;

            while (intCodeVM.ResumeProgram() == IntCodeVM2.HaltCode.WaitingForInput)
            {
                // get outputs
                int newColor = (int)intCodeVM.GetOutputs()[0];
                int directionChange = (int)intCodeVM.GetOutputs()[1];

                // paint
                if (paintedPanels.TryGetValue(robotPos, out int color))
                {
                    paintedPanels[robotPos] = newColor;
                }
                else
                {
                    paintedPanels.Add(robotPos, newColor);
                }

                // rotate
                if (directionChange == 0)
                {
                    direction = (direction - 1);
                    // left 90° from up is left
                    if ((int)direction < 0)
                    {
                        direction = Direction.Left;
                    }
                }
                else
                {
                    direction += 1;
                    // right 90° from left is up
                    if ((int)direction > 3)
                    {
                        direction = Direction.Up;
                    }
                }

                // move one panel forward
                switch (direction)
                {
                    case Direction.Up:
                        robotPos += new Vector2Int(0, -1);
                        break;

                    case Direction.Right:
                        robotPos += new Vector2Int(1, 0);
                        break;

                    case Direction.Down:
                        robotPos += new Vector2Int(0, 1);
                        break;

                    case Direction.Left:
                        robotPos += new Vector2Int(-1, 0);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                // input into VM
                if (paintedPanels.TryGetValue(robotPos, out color))
                {
                    intCodeVM.AddInput(color);
                }
                else
                {
                    intCodeVM.AddInput(0);
                }


                // clear output
                intCodeVM.ClearOutput();

                // run again
            }

            Console.WriteLine($"Last halt reason {intCodeVM.LastHaltReason.ToString()}");
            Console.WriteLine($"Touched {paintedPanels.Count} panels");

            // find highest x and highest y
            int highestX = paintedPanels.Keys.OrderBy(vec => vec.X).Last().X;
            int highestY = paintedPanels.Keys.OrderBy(vec => vec.Y).Last().Y;

            /*
            int lowestX = xSorted[0].X;                         // 0
            int highestX = xSorted[xSorted.Count - 1].X;        // 42
            int lowestY = ySorted[0].Y;                         // 0
            int highestY = ySorted[xSorted.Count - 1].Y;        // 18
            */

            for (int i = 0; i <= highestY; i++)
            {
                for (int j = 0; j <= highestX; j++)
                {
                    int color = 0;
                    if (paintedPanels.TryGetValue(new Vector2Int(j, i), out color))
                    {

                    }

                    // color
                    if (color == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                    }

                    Console.Write(" ");
                }

                Console.WriteLine();
            }

            return $"";
        }

        private Vector2Int GetDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Vector2Int(0, -1);

                case Direction.Right:
                    return new Vector2Int(1, 0);

                case Direction.Down:
                    return new Vector2Int(0, 1);

                case Direction.Left:
                    return new Vector2Int(-1, 0);
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }
    }
}