using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Days.Day08
{
    public class Day08 : IDay
    {
        private List<string> inputs = new List<string>();
        private List<int> ints;

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

            //inputs[0] = "123456789012";

            ints = inputs[0].ToCharArray().Select(c => int.Parse(c.ToString())).ToList();
        }

        /// <inheritdoc />
        public string Part1()
        {
            int imageSize = 25 * 6;
            //List<Layer> layers = new List<Layer>(ints.Count / imageSize);
            List<FlatLayer> layers = new List<FlatLayer>(ints.Count / imageSize);

            for (int i = 0; i < ints.Count / imageSize; i++)
            {
                //layers.Add(new Layer(25, ints.Skip(i * imageSize).Take(imageSize).ToList()));
                layers.Add(new FlatLayer( ints.Skip(i * imageSize).Take(imageSize).ToList()));
            }

            layers = layers.OrderBy(layer => layer.Pixels.Count(i => i == 0)).ToList();
            Console.WriteLine(layers[0].Pixels.Where(i => i == 0).Count());
            Console.WriteLine(layers[layers.Count - 1].Pixels.Where(i => i == 0).Count());

            int amountOfOnes = 0;
            int amountOfTwos = 0;
            foreach (int i in layers[0].Pixels)
            {
                if (i == 1)
                {
                    amountOfOnes++;
                } 
                else if (i == 2)
                {
                    amountOfTwos++;
                }
            }

            // less than 1785
            return $"{amountOfOnes * amountOfTwos}";
        }

        /// <inheritdoc />
        public string Part2()
        {
            int imageSize = 25 * 6;
            //List<Layer> layers = new List<Layer>(ints.Count / imageSize);
            List<FlatLayer> layers = new List<FlatLayer>(ints.Count / imageSize);
            for (int i = 0; i < ints.Count / imageSize; i++)
            {
                //layers.Add(new Layer(25, ints.Skip(i * imageSize).Take(imageSize).ToList()));
                layers.Add(new FlatLayer(ints.Skip(i * imageSize).Take(imageSize).ToList()));
            }

            FlatLayer output = new FlatLayer(new List<int>());
            for (int i = 0; i < imageSize; i++)
            {
                for (int j = 0; j < layers.Count; j++)
                {
                    if (layers[j].Pixels[i] < 2)
                    {
                        output.Pixels.Add(layers[j].Pixels[i]);
                        break;
                    }
                }

                // transparent all the way down
                if (output.Pixels.Count < i)
                {
                    output.Pixels.Add(2);
                }
            }

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    switch (output.Pixels[i * 25 + j])
                    {
                        case 0:
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(output.Pixels[i * 25 + j]);
                            Console.ResetColor();
                            break;

                        case 1:
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write(output.Pixels[i * 25 + j]);
                            Console.ResetColor();
                            break;
                    }
                }

                Console.Write("\n");
            }

            return $"";
        }

        private class FlatLayer
        {
            public List<int> Pixels;

            public FlatLayer(List<int> input)
            {
                Pixels = input;
            }
        }
    }
}