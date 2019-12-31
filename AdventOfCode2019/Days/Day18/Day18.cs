using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode2019.Tools._2DShapes;
using AdventOfCode2019.Tools.Vectors;

namespace AdventOfCode2019.Days.Day18
{
    public class Day18 : IDay
    {
        private List<string> inputs = new List<string>();
        private List<List<char>> inputGrid = new List<List<char>>();
        private List<char> collectedKeys = new List<char>();

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

            inputGrid = inputs.Select(i => i.ToCharArray().ToList()).ToList();
        }

        public string Part1()
        {
            Dictionary<char, Vector2Int> keyLocations = new Dictionary<char, Vector2Int>();
            Dictionary<char, Vector2Int> doorLocations = new Dictionary<char, Vector2Int>();
            Vector2Int startPos = new Vector2Int(0, 0);

            // get positions of important things from maze
            for (int i = 0; i < inputGrid.Count; i++)
            {
                List<char> line = inputGrid[i];
                for (int j = 0; j < line.Count; j++)
                {
                    char c = line[j];

                    if (char.IsUpper(c))
                    {
                        doorLocations.Add(c, new Vector2Int(j, i));
                    }
                    else if (char.IsLower(c))
                    {
                        //keys.Add(new Vector2Int(j, i), c);
                        keyLocations.Add(c, new Vector2Int(j, i));
                    }
                    else if (c == '@')
                    {
                        startPos = new Vector2Int(j, i);
                    }
                }
            }

            // generate all usable paths between keys and start pos
            ConcurrentDictionary<Vector2Int, List<PathToKey>> accessibleKeys = new ConcurrentDictionary<Vector2Int, List<PathToKey>>();
            List<Task> tasks = new List<Task>(27);
            foreach (var key in keyLocations)
            {
                tasks.Add(Task.Run(() =>
                {
                    accessibleKeys[key.Value] = GeneratePaths(key.Value, keyLocations, doorLocations, inputGrid);
                }));
            }

            tasks.Add(Task.Run(() =>
            {
                accessibleKeys[startPos] = GeneratePaths(startPos, keyLocations, doorLocations, inputGrid);
            }));

            Task.WaitAll(tasks.ToArray());

            // find a path
            return FindShortestPath(startPos, accessibleKeys).ToString();
        }

        /// <inheritdoc />
        public string Part2()
        {
            List<string> inputs2 = new List<string>();
            StreamReader reader = null;
            try
            {
                reader = new StreamReader($"Days/{GetType().Name}/input2.txt");
                while (!reader.EndOfStream)
                {
                    inputs2.Add(reader.ReadLine());
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

            var inputGrid2 = inputs2.Select(i => i.ToCharArray().ToList()).ToList();

            Dictionary<char, Vector2Int> keyLocations = new Dictionary<char, Vector2Int>();
            Dictionary<char, Vector2Int> doorLocations = new Dictionary<char, Vector2Int>();
            List<Vector2Int> startPos = new List<Vector2Int>();

            // get positions of important things from maze
            for (int i = 0; i < inputGrid2.Count; i++)
            {
                List<char> line = inputGrid2[i];
                for (int j = 0; j < line.Count; j++)
                {
                    char c = line[j];

                    if (char.IsUpper(c))
                    {
                        doorLocations.Add(c, new Vector2Int(j, i));
                    }
                    else if (char.IsLower(c))
                    {
                        //keys.Add(new Vector2Int(j, i), c);
                        keyLocations.Add(c, new Vector2Int(j, i));
                    }
                    else if (c == '@')
                    {
                        startPos.Add(new Vector2Int(j, i));
                    }
                }
            }

            // generate all usable paths between keys and start pos
            ConcurrentDictionary<Vector2Int, List<PathToKey>> accessibleKeys = new ConcurrentDictionary<Vector2Int, List<PathToKey>>();
            List<Task> tasks = new List<Task>(30);
            foreach (var key in keyLocations)
            {
                tasks.Add(Task.Run(() =>
                {
                    accessibleKeys[key.Value] = GeneratePaths(key.Value, keyLocations, doorLocations, inputGrid2);
                }));
            }

            foreach (Vector2Int pos in startPos)
            {
                tasks.Add(Task.Run(() =>
                {
                    accessibleKeys[pos] = GeneratePaths(pos, keyLocations, doorLocations, inputGrid2);
                }));
            }
            

            Task.WaitAll(tasks.ToArray());

            // find paths
            ConcurrentBag<int> totals = new ConcurrentBag<int>();
            tasks.Clear();
            foreach (Vector2Int pos in startPos)
            {
                tasks.Add(Task.Run(() =>
                {
                    totals.Add(FindShortestPath2(pos, accessibleKeys));
                }));
            }

            Task.WaitAll(tasks.ToArray());

            // sum and return
            return totals.Sum().ToString();
        }

        private int FindShortestPath2(Vector2Int startPos, ConcurrentDictionary<Vector2Int, List<PathToKey>> accessibleKeys)
        {
            int shortest = int.MaxValue;

            Queue<State> locationsLeft = new Queue<State>();
            HashSet<State> visited = new HashSet<State>();
            locationsLeft.Enqueue(new State()
            {
                Position = startPos,
                DistanceTraveled = 0,
                KeyMask = 0,
            });

            int allKeys = 0;
            foreach (PathToKey pathToKey in accessibleKeys[startPos])
            {
                allKeys |= pathToKey.KeyCode;
            }

            while (locationsLeft.Count > 0)
            {
                State currentLocation = locationsLeft.Dequeue();

                // check if already visited
                if (visited.Contains(currentLocation))
                {
                    continue;
                }
                visited.Add(currentLocation);

                // check if keyring is complete
                if (currentLocation.KeyMask == allKeys)
                {
                    if (currentLocation.DistanceTraveled < shortest)
                    {
                        shortest = currentLocation.DistanceTraveled;
                    }

                    continue;
                }

                // cut off routes which are too long
                if (currentLocation.DistanceTraveled > 2000)
                {
                    continue;
                }

                foreach (PathToKey pathToKey in accessibleKeys[currentLocation.Position])
                {
                    // add to queue, with added key and distance
                    locationsLeft.Enqueue(new State()
                    {
                        Position = pathToKey.KeyPosition,
                        DistanceTraveled = currentLocation.DistanceTraveled + pathToKey.Distance,
                        KeyMask = currentLocation.KeyMask | pathToKey.KeyCode
                    });
                }
            }

            return shortest;
        }

        private int FindShortestPath(Vector2Int startPos, ConcurrentDictionary<Vector2Int, List<PathToKey>> accessibleKeys)
        {
            int shortest = int.MaxValue;

            Queue<State> locationsLeft = new Queue<State>();
            HashSet<State> visited = new HashSet<State>();
            locationsLeft.Enqueue(new State()
            {
                Position = startPos,
                DistanceTraveled = 0,
                KeyMask = 0,
            });

            int allKeys = 0;
            for (int i = 0; i < accessibleKeys.Count - 1; i++)
            {
                allKeys += 1 << i;
            }

            while (locationsLeft.Count > 0)
            {
                State currentLocation = locationsLeft.Dequeue();

                // check if already visited
                if (visited.Contains(currentLocation))
                {
                    continue;
                }
                visited.Add(currentLocation);

                // check if keyring is complete
                if (currentLocation.KeyMask == allKeys)
                {
                    if (currentLocation.DistanceTraveled < shortest)
                    {
                        shortest = currentLocation.DistanceTraveled;
                    }

                    continue;
                }

                foreach (PathToKey pathToKey in accessibleKeys[currentLocation.Position])
                {
                    // check if key was already collected
                    if ((pathToKey.KeyCode & currentLocation.KeyMask) == 0)
                    {
                        // check if it can be reached with current keys
                        if ((pathToKey.DoorMask | currentLocation.KeyMask) == currentLocation.KeyMask)
                        {
                            // add to queue, with added key and distance
                            locationsLeft.Enqueue(new State()
                            {
                                Position = pathToKey.KeyPosition,
                                DistanceTraveled = currentLocation.DistanceTraveled + pathToKey.Distance,
                                KeyMask = currentLocation.KeyMask | pathToKey.KeyCode
                            });
                        }
                    }
                }
            }

            return shortest;
        }

        private List<PathToKey> GeneratePaths(Vector2Int startPos, Dictionary<char, Vector2Int> keyLocations, Dictionary<char, Vector2Int> doorLocations, List<List<char>> inputGrid)
        {
            List<PathToKey> output = new List<PathToKey>();
            Queue<PathToKey> locationsLeft = new Queue<PathToKey>();
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

            locationsLeft.Enqueue(new PathToKey()
            {
                KeyPosition = startPos,
                Distance = 0,
                DoorMask = 0
            });

            while (locationsLeft.Count > 0)
            {
                PathToKey currentLocation = locationsLeft.Dequeue();

                // duplication check
                if (visited.Contains(currentLocation.KeyPosition))
                {
                    continue;
                }

                visited.Add(currentLocation.KeyPosition);

                char c = inputGrid[currentLocation.KeyPosition.Y][currentLocation.KeyPosition.X];

                // wall
                if (c == '#')
                {
                    continue;
                }

                // key
                if (char.IsLower(c) && currentLocation.Distance > 0)
                {
                    // output
                    currentLocation.KeyCode = 1 << (c - 'a');
                    output.Add(currentLocation);
                }
                // door (obstacle)
                else if (char.IsUpper(c))
                {
                    currentLocation.DoorMask |= (1 << c - 'A');
                }

                // generate locations around current tile
                List<Vector2Int> around = Around(currentLocation.KeyPosition);
                foreach (Vector2Int newPos in around)
                {
                    locationsLeft.Enqueue(new PathToKey()
                    {
                        Distance = currentLocation.Distance + 1,
                        KeyPosition = newPos,
                        DoorMask = currentLocation.DoorMask,
                    });
                }
            }

            return output;
        }

        private class State
        {
            public Vector2Int Position { get; set; }
            public int DistanceTraveled { get; set; }
            public int KeyMask { get; set; }

            public bool Equals(State other)
            {
                return Equals(Position, other.Position) && KeyMask == other.KeyMask && DistanceTraveled == other.DistanceTraveled;
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((State) obj);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (Position != null ? Position.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ DistanceTraveled;
                    hashCode = (hashCode * 397) ^ KeyMask;
                    return hashCode;
                }
            }
        }

        public class PathToKey
        {
            public Vector2Int KeyPosition { get; set; }
            public int Distance { get; set; }
            public int DoorMask { get; set; }
            public int KeyCode { get; set; }

            public bool Equals(PathToKey other)
            {
                return Equals(KeyPosition, other.KeyPosition) && Distance == other.Distance && DoorMask == other.DoorMask;
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((PathToKey) obj);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (KeyPosition != null ? KeyPosition.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ Distance;
                    hashCode = (hashCode * 397) ^ DoorMask;
                    return hashCode;
                }
            }
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
    }
}