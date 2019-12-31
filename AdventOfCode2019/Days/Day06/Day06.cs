using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2019.Days.Day06
{
    public class Day06 : IDay
    {
        private List<string> inputs = new List<string>();

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
        }

        /// <inheritdoc />
        public string Part1()
        {
            Orbit comOrbit = new Orbit() {Name = "COM"};

            List<Orbit> orbits = new List<Orbit>();
            orbits.Add(comOrbit);

            // place all orbits into list
            foreach (string input in inputs)
            {
                string[] names = input.Split(')');
                orbits.Add(new Orbit()
                {
                    Name = names[1],
                    DirectOrbit = names[0]
                });
            }

            // do checksum
            int checkSum = 0;
            foreach (Orbit orbit in orbits)
            {
                Orbit currentOrbit = orbit;
                while (!currentOrbit.Equals(comOrbit))
                {
                    currentOrbit = orbits[orbits.IndexOf(new Orbit() {Name = currentOrbit.DirectOrbit})];
                    checkSum++;
                }
            }

            return $"{checkSum}";
        }

        /// <inheritdoc />
        public string Part2()
        {
            Orbit comOrbit = new Orbit() { Name = "COM" };

            List<Orbit> orbits = new List<Orbit>();
            orbits.Add(comOrbit);

            // place all orbits into list
            foreach (string input in inputs)
            {
                string[] names = input.Split(')');
                orbits.Add(new Orbit()
                {
                    Name = names[1],
                    DirectOrbit = names[0]
                });
            }

            // do path from YOU to COM
            List<string> pathYOU = new List<string>();
            Orbit currentOrbit = orbits[orbits.IndexOf(new Orbit() { Name = "YOU" })];
            while (!currentOrbit.Equals(comOrbit))
            {
                currentOrbit = orbits[orbits.IndexOf(new Orbit() { Name = currentOrbit.DirectOrbit })];
                pathYOU.Add(currentOrbit.Name);
            }

            // do path from SAN to COM
            List<string> pathSAN = new List<string>();
            currentOrbit = orbits[orbits.IndexOf(new Orbit() { Name = "SAN" })];
            while (!currentOrbit.Equals(comOrbit))
            {
                currentOrbit = orbits[orbits.IndexOf(new Orbit() { Name = currentOrbit.DirectOrbit })];
                pathSAN.Add(currentOrbit.Name);
            }

            // go backwards over path till a match is found
            for (int i = 0; i < pathYOU.Count; i++)
            {
                for (int j = 0; j < pathSAN.Count; j++)
                {
                    if (pathYOU[i].Equals(pathSAN[j]))
                    {
                        return $"{i + j}\n{pathYOU[i]} and {pathSAN[j]}";
                    }
                }
            }

            return $"";
        }
    }

    public class Orbit
    {
        /// <summary>
        /// Name of orbit
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Body which is getting orbited directly
        /// </summary>
        public string DirectOrbit { get; set; }

        public bool Equals(Orbit other)
        {
            return Name == other.Name;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Orbit) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}