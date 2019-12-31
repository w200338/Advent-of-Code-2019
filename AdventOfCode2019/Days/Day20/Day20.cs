using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2019.Tools._2DShapes;
using AdventOfCode2019.Tools.Vectors;

namespace AdventOfCode2019.Days.Day20
{
    public class Day20 : IDay
    {
        private List<string> inputs = new List<string>();
        private List<List<char>> inputGrid = new List<List<char>>();

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

            inputGrid = inputs.Select(line => line.Replace(' ', '#').ToCharArray().ToList()).ToList();
        }

        /// <inheritdoc />
        public string Part1()
        {
            // find all portals
            List<Portal> portals = new List<Portal>();
            for (int i = 0; i < inputGrid.Count; i++)
            {
                for (int j = 0; j < inputGrid[i].Count; j++)
                {
                    // teleporter part
                    if (char.IsUpper(inputGrid[i][j]))
                    {
                        // check right
                        if (j < inputGrid[i].Count - 1 && char.IsUpper(inputGrid[i][j + 1]))
                        {
                            // check if open tile is right
                            if (j + 1 < inputGrid[i].Count - 1 && inputGrid[i][j + 2] == '.')
                            {
                                portals.Add(new Portal()
                                {
                                    Position = new Vector2Int(j + 2, i),
                                    //Position = new Vector2Int(i, j + 2),
                                    Name = $"{inputGrid[i][j]}{inputGrid[i][j + 1]}"
                                });
                            }
                            // otherwise it has to be left
                            else
                            {
                                portals.Add(new Portal()
                                {
                                    Position = new Vector2Int(j - 1, i),
                                    //Position = new Vector2Int(j - 2, i),
                                    Name = $"{inputGrid[i][j]}{inputGrid[i][j + 1]}"
                                });
                            }
                        }
                        // check down
                        else if (i < inputGrid.Count - 1 && char.IsUpper(inputGrid[i + 1][j]))
                        {
                            // check open tile below
                            if (i + 1 < inputGrid.Count - 1 && inputGrid[i + 2][j] == '.')
                            {
                                portals.Add(new Portal()
                                {
                                    Position = new Vector2Int(j, i + 2),
                                    //Position = new Vector2Int(i + 2,j),
                                    Name = $"{inputGrid[i][j]}{inputGrid[i + 1][j]}"
                                });
                            }
                            // otherwise it has to be above
                            else
                            {
                                portals.Add(new Portal()
                                {
                                    Position = new Vector2Int(j, i - 1),
                                    //Position = new Vector2Int(i - 1, j),
                                    Name = $"{inputGrid[i][j]}{inputGrid[i + 1][j]}"
                                });
                            }
                        }
                    }
                }
            }

            // generate all accessible portals
            Dictionary<Portal, List<PortalPath>> accessiblePortals = new Dictionary<Portal, List<PortalPath>>();
            foreach (Portal portal in portals)
            {
                accessiblePortals[portal] = GeneratePairs(portal, portals);
                //Console.WriteLine(accessiblePortals[portal].Count);
            }

            // link portals
            List<PortalPair> portalPairs = new List<PortalPair>();
            foreach (Portal portal in portals)
            {
                // combine on name
                PortalPair portalPair = portalPairs.FirstOrDefault(pair => pair.PortalA.Name.Equals(portal.Name));
                
                // new
                if (portalPair == null)
                {
                    portalPairs.Add(new PortalPair()
                    {
                        PortalA = portal
                    });
                }
                // existing
                else
                {
                    portalPair.PortalB = portal;
                    portalPair.Distance = Vector2Int.DistanceManhattan(portalPair.PortalA.Position, portalPair.PortalB.Position);
                }
            }

            Portal startPortal = portalPairs.FirstOrDefault(p => p.PortalA.Name == "AA")?.PortalA;
            Portal endPortal = portalPairs.FirstOrDefault(p => p.PortalA.Name == "ZZ")?.PortalA;

            return MinimumSteps(portalPairs, accessiblePortals, startPortal, endPortal).ToString();
        }

        private int MinimumSteps(List<PortalPair> pairs, Dictionary<Portal, List<PortalPath>> accessiblePortals, Portal startPortal, Portal endPortal)
        {
            int shortestRoute = int.MaxValue;

            HashSet<Path> seenPaths = new HashSet<Path>();

            Queue<Path> pathsLeft = new Queue<Path>();
            pathsLeft.Enqueue(new Path()
            {
                CurrentPortal = startPortal,
            });

            while (pathsLeft.Count > 0)
            {
                Path currentPath = pathsLeft.Dequeue();

                // skip over seen
                if (seenPaths.Contains(currentPath))
                {
                    continue;
                }

                // new path that hasn't been seen before
                seenPaths.Add(currentPath);

                // check if ended
                if (currentPath.CurrentPortal.Equals(endPortal))
                {
                    if (currentPath.TotalLength + currentPath.TeleportersTaken - 1 < shortestRoute)
                    {
                        shortestRoute = currentPath.TotalLength + currentPath.TeleportersTaken - 1;
                    }

                    continue;
                }

                // try another couple teleporters till they were basically all seen
                if (currentPath.TeleportersTaken < 29)
                {
                    foreach (PortalPath portalPath in accessiblePortals[currentPath.CurrentPortal])
                    {
                        // if there's no other portal
                        if (portalPath.OtherPortal == null || portalPath.OtherPortal.Equals(currentPath.CurrentPortal))
                        {
                            continue;
                        }

                        PortalPair pair = pairs.FirstOrDefault(p => p.PortalA.Position.Equals(portalPath.OtherPortal.Position) || (p.PortalB != null && p.PortalB.Position.Equals(portalPath.OtherPortal.Position)));
                        Portal newPortal = null;
                        if (pair.PortalA.Position.Equals(portalPath.OtherPortal.Position))
                        {
                            newPortal = pair.PortalB;
                        }
                        else
                        {
                            newPortal = pair.PortalA;
                        }

                        // for ZZ
                        if (newPortal == null)
                        {
                            newPortal = pair.PortalA;
                        }

                        if (newPortal != null)
                        {
                            pathsLeft.Enqueue(new Path()
                            {
                                CurrentPortal = newPortal,
                                TeleportersTaken = currentPath.TeleportersTaken + 1,
                                TotalLength = currentPath.TotalLength + portalPath.Distance
                            });
                        }
                    }
                }
            }

            return shortestRoute;
        }

        /// <inheritdoc />
        public string Part2()
        {
            // find all portals
            List<Portal> portals = new List<Portal>();
            for (int i = 0; i < inputGrid.Count; i++)
            {
                for (int j = 0; j < inputGrid[i].Count; j++)
                {
                    // teleporter part
                    if (char.IsUpper(inputGrid[i][j]))
                    {
                        // check right
                        if (j < inputGrid[i].Count - 1 && char.IsUpper(inputGrid[i][j + 1]))
                        {
                            // check if open tile is right
                            if (j + 1 < inputGrid[i].Count - 1 && inputGrid[i][j + 2] == '.')
                            {
                                portals.Add(new Portal()
                                {
                                    Position = new Vector2Int(j + 2, i),
                                    //Position = new Vector2Int(i, j + 2),
                                    Name = $"{inputGrid[i][j]}{inputGrid[i][j + 1]}"
                                });
                            }
                            // otherwise it has to be left
                            else
                            {
                                portals.Add(new Portal()
                                {
                                    Position = new Vector2Int(j - 1, i),
                                    //Position = new Vector2Int(j - 2, i),
                                    Name = $"{inputGrid[i][j]}{inputGrid[i][j + 1]}"
                                });
                            }
                        }
                        // check down
                        else if (i < inputGrid.Count - 1 && char.IsUpper(inputGrid[i + 1][j]))
                        {
                            // check open tile below
                            if (i + 1 < inputGrid.Count - 1 && inputGrid[i + 2][j] == '.')
                            {
                                portals.Add(new Portal()
                                {
                                    Position = new Vector2Int(j, i + 2),
                                    //Position = new Vector2Int(i + 2,j),
                                    Name = $"{inputGrid[i][j]}{inputGrid[i + 1][j]}"
                                });
                            }
                            // otherwise it has to be above
                            else
                            {
                                portals.Add(new Portal()
                                {
                                    Position = new Vector2Int(j, i - 1),
                                    //Position = new Vector2Int(i - 1, j),
                                    Name = $"{inputGrid[i][j]}{inputGrid[i + 1][j]}"
                                });
                            }
                        }
                    }
                }
            }

            // generate all accessible portals
            Console.WriteLine("Generating paths");
            Dictionary<Portal, List<PortalPath>> accessiblePortals = new Dictionary<Portal, List<PortalPath>>();
            foreach (Portal portal in portals)
            {
                // skip over ZZ
                if (portal.Name == "ZZ")
                {
                    continue;
                }

                List<PortalPath> paths = GeneratePairs(portal, portals);
                accessiblePortals[portal] = paths;

                foreach (var path in paths)
                {
                    Console.WriteLine($"{portal.Name} to {path.OtherPortal.Name} ({path.Distance} steps)");
                }

                //Console.WriteLine(accessiblePortals[portal].Count);
            }

            // link portals
            List<PortalPair> portalPairs = new List<PortalPair>();
            foreach (Portal portal in portals)
            {
                // combine on name
                PortalPair portalPair = portalPairs.FirstOrDefault(pair => pair.PortalA.Name.Equals(portal.Name));

                // new
                if (portalPair == null)
                {
                    portalPairs.Add(new PortalPair()
                    {
                        PortalA = portal
                    });
                }
                // existing
                else
                {
                    portalPair.PortalB = portal;
                    portalPair.Distance = Vector2Int.DistanceManhattan(portalPair.PortalA.Position, portalPair.PortalB.Position);
                }
            }

            Portal startPortal = portalPairs.FirstOrDefault(p => p.PortalA.Name == "AA")?.PortalA;
            Portal endPortal = portalPairs.FirstOrDefault(p => p.PortalA.Name == "ZZ")?.PortalA;

            return MinimumSteps2(portalPairs, accessiblePortals, startPortal, endPortal).ToString();
        }

        private int MinimumSteps2(List<PortalPair> pairs, Dictionary<Portal, List<PortalPath>> accessiblePortals, Portal startPortal, Portal endPortal)
        {
            int shortestRoute = int.MaxValue;

            HashSet<Path> seenPaths = new HashSet<Path>();

            Queue<Path> pathsLeft = new Queue<Path>();
            pathsLeft.Enqueue(new Path()
            {
                CurrentPortal = startPortal,
            });

            int minX = 2;
            int minY = 2;
            int maxX = Math.Max(pairs.Select(p => p.PortalA.Position.X).Max(), pairs.Select(p => p.PortalB == null ? 0 : p.PortalB.Position.X).Max());
            int maxY = Math.Max(pairs.Select(p => p.PortalA.Position.Y).Max(), pairs.Select(p => p.PortalB == null ? 0 : p.PortalB.Position.Y).Max());

            while (pathsLeft.Count > 0)
            {
                Path currentPath = pathsLeft.Dequeue();

                // skip over seen
                if (seenPaths.Contains(currentPath))
                {
                    continue;
                }

                // new path that hasn't been seen before
                seenPaths.Add(currentPath);

                // skip over start
                if (currentPath.Level > 0 && currentPath.CurrentPortal.Equals(startPortal))
                {
                    continue;
                }

                // skip over end
                if (currentPath.CurrentPortal.Equals(endPortal))
                {
                    continue;
                }

                /*
                // check if ended
                if (currentPath.CurrentPortal.Equals(endPortal))
                {
                    if (currentPath.Level == 0 && currentPath.TotalLength + currentPath.TeleportersTaken - 1 < shortestRoute)
                    {
                        shortestRoute = currentPath.TotalLength + currentPath.TeleportersTaken - 1;
                        Console.WriteLine($"Found route of {currentPath.TotalLength + currentPath.TeleportersTaken - 1} steps using {currentPath.TeleportersTaken} teleporters");
                    }

                    //Console.WriteLine($"Found route of {currentPath.TotalLength + currentPath.TeleportersTaken - 1} steps using {currentPath.TeleportersTaken} teleporters");


                    continue;
                }
                */

                // try another couple teleporters till they were basically all seen
                if (currentPath.TeleportersTaken < 200)
                {
                    foreach (PortalPath portalPath in accessiblePortals[currentPath.CurrentPortal])
                    {
                        // if there's no other portal or it's the current portal for some reason, skip it
                        if (portalPath.OtherPortal == null || portalPath.OtherPortal.Equals(currentPath.CurrentPortal))
                        {
                            continue;
                        }

                        PortalPair pair = pairs.FirstOrDefault(p => p.PortalA.Position.Equals(portalPath.OtherPortal.Position) || (p.PortalB != null && p.PortalB.Position.Equals(portalPath.OtherPortal.Position)));
                        Portal newPortal = null;
                        if (pair.PortalA.Position.Equals(portalPath.OtherPortal.Position))
                        {
                            newPortal = pair.PortalB;
                        }
                        else
                        {
                            newPortal = pair.PortalA;
                        }

                        // for ZZ
                        if (newPortal == null)
                        {
                            newPortal = pair.PortalA;
                        }

                        if (newPortal != null)
                        {
                            bool isOutside = portalPath.OtherPortal.Position.X == minX ||
                                             portalPath.OtherPortal.Position.X == maxX ||
                                             portalPath.OtherPortal.Position.Y == minY ||
                                             portalPath.OtherPortal.Position.Y == maxY;

                            // outside
                            if (isOutside)
                            {
                                // found it
                                if (currentPath.Level == 0 && newPortal.Name == "ZZ")
                                {
                                    return currentPath.TotalLength + portalPath.Distance + currentPath.TeleportersTaken;
                                }

                                if (currentPath.Level > 0)
                                {
                                    pathsLeft.Enqueue(new Path()
                                    {
                                        CurrentPortal = newPortal,
                                        TeleportersTaken = currentPath.TeleportersTaken + 1,
                                        TotalLength = currentPath.TotalLength + portalPath.Distance,
                                        Level = currentPath.Level - 1
                                    });
                                }
                            }
                            // inside
                            else
                            {
                                pathsLeft.Enqueue(new Path()
                                {
                                    CurrentPortal = newPortal,
                                    TeleportersTaken = currentPath.TeleportersTaken + 1,
                                    TotalLength = currentPath.TotalLength + portalPath.Distance,
                                    Level = currentPath.Level + 1
                                });
                            }
                            /*
                            pathsLeft.Enqueue(new Path()
                            {
                                CurrentPortal = newPortal,
                                TeleportersTaken = currentPath.TeleportersTaken + 1,
                                TotalLength = currentPath.TotalLength + portalPath.Distance,
                                Level = currentPath.Level + ((isOutside) ? +1 : -1)
                            });
                            */
                        }
                    }
                }
            }

            return shortestRoute;
        }

        private class Path
        {
            public int TeleportersTaken { get; set; }
            public Portal CurrentPortal { get; set; }
            public int TotalLength { get; set; }
            public int Level { get; set; }

            public bool Equals(Path other)
            {
                //return TeleportersTaken == other.TeleportersTaken && Equals(CurrentPortal, other.CurrentPortal) && TotalLength == other.TotalLength && Level == other.Level;
                return Equals(CurrentPortal, other.CurrentPortal) && TotalLength == other.TotalLength && Level == other.Level;
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Path) obj);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (CurrentPortal != null ? CurrentPortal.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ TotalLength;
                    hashCode = (hashCode * 397) ^ Level;
                    return hashCode;
                }
            }


        }

        private List<PortalPath> GeneratePairs(Portal startPortal, List<Portal> allPortals)
        {
            RectangleInt bounds = new RectangleInt(Vector2Int.Zero, new Vector2Int(inputGrid[0].Count - 1, inputGrid.Count - 1));
            //Vector2Int startLocation = startPortal.PositionA;

            //List<PortalPair> output = new List<PortalPair>();
            List<PortalPath> output = new List<PortalPath>();
            HashSet<Vector2Int> visitedLocations = new HashSet<Vector2Int>();

            Queue<Vector2Int> locationsLeft = new Queue<Vector2Int>();
            Queue<int> distances = new Queue<int>();
            locationsLeft.Enqueue(startPortal.Position);
            //locationsLeft.Enqueue(startPortal.PositionB);
            distances.Enqueue(0);
            //distances.Enqueue(0);

            while (locationsLeft.Count > 0)
            {
                // get current position from queue
                Vector2Int currentPos = locationsLeft.Dequeue();
                int distance = distances.Dequeue();
                
                // skip over an already visited location
                if (visitedLocations.Contains(currentPos))
                {
                    continue;
                }

                visitedLocations.Add(currentPos);

                char currentTile = inputGrid[currentPos.Y][currentPos.X];

                // teleporter
                if (char.IsUpper(currentTile))
                {
                    Portal otherPortal = null;// = allPortals.FirstOrDefault(p => p.Position.Equals(currentPos));
                    List<Vector2Int> around = Around(currentPos);
                    foreach (var pos in around)
                    {
                        //if (allPortals.FirstOrDefault(p => p.Position.Equals(pos)) != null)
                        if (inputGrid[pos.Y][pos.X] == '.')
                        {
                            otherPortal = allPortals.FirstOrDefault(p => p.Position.Equals(pos));

                            if (otherPortal != null && !otherPortal.Equals(startPortal) && otherPortal.Name != "AA")
                            {
                                output.Add(new PortalPath()
                                {
                                    OtherPortal = otherPortal,
                                    Distance = distance - 1
                                });

                                break;
                            }
                        }
                    }

                    //Portal otherPortal = allPortals.FirstOrDefault(p => p.PositionA.Equals(currentPos) || p.PositionB.Equals(currentPos));
                }
                // open space
                else if (currentTile == '.')
                {
                    List<Vector2Int> around = Around(currentPos);
                    foreach (Vector2Int aroundPos in around)
                    {
                        if (bounds.IsInRectangle(aroundPos))
                        {
                            locationsLeft.Enqueue(aroundPos);
                            distances.Enqueue(distance + 1);
                        }
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

        /// <summary>
        /// Pair of portals
        /// </summary>
        private class PortalPair
        {
            public Portal PortalA { get; set; }

            public Portal PortalB { get; set; }

            public int Distance { get; set; }
        }

        /// <summary>
        /// path to a portal
        /// </summary>
        private class PortalPath
        {
            public Portal OtherPortal { get; set; }
            public int Distance { get; set; }
        }

        private class Portal
        {
            public Vector2Int Position { get; set; }
            public string Name { get; set; }

            public bool Equals(Portal other)
            {
                return Name == other.Name && Position.Equals(other.Position);
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Portal) obj);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                return (Name != null ? Name.GetHashCode() : 0);
            }
        }
    }
}