using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Days.Day14
{
    public class Day14 : IDay
    {
        private List<string> inputs = new List<string>();

        private List<Reaction> Reactions = new List<Reaction>();

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

            // turn every line into a chemical reaction
            foreach (string line in inputs)
            {
                List<Chemical> chemicals = line.Replace(" => ", ", ")                         // replace "=>" with "," so inputs are one line of comma separated chems
                                        .Split(',')                                            // split into an array of amounts and names
                                        .Select(s => s.Trim())                                              // trim whitespace
                                        .Select(c => new Chemical()                                         // create chemical from amount and name
                                        {
                                            Amount = int.Parse(c.Split(' ')[0].Trim()), 
                                            Name = c.Split(' ')[1].Trim()
                                        }).ToList();

                Reactions.Add(new Reaction()
                {
                    Output = chemicals.Last(),
                    Input = chemicals.Take(chemicals.Count - 1).ToList()
                });
            }
        }

        /// <summary>
        /// List of chemicals stored for other requirements
        /// </summary>
        private List<Chemical> storage = new List<Chemical>();

        /// <summary>
        /// Recursively calculate amount of ore required to make an amount of a chemical
        /// </summary>
        /// <param name="inputChemical"></param>
        /// <returns></returns>
        public long OreRequired(Chemical inputChemical)
        {
            if (inputChemical.Name.Equals("ORE"))
            {
                return inputChemical.Amount;
            }

            
            // check if in storage
            if (storage.Count(c => c.Name.Equals(inputChemical.Name)) > 0)
            {
                long minimumStored = Math.Min(storage.First(c => c.Name.Equals(inputChemical.Name)).Amount, inputChemical.Amount);
                inputChemical.Amount -= minimumStored;
                storage.First(c => c.Name.Equals(inputChemical.Name)).Amount -= minimumStored;
            }

            // forgot about this
            if (inputChemical.Amount == 0)
            {
                return 0;
            }

            // find required reaction and the amount needed for it
            Reaction reaction = Reactions.First(r => r.Output.Name.Equals(inputChemical.Name));
            long multiple = (long)Math.Ceiling((decimal)inputChemical.Amount / reaction.Output.Amount);

            // calculate ore cost
            long total = 0;
            foreach (Chemical chem in reaction.Input)
            {

                total += OreRequired(new Chemical()
                {
                    Name = chem.Name,
                    Amount = chem.Amount * multiple
                });
            }

            // add extra to storage
            long surplus = reaction.Output.Amount * multiple - inputChemical.Amount;
            if (storage.Count(r => r.Name.Equals(inputChemical.Name)) > 0)
            {
                storage.First(r => r.Name.Equals(inputChemical.Name)).Amount += surplus;
            }
            else
            {
                storage.Add(new Chemical()
                {
                    Name = inputChemical.Name,
                    Amount = surplus
                });
            }

            return total;
        }

        /// <inheritdoc />
        public string Part1()
        {
            // calculate cost of one fuel unit
            // answer: 143173
            return $"total ore needed {OreRequired(new Chemical() {Name = "FUEL", Amount = 1})}";
        }

        /// <inheritdoc />
        public string Part2()
        {
            // throw away storage
            storage.Clear();

            long totalOreNeeded = 1_000_000_000_000;


            // find upper boundary of search space
            long high = 2;
            while (true)
            {
                storage.Clear();
                long amount = OreRequired(new Chemical()
                {
                    Name = "FUEL",
                    Amount = high
                });

                if (amount > totalOreNeeded)
                {
                    break;
                }

                high *= 2;
            }

            // binary search
            long low = 1;
            while (low + 1 < high)
            {
                storage.Clear();

                long middle = (low + high) / 2;
                long amount = OreRequired(new Chemical()
                {
                    Name = "FUEL",
                    Amount = middle
                });

                if (amount > totalOreNeeded)
                {
                    high = middle;
                }
                else
                {
                    low = middle;
                }
            }

            // answer: 8845261
            return $"Produced {low} FUEL";
        }

        /// <summary>
        /// Class to keep track of a name and an amount
        /// </summary>
        public class Chemical
        {
            public string Name { get; set; }

            public long Amount { get; set; }

            public bool Equals(Chemical other)
            {
                return Name == other.Name && Amount == other.Amount;
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Chemical)obj);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ Amount.GetHashCode();
                }
            }

            public override string ToString()
            {
                return $"{Amount} {Name}";
            }
        }

        /// <summary>
        /// Reaction between input chemicals creates output chemical
        /// </summary>
        public class Reaction
        {
            public List<Chemical> Input { get; set; } = new List<Chemical>();

            public Chemical Output { get; set; }
        }
    }
}