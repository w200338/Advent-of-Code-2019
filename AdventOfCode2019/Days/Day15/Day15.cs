using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2019.Tools.Vectors;

namespace AdventOfCode2019.Days.Day15
{
    public class Day15 : IDay
    {
        private List<string> inputs = new List<string>();
        private List<long> program = new List<long>();

        private Random random = new Random();
        private bool visualize = true;

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

        private void DrawScreen(Dictionary<Vector2Int, Tile> tiles, Vector2Int droidPos)
        {
            // hide cursor
            Console.CursorVisible = false;

            // find edges
            int minX = tiles.OrderBy(t => t.Key.X).ToList().First().Key.X;
            int maxX = tiles.OrderBy(t => t.Key.X).ToList().Last().Key.X;
            int minY = tiles.OrderBy(t => t.Key.Y).ToList().First().Key.Y;
            int maxY = tiles.OrderBy(t => t.Key.Y).ToList().Last().Key.Y;

            // move cursor and set window size
            Console.SetCursorPosition(0, 7);
            Console.SetWindowSize(50, 50);
            for (int i = minY; i <= maxY; i++)
            {
                for (int j = minX; j <= maxX; j++)
                {
                    Tile tile = Tile.Unexplored;
                    if (tiles.TryGetValue(new Vector2Int(j, i), out tile))
                    {
                        
                    }
                    else
                    {
                        tile = Tile.Unexplored;
                    }

                    if (new Vector2Int(j, i).Equals(droidPos))
                    {
                        Console.Write("D");
                    }
                    else
                    {
                        switch (tile)
                        {
                            case Tile.Unexplored:
                                Console.Write(" ");
                                break;

                            case Tile.Empty:
                                Console.Write(".");
                                break;

                            case Tile.Wall:
                                Console.Write("#");
                                break;

                            case Tile.Oxygen:
                                Console.Write("*");
                                break;

                            case Tile.Oxygenated:
                                Console.Write("O");
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();

            // show cursor again
            Console.CursorVisible = true;
            Console.SetCursorPosition(47, 10);
        }

        // used for drawing
        Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>
        {
            { Vector2Int.Zero, Tile.Empty }
        };

        List<Place> places = new List<Place>();

        /// <inheritdoc />
        public string Part1()
        {
            IntCodeVM2 vm = new IntCodeVM2(program);

            // used for drawing
            tiles = new Dictionary<Vector2Int, Tile>
            {
                { Vector2Int.Zero, Tile.Empty }
            };

            // all visited places
            places = new List<Place>();
            places.Add(new Place()
            {
                Position = Vector2Int.Zero,
                PreviousDir = Direction.East,
                UnExplored = new List<Direction>() { Direction.East, Direction.North, Direction.West, Direction.South}
            });

            Vector2Int currentPos = Vector2Int.Zero;
            Vector2Int lastValidPos = Vector2Int.Zero;
            
            while (places.Count(p => p.UnExplored.Count > 0) > 0)
            {
                // generate next move
                Direction dir = NextMove(currentPos, places);
                vm.AddInput((int)dir);
                currentPos = Move(currentPos, dir);
                

                /*
                // bump into every wall and pray
                switch (random.Next(0, 4))
                {
                    case 0:
                        vm.AddInput((int)Direction.North);
                        currentPos = Move(currentPos, Direction.North);
                        break;

                    case 1:
                        vm.AddInput((int)Direction.South);
                        currentPos = Move(currentPos, Direction.South);
                        break;

                    case 2:
                        vm.AddInput((int)Direction.West);
                        currentPos = Move(currentPos, Direction.West);
                        break;

                    case 3:
                        vm.AddInput((int)Direction.East);
                        currentPos = Move(currentPos, Direction.East);
                        break;
                }
                */
                
                // get input
                /*
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        vm.AddInput((int)Direction.North);
                        currentPos = Move(currentPos, Direction.North);
                        break;

                    case ConsoleKey.DownArrow:
                        vm.AddInput((int)Direction.South);
                        currentPos = Move(currentPos, Direction.South);
                        break;

                    case ConsoleKey.LeftArrow:
                        vm.AddInput((int)Direction.West);
                        currentPos = Move(currentPos, Direction.West);
                        break;

                    case ConsoleKey.RightArrow:
                        vm.AddInput((int)Direction.East);
                        currentPos = Move(currentPos, Direction.East);
                        break;
                }
                */
                

                // resume
                vm.ResumeProgram();

                // get status
                switch (vm.GetLastOutput())
                {
                    case 0:
                        if (!tiles.ContainsKey(currentPos))
                        {
                            tiles.Add(currentPos, Tile.Wall);
                        }
                        
                        currentPos = lastValidPos;
                        break;

                    case 1:
                        lastValidPos = currentPos;

                        if (!tiles.ContainsKey(currentPos))
                        {
                            tiles.Add(currentPos, Tile.Empty);

                            // add surrounding tiles to explore
                            Place newPlace = new Place()
                            {
                                Position = currentPos,
                                PreviousDir = OppositeDir(dir),
                            };

                            if (!tiles.ContainsKey(currentPos + new Vector2Int(0, -1)))
                            {
                                newPlace.UnExplored.Add(Direction.North);
                            }
                            if (!tiles.ContainsKey(currentPos + new Vector2Int(0, +1)))
                            {
                                newPlace.UnExplored.Add(Direction.South);
                            }
                            if (!tiles.ContainsKey(currentPos + new Vector2Int(-1, 0)))
                            {
                                newPlace.UnExplored.Add(Direction.West);
                            }
                            if (!tiles.ContainsKey(currentPos + new Vector2Int(+1, 0)))
                            {
                                newPlace.UnExplored.Add(Direction.East);
                            }

                            places.Add(newPlace);
                        }

                        break;

                    case 2:
                        lastValidPos = currentPos;

                        if (!tiles.ContainsKey(currentPos))
                        {
                            tiles.Add(currentPos, Tile.Oxygen);

                            // add surrounding tiles to explore
                            Place newPlace = new Place()
                            {
                                Position = currentPos,
                                PreviousDir = OppositeDir(dir),
                            };

                            if (!tiles.ContainsKey(currentPos + new Vector2Int(0, -1)))
                            {
                                newPlace.UnExplored.Add(Direction.North);
                            }
                            if (!tiles.ContainsKey(currentPos + new Vector2Int(0, +1)))
                            {
                                newPlace.UnExplored.Add(Direction.South);
                            }
                            if (!tiles.ContainsKey(currentPos + new Vector2Int(-1, 0)))
                            {
                                newPlace.UnExplored.Add(Direction.West);
                            }
                            if (!tiles.ContainsKey(currentPos + new Vector2Int(+1, 0)))
                            {
                                newPlace.UnExplored.Add(Direction.East);
                            }

                            places.Add(newPlace);
                        }
                        break;
                }

                // draw screen
                if (visualize)
                {
                    DrawScreen(tiles, currentPos);
                }
            }

            // find out where oxygen is
            Vector2Int oxygenLocation = tiles.Where(t => t.Value == Tile.Oxygen).Select(t => t.Key).First();
            Place oxygenPlace = places.Find(p => p.Position.Equals(oxygenLocation));
            
            int distance = 0;
            Place currentPlace = oxygenPlace;
            
            while (!currentPlace.Position.Equals(Vector2Int.Zero))
            {
                distance++;
                var currentPosition = Move(currentPlace.Position, currentPlace.PreviousDir);
                currentPlace = places.Find(p => p.Position.Equals(currentPosition));
            }

            // push output down a bit
            if (visualize)
            {
                Console.SetCursorPosition(0, 50);
            }
            
            return $"Distance to travel: {distance} tiles";
        }

        /// <inheritdoc />
        public string Part2()
        {
            // oxygen starts at the oxygen generatpr
            List<Vector2Int> nextMinute = NearbyEmpty(tiles.First(t => t.Value == Tile.Oxygen).Key, places, tiles);

            // replace oxygenGenerator tile with oxygenated tile
            tiles[tiles.First(t => t.Value == Tile.Oxygen).Key] = Tile.Oxygenated;
            
            int minute = 0;

            // spread oxygen
            //while (tiles.Count(t => t.Value == Tile.Empty) > 0)
            while(nextMinute.Count > 0)
            {
                // increase time
                minute++;

                // fill tiles with oxygen
                foreach (Vector2Int pos in nextMinute)
                {
                    tiles[pos] = Tile.Oxygenated;
                }

                // generate new list
                List<Vector2Int> newNextMinute = new List<Vector2Int>();

                foreach (Vector2Int pos in nextMinute)
                {
                    newNextMinute.AddRange(NearbyEmpty(pos, places, tiles));
                }

                // overwrite list
                nextMinute = newNextMinute;

                // draw screen
                if (visualize)
                {
                    DrawScreen(tiles, Vector2Int.Zero);
                }
            }

            if (visualize)
            {
                Console.SetCursorPosition(0, 51);
            }

            return $"it took {minute} minutes to fill the area";
        }

        /// <summary>
        /// Determine next move
        /// </summary>
        /// <param name="currentPos"></param>
        /// <param name="places"></param>
        /// <returns></returns>
        private Direction NextMove(Vector2Int currentPos, List<Place> places)
        {
            Place place = places.FirstOrDefault(p => p.Position.Equals(currentPos));

            // first unexplored place
            if (place.UnExplored.Count > 0)
            {
                Direction output = place.UnExplored[0];
                place.UnExplored.RemoveAt(0);
                return output;
            }

            // go back
            return place.PreviousDir;
        }

        /// <summary>
        /// Find nearby empty tiles
        /// </summary>
        /// <param name="currentPos"></param>
        /// <param name="places"></param>
        /// <param name="tiles"></param>
        /// <returns></returns>
        private List<Vector2Int> NearbyEmpty(Vector2Int currentPos, List<Place> places, Dictionary<Vector2Int, Tile> tiles)
        {
            List<Vector2Int> output = new List<Vector2Int>();

            Tile tile;
            if (tiles.TryGetValue(Move(currentPos, Direction.North), out tile))
            {
                if (tile == Tile.Empty)
                {
                    output.Add(Move(currentPos, Direction.North));
                }
            }

            if (tiles.TryGetValue(Move(currentPos, Direction.South), out tile))
            {
                if (tile == Tile.Empty)
                {
                    output.Add(Move(currentPos, Direction.South));
                }
            }

            if (tiles.TryGetValue(Move(currentPos, Direction.East), out tile))
            {
                if (tile == Tile.Empty)
                {
                    output.Add(Move(currentPos, Direction.East));
                }
            }

            if (tiles.TryGetValue(Move(currentPos, Direction.West), out tile))
            {
                if (tile == Tile.Empty)
                {
                    output.Add(Move(currentPos, Direction.West));
                }
            }

            return output;
        }

        private class Place
        {
            /// <summary>
            /// Position
            /// </summary>
            public Vector2Int Position { get; set; }
            /// <summary>
            /// Where it came from
            /// </summary>
            public Direction PreviousDir { get; set; }

            public List<Direction> UnExplored { get; set; } = new List<Direction>();
        }

        private Direction OppositeDir(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return Direction.South;

                case Direction.South:
                    return Direction.North;

                case Direction.West:
                    return Direction.East;

                case Direction.East:
                    return Direction.West;

                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        private Vector2Int Move(Vector2Int position, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return position + new Vector2Int(0, -1);
                    
                case Direction.South:
                    return position + new Vector2Int(0, 1);
                    
                case Direction.West:
                    return position + new Vector2Int(-1, 0);
                    
                case Direction.East:
                    return position + new Vector2Int(1, 0);
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        private enum Direction
        {
            North = 1,
            South = 2,
            West = 3,
            East = 4
        }

        private enum Tile
        {
            Unexplored,
            Empty,
            Wall,
            Oxygen,
            Oxygenated
            /*
            HitWall = 0,
            MovedOneNeedStep = 1,
            MovedOneFoundOxygen = 2
            */
        }
    }
}