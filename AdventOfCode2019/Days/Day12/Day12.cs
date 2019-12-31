using System;
using System.Collections.Generic;
using System.IO;
using AdventOfCode2019.Tools.Vectors;

namespace AdventOfCode2019.Days.Day12
{
    public class Day12 : IDay
    {
        private List<string> inputs = new List<string>();
        private List<Vector3Int> startPositions = new List<Vector3Int>();

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

            foreach (string line in inputs)
            {
                string newLine = line.Replace("<", "").Replace(">", "").Replace("X", "").Replace("Y", "").Replace("Z", "").Replace(" ", "");
                // TODO split numbers into vector
            }

            startPositions.Add(new Vector3Int(-16, 15, -9));
            startPositions.Add(new Vector3Int(-14, 5, 4));
            startPositions.Add(new Vector3Int(2, 0, 6));
            startPositions.Add(new Vector3Int(-3, 18, 9));
        }

        /// <inheritdoc />
        public string Part1()
        {
            // create moons
            List<Moon> moons = new List<Moon>();
            foreach (Vector3Int pos in startPositions)
            {
                moons.Add(new Moon()
                {
                    Position = pos,
                    Velocity = Vector3Int.Zero
                });
            }

            // simulate 1k steps
            for (int i = 0; i < 1_000; i++)
            {
                // change velocity
                foreach (Moon m1 in moons)
                {
                    foreach (Moon m2 in moons)
                    {
                        // skip over self
                        if (m1 == m2)
                        {
                            continue;
                        }

                        // X
                        if (m2.Position.X > m1.Position.X)
                        {
                            m1.Velocity.X += 1;
                        }
                        else if (m2.Position.X < m1.Position.X)
                        {
                            m1.Velocity.X -= 1;
                        }

                        // Y
                        if (m2.Position.Y > m1.Position.Y)
                        {
                            m1.Velocity.Y += 1;
                        }
                        else if (m2.Position.Y < m1.Position.Y)
                        {
                            m1.Velocity.Y -= 1;
                        }

                        // Z
                        if (m2.Position.Z > m1.Position.Z)
                        {
                            m1.Velocity.Z += 1;
                        }
                        else if (m2.Position.Z < m1.Position.Z)
                        {
                            m1.Velocity.Z -= 1;
                        }
                    }
                }

                // move moons
                foreach (Moon m in moons)
                {
                    m.Position += m.Velocity;
                }
            }

            // calculate energy
            int totalEnergy = 0;
            foreach (Moon m in moons)
            {
                totalEnergy += m.TotalEnergy;
            }

            return $"Total energy {totalEnergy}";
        }

        /// <inheritdoc />
        public string Part2()
        {
            // create moons
            List<Moon> moons = new List<Moon>();
            foreach (Vector3Int pos in startPositions)
            {
                moons.Add(new Moon()
                {
                    Position = pos,
                    Velocity = Vector3Int.Zero
                });
            }

            bool[] looped = new bool[3];
            int[] loopedAt = new int[3];

            // simulate 1k steps
            for (int i = 1; i < 1_000_000_000; i++)
            {
                // change velocity
                foreach (Moon m1 in moons)
                {
                    foreach (Moon m2 in moons)
                    {
                        // skip over self
                        if (m1 == m2)
                        {
                            continue;
                        }

                        // X
                        if (m2.Position.X > m1.Position.X)
                        {
                            m1.Velocity.X += 1;
                        }
                        else if (m2.Position.X < m1.Position.X)
                        {
                            m1.Velocity.X -= 1;
                        }

                        // Y
                        if (m2.Position.Y > m1.Position.Y)
                        {
                            m1.Velocity.Y += 1;
                        }
                        else if (m2.Position.Y < m1.Position.Y)
                        {
                            m1.Velocity.Y -= 1;
                        }

                        // Z
                        if (m2.Position.Z > m1.Position.Z)
                        {
                            m1.Velocity.Z += 1;
                        }
                        else if (m2.Position.Z < m1.Position.Z)
                        {
                            m1.Velocity.Z -= 1;
                        }
                    }
                }

                // move moons
                foreach (Moon m in moons)
                {
                    m.Position += m.Velocity;
                }

                bool same;
                if (!looped[0])
                {
                    same = true;
                    for (int j = 0; j < moons.Count; j++)
                    {
                        if (moons[j].Position.X != startPositions[j].X || moons[j].Velocity.X != 0)
                        {
                            same = false;
                            break;
                        }
                    }

                    if (same)
                    {
                        looped[0] = true;
                        loopedAt[0] = i;
                    }
                }

                if (!looped[1])
                {
                    same = true;
                    for (int j = 0; j < moons.Count; j++)
                    {
                        if (moons[j].Position.Y != startPositions[j].Y || moons[j].Velocity.Y != 0)
                        {
                            same = false;
                            break;
                        }
                    }

                    if (same)
                    {
                        looped[1] = true;
                        loopedAt[1] = i;
                    }
                }

                if (!looped[2])
                {
                    same = true;
                    for (int j = 0; j < moons.Count; j++)
                    {
                        if (moons[j].Position.Z != startPositions[j].Z || moons[j].Velocity.Z != 0)
                        {
                            same = false;
                            break;
                        }
                    }

                    if (same)
                    {
                        looped[2] = true;
                        loopedAt[2] = i;
                    }
                }

                // if all are done then end the loop
                same = true;
                for (int j = 0; j < 3; j++)
                {
                    if (!looped[j])
                    {
                        same = false;
                        break;
                    }
                }

                // if all have looped
                if (same)
                {
                    break;
                }

                /*
                // check if they line up
                same = true;
                for (int j = 0; j < moons.Count; j++)
                {
                    if (!moons[j].Position.Equals(startPositions[j]))
                    {
                        same = false;
                        break;
                    }
                }

                if (same)
                {
                    return $"loops after {i + 1} steps";
                    break;
                }
                */

                if (i % 100_000 == 0)
                {
                    Console.WriteLine($"steps: {i}");
                }
            }

            long loopTime = LeastCommonMultiple(loopedAt[0], LeastCommonMultiple(loopedAt[1], loopedAt[2]));

            return $"loops after {loopTime} steps";
        }

        private class Moon
        {
            /// <summary>
            /// Position in SPAAAAAAAAAAAAAAACE
            /// </summary>
            public Vector3Int Position { get; set; }

            /// <summary>
            /// Current velocity of this moon
            /// </summary>
            public Vector3Int Velocity { get; set; }

            /// <summary>
            /// sum of absolutes of axis of position
            /// </summary>
            public int PotentialEnergy => Math.Abs(Position.X) + Math.Abs(Position.Y) + Math.Abs(Position.Z);

            /// <summary>
            /// sum of absolutes of axis of velocities
            /// </summary>
            public int KineticEnergy => Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) + Math.Abs(Velocity.Z);
            
            /// <summary>
            /// E_kinetic  * E_potential
            /// </summary>
            public int TotalEnergy => KineticEnergy * PotentialEnergy;
        }

        private long GreatestCommonDivider(long a, long b)
        {
            while (a != b)
            {
                if (a > b)
                {
                    a -= b;
                }
                else
                {
                    b -= a;
                }
            }

            return a;
        }

        private long LeastCommonMultiple(long a, long b)
        {
            return (a * b) / GreatestCommonDivider(a, b);
        }
    }
}