using System;
using System.Collections.Generic;

namespace AdventOfCode2019.Days
{
    /// <summary>
    /// 2019 computer
    /// </summary>
    public static class IntCodeComputer
    {
        /// <summary>
        /// Execute computer program on a given program
        /// </summary>
        /// <param name="memory"></param>
        /// <param name="startOpCodeAddress"></param>
        public static void RunProgram(List<int> memory, int startOpCodeAddress = 0)
        {
            // size of memory for quicker calculations
            int memorySize = memory.Count;

            // pointer to current instruction
            int opCodeAddress = startOpCodeAddress;

            while (true)
            {
                // get op code
                int opCode = memory[opCodeAddress];

                int a, b, c;
                switch (opCode)
                {
                    // +
                    case 1:
                        // add values in address a and b and put it into address c
                        a = memory[opCodeAddress + 1];
                        b = memory[opCodeAddress + 2];
                        c = memory[opCodeAddress + 3];

                        if (a >= memorySize || b >= memorySize || c >= memorySize)
                        {
                            return;
                        }

                        memory[c] = memory[a] + memory[b];
                        opCodeAddress += 4; // move pointer to next instruction address
                        break;

                    // *
                    case 2:
                        // multiply values in address a and b and put it into address c
                        a = memory[opCodeAddress + 1];
                        b = memory[opCodeAddress + 2];
                        c = memory[opCodeAddress + 3];

                        if (a >= memorySize || b >= memorySize || c >= memorySize)
                        {
                            return;
                        }

                        memory[c] = memory[a] * memory[b];
                        opCodeAddress += 4; // move pointer to next instruction address
                        break;

                    // input
                    case 3:
                        a = memory[opCodeAddress + 1];
                        
                        if (a >= memorySize)
                        {
                            return;
                        }

                        // TODO: give input somehow

                        memory[a] = a;

                        opCodeAddress += 2;
                        break;

                    // output
                    case 4:
                        a = memory[opCodeAddress + 1];

                        if (a >= memorySize)
                        {
                            return;
                        }

                        Console.WriteLine(memory[a]);

                        opCodeAddress += 2;
                        break;

                    // stop program normally
                    case 99:
                        return;

                    // stop at any other opCode (error)
                    default:
                        return;

                    
                }
            }
        }
    }
}
