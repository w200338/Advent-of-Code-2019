using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2019.Tools.Vectors;

namespace AdventOfCode2019.Days.Day13
{
    public class Day13 : IDay
    {
        private List<string> inputs = new List<string>();
        private List<long> longs;
        private bool humanInput = false;

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

            longs = inputs[0].Split(',').Select(long.Parse).ToList();
        }

        
        /// <inheritdoc />
        public string Part1()
        {
            IntCodeVM2 vm = new IntCodeVM2(longs);
            vm.ExecuteProgram();

            List<int> output = vm.GetOutputs().Select(l => (int)l).ToList();
            
            List<Tile> tiles = new List<Tile>();
            for (int i = 0; i < output.Count; i+= 3)
            {
                tiles.Add(new Tile()
                {
                    Position = new Vector2Int(output[i], output[i + 1]),
                    TileId = output[i + 2]
                });
            }

            int blocks = 0;
            foreach (Tile t in tiles)
            {
                if (t.TileId == 2)
                {
                    blocks++;
                }
            }

            DrawScreen(tiles);


            return $"{blocks}";
        }

        private void DrawScreen(List<Tile> input)
        {
            if (input.Count == 0) return;

            Dictionary<Vector2Int, int> tiles = new Dictionary<Vector2Int, int>();
            foreach (var tile in input)
            {
                if (!tiles.ContainsKey(tile.Position))
                {
                    tiles.Add(tile.Position, tile.TileId);
                }
            }

            int maxX = tiles.Keys.OrderBy(t => t.X).Last().X;
            int maxY = tiles.Keys.OrderBy(t => t.Y).Last().Y;

            for (int i = 0; i <= maxY; i++)
            {
                for (int j = 0; j <= maxX; j++)
                {
                    int color = 0;
                    if (tiles.TryGetValue(new Vector2Int(j, i), out color))
                    {

                    }

                    switch (color)
                    {
                        case 0:
                            Console.BackgroundColor = ConsoleColor.Black;
                            break;

                        case 1:
                            Console.BackgroundColor = ConsoleColor.Blue;
                            break;

                        case 2:
                            Console.BackgroundColor = ConsoleColor.Gray;
                            break;

                        case 3:
                            Console.BackgroundColor = ConsoleColor.Green;
                            break;

                        case 4:
                            Console.BackgroundColor = ConsoleColor.Red;
                            break;

                        default:
                            Console.BackgroundColor = ConsoleColor.White;
                            break;
                    }

                    Console.Write(" ");
                }

                Console.WriteLine();
            }

            Console.BackgroundColor = ConsoleColor.Black;
        }

        /// <inheritdoc />
        public string Part2()
        {
            IntCodeVM2 vm = new IntCodeVM2(longs);
            vm.SetMemory(0, 2);
            vm.ExecuteProgram();

            List<int> output = vm.GetOutputs().Select(l => (int)l).ToList();

            Dictionary<Vector2Int, int> tiles = new Dictionary<Vector2Int, int>();
            int finalScore = 0;

            // draw tiles
            for (int i = 0; i < output.Count; i += 3)
            {
                // get score
                if (output[i] == -1)
                {
                    finalScore = output[i + 2];
                    continue;
                }

                // create new tiles
                tiles.Add(new Vector2Int(output[i], output[i + 1]), output[i + 2]);
            }

            // as long as breakable blocks remain
            while (tiles.Values.Count(t => t == 2) > 0)
            {
                // redraw tiles
                output = vm.GetOutputs().Select(l => (int)l).ToList();
                for (int i = 0; i < output.Count; i += 3)
                {
                    // get score
                    if (output[i] == -1)
                    {
                        finalScore = output[i + 2];
                        Console.WriteLine($"Current score {finalScore}");
                        continue;
                    }

                    Vector2Int pos = new Vector2Int(output[i], output[i + 1]);
                    if (tiles.ContainsKey(pos))
                    {
                        tiles[pos] = output[i + 2];
                    }
                    else
                    {
                        tiles.Add(pos, output[i + 2]);
                    }
                }

                // show screen
                if (humanInput)
                {
                    DrawScreen(tiles);

                    // do a move
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.DownArrow:
                            vm.AddInput(0);
                            break;

                        case ConsoleKey.LeftArrow:
                            vm.AddInput(-1);
                            break;

                        case ConsoleKey.RightArrow:
                            vm.AddInput(+1);
                            break;
                    }
                }
                else
                {
                    // get paddle pos
                    Vector2Int paddle = tiles.Keys.ToList()[tiles.Values.ToList().FindIndex(i => i == 3)];

                    // get ball pos
                    Vector2Int ball = tiles.Keys.ToList()[tiles.Values.ToList().FindIndex(i => i == 4)];

                    // determine next move
                    if (ball.X > paddle.X)
                    {
                        vm.AddInput(1);
                    }
                    else if (ball.X < paddle.X)
                    {
                        vm.AddInput(-1);
                    }
                    else
                    {
                        vm.AddInput(0);
                    }
                }

                // run again
                vm.ClearOutput();
                vm.ResumeProgram();
            }

            


            return $"final score {finalScore}";
        }

        private void DrawScreen(Dictionary<Vector2Int, int> input)
        {
            if (input.Count == 0) return;

            int maxX = input.Keys.OrderBy(t => t.X).Last().X;
            int maxY = input.Keys.OrderBy(t => t.Y).Last().Y;

            for (int i = 0; i <= maxY; i++)
            {
                for (int j = 0; j <= maxX; j++)
                {
                    int color = 0;
                    if (input.TryGetValue(new Vector2Int(j, i), out color))
                    {

                    }

                    switch (color)
                    {
                        case 0:
                            Console.BackgroundColor = ConsoleColor.Black;
                            break;

                        case 1:
                            Console.BackgroundColor = ConsoleColor.Blue;
                            break;

                        case 2:
                            Console.BackgroundColor = ConsoleColor.Gray;
                            break;

                        case 3:
                            Console.BackgroundColor = ConsoleColor.Green;
                            break;

                        case 4:
                            Console.BackgroundColor = ConsoleColor.Red;
                            break;

                        default:
                            Console.BackgroundColor = ConsoleColor.White;
                            break;
                    }

                    Console.Write(" ");
                }

                Console.WriteLine();
            }

            Console.BackgroundColor = ConsoleColor.Black;
        }

        private class Tile
        {
            public Vector2Int Position { get; set; }
            public int TileId { get; set; }
        }
    }
}