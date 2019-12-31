using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2019.Days.Day22
{
    public class Day22 : IDay
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
            int deckSize = 10007;

            List<int> cards = Enumerable.Range(0, deckSize).ToList();

            foreach (string shuffleTech in inputs)
            {
                switch (shuffleTech.Split(' ')[1])
                {
                    // weird shuffle technique, treat original list as a queue, place number on the Nth place in a new list (keep going round till done)
                    case "with":
                        int size = int.Parse(shuffleTech.Split(' ')[3]);

                        List<int> newCards = Enumerable.Range(0, deckSize).ToList();
                        for (int i = 0; i < cards.Count; i++)
                        {
                            newCards[i * size % cards.Count] = cards[i];
                        }

                        cards = newCards;
                        break;

                    // deal into new stack
                    case "into":
                        // basically reverse the order
                        cards.Reverse();
                        break;

                    // cut deck
                    default:
                        int cutSize = int.Parse(shuffleTech.Split(' ')[1]);

                        // take N from the front and put them on the back
                        if (cutSize > 0)
                        {
                            List<int> cutList = cards.Take(cutSize).ToList();
                            cards.RemoveRange(0, cutSize);
                            cards.AddRange(cutList);
                        }
                        // take last N from the back and put them in the front
                        else
                        {
                            List<int> cutList = cards.Skip(cards.Count + cutSize).Take(Math.Abs(cutSize)).ToList();
                            cards.RemoveRange(cards.Count + cutSize, Math.Abs(cutSize));
                            cards.InsertRange(0, cutList);
                        }

                        break;
                }
            }

            // find card 2019
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i] == 2019)
                {
                    return i.ToString();
                }
            }

            return $"";
        }

        /// <inheritdoc />
        public string Part2()
        {
            BigInteger deckSize = new BigInteger(119315717514047);
            BigInteger timesToShuffle = new BigInteger(101741582076661);

            BigInteger multiplier = BigInteger.One;
            BigInteger offset = BigInteger.Zero;

            foreach (string shuffleTech in inputs)
            {
                switch (shuffleTech.Split(' ')[1])
                {
                    // weird shuffle technique, treat original list as a queue, place number on the Nth place in a new list (keep going round till done)
                    case "with":
                        int size = int.Parse(shuffleTech.Split(' ')[3]);

                        multiplier *= ModuloInverse(new BigInteger(size), deckSize);
                        break;

                    // deal into new stack
                    case "into":
                        // reverse the order
                        multiplier *= BigInteger.MinusOne;

                        // shift offset
                        offset += multiplier;
                        break;

                    // cut deck
                    default:
                        int cutSize = int.Parse(shuffleTech.Split(' ')[1]);

                        // shift offset
                        offset += new BigInteger(cutSize) * multiplier;
                        break;
                }
            }

            multiplier = PositiveMod(multiplier, deckSize);
            offset = PositiveMod(offset, deckSize);
            //multiplier %= deckSize;
            //offset %= deckSize;

            BigInteger totalIncrement = BigInteger.ModPow(multiplier, timesToShuffle, deckSize);
            BigInteger totalOffset = offset * (1 - BigInteger.ModPow(multiplier, timesToShuffle, deckSize)) * ModuloInverse(1 - multiplier, deckSize);

            totalIncrement = PositiveMod(totalIncrement, deckSize);
            totalOffset = PositiveMod(totalOffset, deckSize);
            //totalIncrement %= deckSize;
            //totalOffset %= deckSize;

            return ((totalOffset + 2020 * totalIncrement) % deckSize).ToString();

            // less than 119315717506989
        }

        /// <summary>
        /// Make sure the module answer is positive instead of just being the remainder
        /// </summary>
        /// <param name="number"></param>
        /// <param name="modulo"></param>
        /// <returns></returns>
        private BigInteger PositiveMod(BigInteger number, BigInteger modulo)
        {
            return (number % modulo + modulo) % modulo;
        }

        /// <summary>
        /// Inverse of modulo operation, only for prime numbers
        /// </summary>
        /// <param name="number"></param>
        /// <param name="prime"></param>
        /// <returns></returns>
        private BigInteger ModuloInverse(BigInteger number, BigInteger prime)
        {
            return BigInteger.ModPow(number, prime - 2, prime);
        }
    }
}