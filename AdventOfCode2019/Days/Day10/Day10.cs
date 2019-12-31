using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2019.Tools._2DShapes;
using AdventOfCode2019.Tools.Vectors;

namespace AdventOfCode2019.Days.Day10
{
    public class Day10 : IDay
    {
        private Rectangle grid;
        private List<string> inputs = new List<string>();
        private List<Vector2Int> asteroids = new List<Vector2Int>();

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

            // to 2D array
            for (int i = 0; i < inputs.Count; i++)
            {
                for (int j = 0; j < inputs[i].Length; j++)
                {
                    if (inputs[i][j] == '#')
                    {
                        asteroids.Add(new Vector2Int(j, i));
                    }
                }
            }

            grid = new Rectangle(Vector2.Zero, new Vector2(inputs[0].Length, inputs.Count));
        }

        /// <inheritdoc />
        public string Part1()
        {
            
            Vector2Int bestPos = Vector2Int.Zero;
            int bestPosAmount = 0;

            foreach (Vector2Int asteroid in asteroids)
            {
                int amount = 0;
                foreach (Vector2Int otherAsteroid in asteroids)
                {
                    // skip over self
                    if (asteroid.Equals(otherAsteroid))
                    {
                        continue;
                    }

                    amount += (CheckPath(asteroid, otherAsteroid)) ? 1 : 0;
                }

                if (amount > bestPosAmount)
                {
                    bestPosAmount = amount;
                    bestPos = asteroid;
                }
            }

            return $"{bestPos.X} {bestPos.Y} can see {bestPosAmount}";
            

            return "";
        }

        /// <inheritdoc />
        public string Part2()
        {
            Vector2Int baseLocation = new Vector2Int(22, 25);
            asteroids.Remove(baseLocation); // can't hit the base
            
            List<AsteroidHit> asteroidHits = new List<AsteroidHit>();

            // end the suffering v4, please
            // go through every asteroid, check if it's in direct line of sight because there's 286 of those
            // then calculate the angle between the two points, then rotate that by -90 degrees (counter-clockwise) because following standards is for losers obviously
            foreach (Vector2Int asteroid in asteroids)
            {
                if (CheckPath(baseLocation, asteroid))
                {
                    asteroidHits.Add(new AsteroidHit()
                    {
                        AmountInFront = 0,
                        Angle = GetAngle(baseLocation, asteroid),
                        Position = asteroid
                    });
                }
            }
            

            /*
            List<Vector2Int> livingAsteroids = new List<Vector2Int>(asteroids);
            int amountInFront = 0;
            while (livingAsteroids.Count > 0)
            {
                List<Vector2Int> toRemove = new List<Vector2Int>();

                foreach (Vector2Int asteroid in livingAsteroids)
                {
                    int hits = CountHits(baseLocation, asteroid);
                    if (hits == amountInFront)
                    {
                        asteroidHits.Add(new AsteroidHit()
                        {
                            Position = asteroid,
                            AmountInFront = hits,
                            Angle = GetAngle(baseLocation, asteroid)
                        });

                        toRemove.Add(asteroid);
                    }
                }

                foreach (Vector2Int removal in toRemove)
                {
                    livingAsteroids.Remove(removal);
                }

                amountInFront++;
            }
            */

            /*
            foreach (Vector2Int asteroid in asteroids)
            {
                int hits = CountHits(baseLocation, asteroid);
                asteroidHits.Add(new AsteroidHit()
                {
                    Position = asteroid,
                    AmountInFront = hits,
                    Angle = GetAngle(baseLocation, asteroid)
                });
            }
            */

            /*
            for (int i = 0; i < inputs.Count; i++)
            {
                for (int j = 0; j < inputs[0].Length; j++)
                {
                    Vector2Int asteroid = new Vector2Int(j, i);
                    if (asteroid.Equals(baseLocation))
                    {
                        continue;
                    }

                    int hits = CountHits(baseLocation, asteroid);
                    if (hits > -1)
                    {
                        asteroidHits.Add(new AsteroidHit()
                        {
                            Position = asteroid,
                            AmountInFront = hits,
                            Angle = GetAngle(baseLocation, asteroid)
                        });
                    }
                }
            }
            */

            // sort list by angle and hits
            asteroidHits = asteroidHits.OrderBy(a => a.Angle).ThenBy(a => a.AmountInFront).ToList();

            // do the thing with the 200th item
            return $"{asteroidHits[199].Position.X * 100 + asteroidHits[199].Position.Y}";

            // less than 901
            // higher than 109
            // not 727
            // 504
        }

        private class AsteroidHit
        {
            public Vector2Int Position { get; set; }
            public double Angle { get; set; }
            public int AmountInFront { get; set; }
        }

        /// <summary>
        /// Get angle in degrees
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private double GetAngle(Vector2Int a, Vector2Int b)
        {
            double angle = Math.Atan2(a.Y - b.Y, a.X - b.X) * 180 / Math.PI;
            if (angle < 0) // always needs to be positive
            {
                angle += 360;
            }

            angle -= 90; // rotate left by 90°

            if (angle < 0) // always needs to be positive
            {
                angle += 360;
            }

            return angle;
        }

        /// <summary>
        /// Count the amount of hits
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int CountHits(Vector2Int a, Vector2Int b)
        {
            int deltaX = b.X - a.X;
            int deltaY = b.Y - a.Y;
            int gcd = GreatestCommonDivider(deltaX, deltaY);

            if (deltaX != deltaY || deltaY != 0)
            {
                deltaX /= gcd;
                deltaY /= gcd;
            }

            int runningX = a.X + deltaX;
            int runningY = a.Y + deltaY;

            int hits = 0;

            while (true)
            {
                Vector2Int runningPos = new Vector2Int(runningX, runningY);

                // check if it goes out of bounds
                if (!grid.IsInRectangle(new Vector2(runningX, runningY)))
                {
                    return hits;
                }

                if (asteroids.Contains(runningPos))
                {
                    if (runningPos.Equals(b))
                    {
                        return hits;
                    }

                    hits++;
                    //return -1; // doesn't matter, there's 286 asteroids with direct line of sight
                }

                runningX += deltaX;
                runningY += deltaY;
            }
        }


        /// <summary>
        /// Check if direct path between two vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool CheckPath(Vector2Int a, Vector2Int b)
        {
            int deltaX = b.X - a.X;
            int deltaY = b.Y - a.Y;
            int gcd = GreatestCommonDivider(deltaX, deltaY);

            if (deltaX != deltaY || deltaY != 0)
            {
                deltaX /= gcd;
                deltaY /= gcd;
            }

            int runningX = a.X + deltaX;
            int runningY = a.Y + deltaY;

            while (true)
            {
                Vector2Int runningPos = new Vector2Int(runningX, runningY);

                // check if it goes out of bounds
                if (!grid.IsInRectangle(new Vector2(runningX, runningY)))
                {
                    return false;
                }

                if (asteroids.Contains(runningPos))
                {
                    if (runningPos.Equals(b))
                    {
                        return true;
                    }

                    return false;
                }

                runningX += deltaX;
                runningY += deltaY;
            }
        }

        public int GreatestCommonDivider(int a, int b)
        {
            // sign doesn't matter in this one
            a = Math.Abs(a);
            b = Math.Abs(b);

            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a == 0 ? b : a;
        }
    }
}