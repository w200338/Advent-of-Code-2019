using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.Days.Day23
{
    public class Day23 : IDay
    {
        private List<string> inputs = new List<string>();
        private List<long> program = new List<long>();

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

        /// <inheritdoc />
        public string Part1()
        {
            // create vms and inputs
            List<IntCodeVM2> vms = new List<IntCodeVM2>();
            for (int i = 0; i < 50; i++)
            {
                vms.Add(new IntCodeVM2(program));
                vms[i].AddInput(i);
            }

            while (true)
            {
                // run all of them till they stop waiting for input
                List<Task> tasks = new List<Task>();
                foreach (IntCodeVM2 vm in vms)
                {
                    tasks.Add(Task.Run(() => { vm.ResumeProgram(); }));

                }
                Task.WaitAll(tasks.ToArray());

                // add input to all of them
                bool[] addedInput = new bool[50];
                for (int i = 0; i < vms.Count; i++)
                {
                    IntCodeVM2 vm = vms[i];
                    List<long> packet = vm.GetOutputs();

                    if (packet.Count > 0)
                    {
                        // go through multiple packets
                        for (int j = 0; j < packet.Count; j += 3)
                        {
                            // actually crashes the program
                            if (packet[j] == 255)
                            {
                                return $"Y value of first packet which was went to 255: {packet[2]}";
                            }

                            IntCodeVM2 receiver = vms[(int)packet[j]];
                            receiver.AddInput(packet[j + 1]);
                            receiver.AddInput(packet[j + 2]); // crash showed 22074

                            addedInput[i] = true;
                        }

                        // clear output for next time
                        vm.ClearOutput();
                    }
                }

                // all of those which didn't receive input get -1
                for (int i = 0; i < addedInput.Length; i++)
                {
                    if (!addedInput[i])
                    {
                        vms[i].AddInput(-1);
                    }
                }
            }
        }

        /// <inheritdoc />
        public string Part2()
        {
            // create vms and inputs
            List<IntCodeVM2> vms = new List<IntCodeVM2>();
            for (int i = 0; i < 50; i++)
            {
                vms.Add(new IntCodeVM2(program));
                vms[i].AddInput(i);
            }

            List<long> lastNatPacket = new List<long>()
            {
                0, 0, 0
            };
            List<long> previousYValuesUsed = new List<long>();
            bool updatedY = false;

            while (true)
            {
                // run all of them till they stop waiting for input
                List<Task> tasks = new List<Task>();
                foreach (IntCodeVM2 vm in vms)
                {
                    tasks.Add(Task.Run(() => { vm.ResumeProgram(); }));

                }
                Task.WaitAll(tasks.ToArray());

                // add input to all of them
                bool[] addedInput = new bool[50];
                for (int i = 0; i < vms.Count; i++)
                {
                    IntCodeVM2 vm = vms[i];
                    List<long> packet = vm.GetOutputs();

                    if (packet.Count > 0)
                    {
                        // go through multiple packets
                        for (int j = 0; j < packet.Count; j += 3)
                        {
                            // actually crashes the program
                            if (packet[j] == 255)
                            {
                                //return $"Y value of first packet which was went to 255: {packet[2]}";
                                lastNatPacket[1] = packet[j + 1];
                                lastNatPacket[2] = packet[j + 2];

                                updatedY = true;
                            }
                            else
                            {
                                IntCodeVM2 receiver = vms[(int)packet[j]];
                                receiver.AddInput(packet[j + 1]);
                                receiver.AddInput(packet[j + 2]); // crash showed 22074

                                addedInput[i] = true;
                            }
                        }

                        // clear output for next time
                        vm.ClearOutput();
                    }
                }

                // check if all inputs are empty
                if (addedInput.All(i => i == false))
                {
                    // send last NAT packet to vm 0
                    vms[0].AddInput(lastNatPacket);

                    // only add used Y after updating Y
                    if (updatedY)
                    {
                        previousYValuesUsed.Add(lastNatPacket[2]);
                        updatedY = false;
                    }

                    // check if this one was sent before
                    if (previousYValuesUsed.Count > 1 && previousYValuesUsed[previousYValuesUsed.Count - 2] == previousYValuesUsed[previousYValuesUsed.Count - 1])
                    {
                        return $"sent {previousYValuesUsed[previousYValuesUsed.Count - 2]} twice in a row";
                    }

                    // send -1 to the rest
                    for (int i = 1; i < addedInput.Length; i++)
                    {
                        if (!addedInput[i])
                        {
                            vms[i].AddInput(-1);
                        }
                    }
                }
                // just another round
                else
                {
                    // all of those which didn't receive input get -1
                    for (int i = 0; i < addedInput.Length; i++)
                    {
                        if (!addedInput[i])
                        {
                            vms[i].AddInput(-1);
                        }
                    }
                }
            }
        }
    }
}