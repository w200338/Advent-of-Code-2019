using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode2019.Tools._2DShapes;
using AdventOfCode2019.Tools.Vectors;

namespace AdventOfCode2019.Days.Day24
{
    public class Day24 : IDay
    {
        private List<string> inputs = new List<string>();
        private List<List<char>> inputGrid = new List<List<char>>();

        // thread safe
        private static List<GridDepth> grids = new List<GridDepth>();

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

            foreach (var line in inputs)
            {
                inputGrid.Add(line.ToArray().ToList());
            }
        }

        /// <inheritdoc />
        public string Part1()
        {
            List<int> bioRatings = new List<int>();
            bioRatings.Add(CalcScore(inputGrid));
            RectangleInt bounds = new RectangleInt(Vector2Int.Zero, new Vector2Int(4, 4));

            // do a step
            while (true)
            {
                List<List<char>> newGrid = new List<List<char>>();

                for (int i = 0; i < inputGrid.Count; i++)
                {
                    newGrid.Add(new List<char>());

                    for (int j = 0; j < inputGrid[0].Count; j++)
                    {
                        // check neighbours
                        int neighbourCount = 0;
                        foreach (Vector2Int neighbour in Around(new Vector2Int(j, i)))
                        {
                            if (bounds.IsInRectangle(neighbour))
                            {
                                if (inputGrid[neighbour.Y][neighbour.X] == '#')
                                {
                                    neighbourCount++;
                                }
                            }
                        }

                        // add or remove creature
                        if (inputGrid[i][j] == '#')
                        {
                            newGrid[i].Add(neighbourCount == 1 ? '#' : '.');
                        }
                        else
                        {
                            if (neighbourCount == 1 || neighbourCount == 2)
                            {
                                newGrid[i].Add('#');
                            }
                            else
                            {
                                newGrid[i].Add('.');
                            }
                        }

                        //newGrid[i].Add()
                    }
                }

                // print board

                // calc bio rating
                int bioRating = CalcScore(newGrid);

                // check if it already existed
                if (bioRatings.Contains(bioRating))
                {
                    return bioRating.ToString();
                }

                bioRatings.Add(bioRating);

                inputGrid = newGrid;
            }

            return $"";
        }

        private int CalcScore(List<List<char>> grid)
        {
            int output = 0;

            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[0].Count; j++)
                {
                    if (grid[i][j] == '#')
                    {
                        output += 1 << (i * 5 + j);
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Get list of vectors around given vector
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private List<Vector2Int> Around(Vector2Int position)
        {
            return new List<Vector2Int>()
            {
                new Vector2Int(position.X, position.Y - 1),
                new Vector2Int(position.X, position.Y + 1),
                new Vector2Int(position.X - 1, position.Y),
                new Vector2Int(position.X + 1, position.Y),
            };
        }

        /// <inheritdoc />
        public string Part2()
        {
            // SANITIZE YOUR INPUT!!!!
            inputGrid = new List<List<char>>();
            foreach (var line in inputs)
            {
                inputGrid.Add(line.ToArray().ToList());
            }

            // settings
            int depths = 203;
            int steps = 200;

            // create a list of empty grids and center grid
            grids.Add(new GridDepth()
            {
                Depth = 0,
                Grid = inputGrid,
            });

            for (int i = -depths; i <= depths; i++)
            {
                if (i == 0)
                {
                    continue;
                }

                grids.Add(new GridDepth()
                {
                    Depth = i,
                    Grid = new List<List<char>>()
                    {
                        new List<char>() {'.', '.', '.', '.', '.'},
                        new List<char>() {'.', '.', '.', '.', '.'},
                        new List<char>() {'.', '.', '?', '.', '.'},
                        new List<char>() {'.', '.', '.', '.', '.'},
                        new List<char>() {'.', '.', '.', '.', '.'},
                    }
                });
            }

            // steps
            for (int i = 0; i < steps; i++)
            {
                ConcurrentBag<GridDepth> newGrids = new ConcurrentBag<GridDepth>();

                // go over all currently existing grids
                List<Task> tasks = new List<Task>();
                foreach (var currentGrid in grids)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        int z = currentGrid.Depth;
                        List<List<char>> newGrid = new List<List<char>>();

                        // y
                        for (int k = 0; k < inputGrid.Count; k++)
                        {
                            newGrid.Add(new List<char>());

                            // x
                            for (int l = 0; l < inputGrid[0].Count; l++)
                            {
                                // skip over center
                                if (k == 2 && l == 2)
                                {
                                    newGrid[k].Add('?');
                                    continue;
                                }

                                // check neighbours
                                int neighbourCount = CountNeighbours(grids, new Vector3Int(l, k, z));

                                // add or remove creature
                                if (currentGrid.Grid[k][l] == '#')
                                {
                                    newGrid[k].Add(neighbourCount == 1 ? '#' : '.');
                                }
                                else
                                {
                                    if (neighbourCount == 1 || neighbourCount == 2)
                                    {
                                        newGrid[k].Add('#');
                                    }
                                    else
                                    {
                                        newGrid[k].Add('.');
                                    }
                                }
                            }
                        }

                        newGrids.Add(new GridDepth()
                        {
                            Depth = z,
                            Grid = newGrid
                        });
                        //currentGrid.Grid = newGrid;
                    }));
                }

                Task.WaitAll(tasks.ToArray());

                grids = newGrids.ToList();
            }

            // count the amount of bugs
            int bugCount = 0;
            foreach (GridDepth grid in grids.OrderBy(g => g.Depth))
            {
                for (int i = 0; i < grid.Grid.Count; i++)
                {
                    for (int j = 0; j < grid.Grid[0].Count; j++)
                    {
                        if (i == 2 && j == 2)
                        {
                            continue;
                        }

                        if (grid.Grid[i][j] == '#')
                        {
                            bugCount++;
                        }
                    }
                }

                PrintGrid(grid.Grid, grid.Depth);
            }

            // higher than 126
            return bugCount.ToString();
        }

        private void PrintGrid(List<List<char>> grid, int depth = 0)
        {
            Console.WriteLine($"\n Depth: {depth}");

            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[0].Count; j++)
                {
                    Console.Write(grid[j][i]);
                }

                Console.WriteLine();
            }
        }

        private int CountNeighbours(List<GridDepth> grids, Vector3Int position)
        {
            int output = 0;

            RectangleInt bounds = new RectangleInt(Vector2Int.Zero, new Vector2Int(4, 4));
            Vector2Int pos2D = new Vector2Int(position.X, position.Y);

            // check direct neighbours
            foreach (Vector2Int neighbour in Around(pos2D))
            {
                if (neighbour != new Vector2Int(2, 2) && bounds.IsInRectangle(neighbour))
                {
                    if (grids.FirstOrDefault(g => g.Depth == position.Z)?.Grid[neighbour.Y][neighbour.X] == '#')
                    //if (grids[depthIndex].Grid[neighbour.Y][neighbour.X] == '#')
                    {
                        output++;
                    }
                }
            }

            // check depth below
            if (pos2D == new Vector2Int(2, 1))
            {
                for (int i = 0; i < 5; i++)
                {
                    output += grids.FirstOrDefault(g => g.Depth == position.Z + 1)?.Grid[0][i] == '#' ? 1 : 0;
                    //output += grids[depthIndex + 1]?.Grid[0][i] == '#' ? 1 : 0;
                }
            }
            else if (pos2D == new Vector2Int(1, 2))
            {
                for (int i = 0; i < 5; i++)
                {
                    output += grids.FirstOrDefault(g => g.Depth == position.Z + 1)?.Grid[i][0] == '#' ? 1 : 0;
                    //output += grids[depthIndex + 1].Grid[i][0] == '#' ? 1 : 0;
                }
            }
            else if (pos2D == new Vector2Int(3, 2))
            {
                for (int i = 0; i < 5; i++)
                {
                    output += grids.FirstOrDefault(g => g.Depth == position.Z + 1)?.Grid[i][4] == '#' ? 1 : 0;
                    //output += grids[depthIndex + 1].Grid[i][4] == '#' ? 1 : 0;
                }
            }
            else if (pos2D == new Vector2Int(2, 3))
            {
                for (int i = 0; i < 5; i++)
                {
                    output += grids.FirstOrDefault(g => g.Depth == position.Z + 1)?.Grid[4][i] == '#' ? 1 : 0;
                    //output += grids[depthIndex + 1].Grid[4][i] == '#' ? 1 : 0;
                }
            }

            // check depth above
            if (position.X == 0)
            {
                output += grids.FirstOrDefault(g => g.Depth == position.Z - 1)?.Grid[2][1] == '#' ? 1 : 0;
                //output += grids[depthIndex - 1].Grid[2][1] == '#' ? 1 : 0;
            }
            else if (position.X == 4)
            {
                output += grids.FirstOrDefault(g => g.Depth == position.Z - 1)?.Grid[2][3] == '#' ? 1 : 0;
                //output += grids[depthIndex - 1].Grid[2][3] == '#' ? 1 : 0;
            }

            if (position.Y == 0)
            {
                output += grids.FirstOrDefault(g => g.Depth == position.Z - 1)?.Grid[1][2] == '#' ? 1 : 0;
                //output += grids[depthIndex - 1].Grid[1][2] == '#' ? 1 : 0;
            }
            else if (position.Y == 4)
            {
                output += grids.FirstOrDefault(g => g.Depth == position.Z - 1)?.Grid[3][2] == '#' ? 1 : 0;
                //output += grids[depthIndex - 1].Grid[3][2] == '#' ? 1 : 0;
            }

            return output;
        }

        private class GridDepth
        {
            public int Depth { get; set; }
            public List<List<char>> Grid { get; set; }
        }
    }
}