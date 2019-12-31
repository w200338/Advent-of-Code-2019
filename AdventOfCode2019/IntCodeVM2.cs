using System;
using System.Collections.Generic;

namespace AdventOfCode2019
{
    public class IntCodeVM2
    {
        /// <summary>
        /// Memory of the VM
        /// </summary>
        private readonly Dictionary<long, long> memory = new Dictionary<long, long>();

        /// <summary>
        /// Currently loaded program
        /// </summary>
        private List<long> loadedProgram;

        /// <summary>
        /// List of all inputs
        /// </summary>
        private readonly List<long> inputs = new List<long>();

        /// <summary>
        /// Keeps track of position in inputs list to load next
        /// </summary>
        private int inputPointer;

        /// <summary>
        /// List of all outputs
        /// </summary>
        private readonly List<long> outputs = new List<long>();

        /// <summary>
        /// Reason VM halted
        /// </summary>
        public HaltCode LastHaltReason { get; private set; } = HaltCode.NotStarted;

        /// <summary>
        /// Address of opcode pointer when pausing
        /// </summary>
        private long pausedOpCodePointer;

        /// <summary>
        /// Base offset for param mode 2
        /// </summary>
        private long baseOffset;

        /// <summary>
        /// Create a VM and load a program into memory
        /// </summary>
        /// <param name="program"></param>
        public IntCodeVM2(List<long> program)
        {
            LoadProgram(program);
        }

        /// <summary>
        /// Get addresses for params
        /// </summary>
        /// <param name="opCodePointerPosition"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        private long[] GetParamAddresses(long opCodePointerPosition, int amount)
        {
            long[] output = new long[amount];
            long paramModes = GetMemory(opCodePointerPosition) / 100;

            for (int i = 0; i < amount; i++)
            {
                switch (paramModes % 10)
                {
                    // position
                    case 0:
                        output[i] = GetMemory(opCodePointerPosition + i + 1);
                        break;

                    // direct value
                    case 1:
                        output[i] = opCodePointerPosition + i + 1;
                        break;

                    // base offset location
                    case 2:
                        output[i] = baseOffset + GetMemory(opCodePointerPosition + i + 1);
                        break;

                    default:
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Red;

                        Console.WriteLine($"Fatal VM error: unknown parameter mode {paramModes % 10}");

                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }

                paramModes /= 10;
            }

            return output;
        }

        /// <summary>
        /// Resume paused program
        /// </summary>
        /// <returns>Reason for halting VM</returns>
        public HaltCode ResumeProgram()
        {
            return ExecuteProgram(pausedOpCodePointer);
        }

        /// <summary>
        /// Execute loaded program
        /// </summary>
        /// <returns>Reason for halting VM</returns>
        public HaltCode ExecuteProgram(long startAddress = 0)
        {
            // stop immediately if start address is negative
            if (startAddress < 0)
            {
                LastHaltReason = HaltCode.InvalidOpCodePointer;
                return HaltCode.InvalidOpCodePointer;
            }

            // pointer to address of current opcode
            long opcodePointer = startAddress;

            // parameters
            long[] longParams;

            // execute program
            while (true)
            {
                long opCode = GetMemory(opcodePointer);

                // instruction
                switch (opCode % 100)
                {
                    // addition
                    case 1:
                        longParams = GetParamAddresses(opcodePointer, 3);

                        SetMemory(longParams[2], GetMemory(longParams[0]) + GetMemory(longParams[1]));

                        opcodePointer += 4;
                        break;

                    // multiplication
                    case 2:
                        longParams = GetParamAddresses(opcodePointer, 3);

                        SetMemory(longParams[2], GetMemory(longParams[0]) * GetMemory(longParams[1]));

                        opcodePointer += 4;
                        break;

                    // get input
                    case 3:
                        longParams = GetParamAddresses(opcodePointer, 1);

                        long? input = GetNextInput();

                        // halt VM till input is given
                        if (input == null)
                        {
                            pausedOpCodePointer = opcodePointer;
                            LastHaltReason = HaltCode.WaitingForInput;
                            return HaltCode.WaitingForInput;
                        }
                        else
                        {
                            SetMemory(longParams[0], (long) input);
                        }

                        opcodePointer += 2;
                        break;

                    // output
                    case 4:
                        longParams = GetParamAddresses(opcodePointer, 1);

                        outputs.Add(GetMemory(longParams[0]));

                        opcodePointer += 2;
                        break;

                    // jump if not zero
                    case 5:
                        longParams = GetParamAddresses(opcodePointer, 2);
                        
                        if (GetMemory(longParams[0]) != 0L)
                        {
                            opcodePointer = GetMemory(longParams[1]);
                        }
                        else
                        {
                            opcodePointer += 3;
                        }
                        break;

                    // jump if zero
                    case 6:
                        longParams = GetParamAddresses(opcodePointer, 2);

                        if (GetMemory(longParams[0]) == 0L)
                        {
                            opcodePointer = GetMemory(longParams[1]);
                        }
                        else
                        {
                            opcodePointer += 3;
                        }
                        break;

                    // less than
                    case 7:
                        longParams = GetParamAddresses(opcodePointer, 3);

                        SetMemory(longParams[2], GetMemory(longParams[0]) < GetMemory(longParams[1]) ? 1 : 0);

                        opcodePointer += 4;
                        break;

                    // equals
                    case 8:
                        longParams = GetParamAddresses(opcodePointer, 3);

                        SetMemory(longParams[2], GetMemory(longParams[0]) == GetMemory(longParams[1]) ? 1 : 0);

                        opcodePointer += 4;
                        break;

                    // change base offset
                    case 9:
                        longParams = GetParamAddresses(opcodePointer, 1);

                        baseOffset += GetMemory(longParams[0]);

                        opcodePointer += 2;
                        break;

                    // end program
                    case 99:
                        LastHaltReason = HaltCode.Finished;
                        return HaltCode.Finished;

                    // unknown opcode
                    default:
                        // write error to console
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Red;

                        Console.WriteLine($"Fatal VM error: unknown opcode {opCode % 100} ({opCode}) at address {opcodePointer}");

                        Console.ForegroundColor = ConsoleColor.White;

                        LastHaltReason = HaltCode.UnknownOpCode;
                        return HaltCode.UnknownOpCode;
                }
            }
        }

        #region IO
        /// <summary>
        /// Get last output of VM
        /// </summary>
        /// <returns></returns>
        public long GetLastOutput()
        {
            return outputs[outputs.Count - 1];
        }

        /// <summary>
        /// Get all outputs of VM
        /// </summary>
        /// <returns></returns>
        public List<long> GetOutputs()
        {
            return outputs;
        }

        /// <summary>
        /// Clear output
        /// </summary>
        public void ClearOutput()
        {
            outputs.Clear();
        }

        /// <summary>
        /// Add an input for the VM
        /// </summary>
        /// <param name="input"></param>
        public void AddInput(long input)
        {
            inputs.Add(input);
        }

        /// <summary>
        /// Add an entire list of inputs
        /// </summary>
        /// <param name="input"></param>
        public void AddInput(List<long> input)
        {
            foreach (long l in input)
            {
                inputs.Add(l);
            }
        }

        /// <summary>
        /// Get next input
        /// </summary>
        /// <returns></returns>
        private long? GetNextInput()
        {
            // input hasn't been given yet
            if (inputPointer >= inputs.Count)
            {
                return null;
            }

            // return input and increment pointer with 1 afterwards
            return inputs[inputPointer++];
        }
        #endregion

        #region ProgramManagement
        /// <summary>
        /// Load a program into memory
        /// </summary>
        /// <param name="program">Program which gets loaded</param>
        /// <param name="deleteMemory">Delete current memory?</param>
        public void LoadProgram(List<long> program, bool deleteMemory = false)
        {
            // delete memory of VM if requested
            if (deleteMemory)
            {
                memory.Clear();
            }

            loadedProgram = program;

            // load each int into memory
            for (int i = 0; i < program.Count; i++)
            {
                SetMemory(i, program[i]);
            }
        }

        /// <summary>
        /// Reload program and purge memory
        /// </summary>
        public void Reset()
        {
            LoadProgram(loadedProgram, true);
        }
        #endregion

        #region MemoryManagement
        /// <summary>
        /// Get memory from an address (or 0 if not yet set)
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public long GetMemory(long address)
        {
            if (memory.TryGetValue(address, out long value))
            {
                return value;
            }

            memory.Add(address, 0);
            return 0;
        }

        /// <summary>
        /// Set a value in memory
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void SetMemory(long address, long value)
        {
            if (memory.ContainsKey(address))
            {
                memory[address] = value;
            }
            else
            {
                memory.Add(address, value);
            }
        }
        #endregion

        /// <summary>
        /// Reasons for halting the VM
        /// </summary>
        public enum HaltCode
        {
            NotStarted,
            Finished,
            UnknownOpCode,
            InvalidOpCodePointer,
            WaitingForInput
        }
    }
}
