using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Days
{
    public class IntCodeVM
    {
        /// <summary>
        /// Currently loaded program
        /// </summary>
        private List<long> loadedProgram;

        /// <summary>
        /// Computer memory
        /// </summary>
        private long[] memory;

        /// <summary>
        /// List of inputs to be used by the program
        /// </summary>
        private List<long> inputs = new List<long>();

        /// <summary>
        /// Input which is currently getting read
        /// </summary>
        private int currentInput = 0;

        /// <summary>
        /// Outputs
        /// </summary>
        private List<long> outputs = new List<long>();

        /// <summary>
        /// keep track of pointer position if program is paused
        /// </summary>
        private int pointerPosition = 0;

        /// <summary>
        /// Relative base for relative paramaters
        /// </summary>
        private int relativeBase = 0;

        /// <summary>
        /// Reason the VM stopped last time
        /// </summary>
        public HaltCode LastHaltCode { get; private set; } = HaltCode.NotStarted;

        /// <summary>
        /// Total amount of instructions executed
        /// </summary>
        private ulong instructionsExecuted = 0;

        /// <summary>
        /// Create a new computer with a program
        /// </summary>
        /// <param name="program">list of integer instructions</param>
        /// <param name="memorySize"></param>
        public IntCodeVM(List<long> program, int memorySize = -1)
        {
            // automatically set memory size to program size or to parameter
            memory = new long[memorySize == -1 ? program.Count : memorySize];

            // load program into memory
            LoadProgram(program);
        }

        public IntCodeVM(List<int> program, int memorySize = -1)
        {
            // automatically set memory size to program size or to parameter
            memory = new long[memorySize == -1 ? program.Count : memorySize];

            // load program into memory
            LoadProgram(program.Select(i => (long)i).ToList());
        }

        /// <summary>
        /// Create a new computer with a program
        /// </summary>
        /// <param name="program">comma separated string of integers</param>
        /// <param name="memorySize"></param>
        public IntCodeVM(string program, int memorySize = -1)
        {
            // automatically set memory size to program size or to parameter
            memory = new long[memorySize == -1 ? program.Length : memorySize];

            // load program into memory
            LoadProgram(program.Split(',').Select(s => Convert.ToInt64(s)).ToList());
        }

        /// <summary>
        /// Add input for program
        /// </summary>
        /// <param name="input"></param>
        public void AddInput(long input)
        {
            inputs.Add(input);
        }

        /// <summary>
        /// Return an input
        /// </summary>
        /// <returns></returns>
        private long? GetInput()
        {
            if (currentInput >= inputs.Count)
            {
                return null;
            }

            return inputs[currentInput++];
        }

        public List<long> GetOutputs()
        {
            return outputs;
        }

        public long GetLastOutput()
        {
            return outputs[outputs.Count - 1];
        }

        /// <summary>
        /// Get array of parameters for an instruction
        /// </summary>
        /// <param name="pointerPosition"></param>
        /// <param name="amount"></param>
        /// <param name="lastIsPointer"></param>
        /// <returns></returns>
        public long[] GetParameters(int pointerPosition, int amount)
        {
            long[] output = new long[amount];

            // get operation parameter modes
            long opcode = memory[pointerPosition] / 100;

            for (int i = 0; i < amount; i++)
            {
                // position -> get from position in memory
                if (opcode % 10 == 0)
                {
                    output[i] = memory[memory[pointerPosition + i + 1]];
                }
                // value -> return straight from memory
                else if (opcode % 10 == 1)
                {
                    output[i] = memory[pointerPosition + i + 1];
                }
                // relative mode
                else if (opcode % 10 == 2)
                {
                    output[i] = memory[relativeBase + memory[pointerPosition + i + 1]];
                }

                opcode /= 10;
            }

            return output;
        }

        /// <summary>
        /// Resume a paused program
        /// </summary>
        /// <returns>if the program finished</returns>
        public HaltCode ResumeProgram()
        {
            return ExecuteProgram(pointerPosition);
        }

        /// <summary>
        /// Execute a program
        /// </summary>
        /// <returns>Boolean based on how program stopped, false if it can be continued (opcode 99) or true if its waiting on input</returns>
        public HaltCode ExecuteProgram(int startOpcodeAddress = 0)
        {
            // stop immediately if start address is out of memory
            if (startOpcodeAddress < 0 || startOpcodeAddress >= memory.Length)
            {
                LastHaltCode = HaltCode.InvalidPointer;
                return HaltCode.InvalidPointer;
            }

            // pointer to address of current opcode
            int opcodePointer = startOpcodeAddress;

            // possible parameters
            long a, b;

            while (true)
            {
                // get op code
                long opCode = memory[opcodePointer];

                long[] param;
                switch (opCode % 100)
                {
                    // +
                    case 1:
                        param = GetParameters(opcodePointer, 2);
                        a = param[0];
                        b = param[1];

                        if ((memory[opcodePointer] / 100_0_0) % 10 == 2)
                        {
                            SetMemory(memory[opcodePointer + 3] + relativeBase, a + b);
                        }
                        else if ((memory[opcodePointer] / 100_0_0) % 10 == 0)
                        {
                            SetMemory(memory[opcodePointer + 3], a + b);
                        }
                        else
                        {
                            Console.WriteLine($"Opcode {opCode} wasn't handled");
                        }
                        
                        //SetMemory(c, a + b);
                        opcodePointer += 4; // move pointer to next instruction address
                        break;

                    // *
                    case 2:
                        param = GetParameters(opcodePointer, 2);
                        a = param[0];
                        b = param[1];
                        //c = param[2];

                        
                        if ((memory[opcodePointer] / 100_0_0) % 100 == 2)
                        {
                            SetMemory(memory[opcodePointer + 3] + relativeBase, a * b);
                        }
                        else if ((memory[opcodePointer] / 100_0_0) % 10 == 0)
                        {
                            SetMemory(memory[opcodePointer + 3], a * b);
                        }
                        else
                        {
                            Console.WriteLine($"Opcode {opCode} wasn't handled");
                        }
                        

                        //SetMemory(c, a * b);
                        opcodePointer += 4; // move pointer to next instruction address
                        break;

                    // input
                    case 3:
                        //SetMemory(memory[opcodePointer + 1], GetInput());

                        long? input = GetInput();

                        // pause and wait for input
                        if (input == null)
                        {
                            pointerPosition = opcodePointer;
                            LastHaltCode = HaltCode.WaitingForInput;
                            return HaltCode.WaitingForInput;
                        }
                        // use input and continue running
                        else
                        {
                            switch (memory[opcodePointer] / 100 % 10)
                            {
                                case 0:
                                    SetMemory(memory[memory[opcodePointer + 1]], (long)input);
                                    break;

                                case 1:
                                    SetMemory(memory[opcodePointer + 1], (long)input);
                                    break;

                                case 2:
                                    SetMemory(memory[opcodePointer + 1] + relativeBase, (long)input);
                                    break;
                            }

                            /*
                            param = GetParameters(opcodePointer, 1);
                            a = param[0];
                            SetMemory(a, (long)input);
                            */
                        }

                        opcodePointer += 2;
                        break; // move pointer to next instruction address

                    // output
                    case 4:
                        //Console.WriteLine(GetMemory(memory[opcodePointer + 1]));
                        param = GetParameters(opcodePointer, 1);
                        a = param[0];

                        outputs.Add(a);

                        opcodePointer += 2;
                        break; // move pointer to next instruction address

                    // jump if true
                    case 5:
                        param = GetParameters(opcodePointer, 2);
                        a = param[0];
                        b = param[1];

                        if (a != 0)
                        {
                            opcodePointer = (int)b;
                        }
                        else
                        {
                            opcodePointer += 3;
                        }
                        break;

                    // jump if false
                    case 6:
                        param = GetParameters(opcodePointer, 2);
                        a = param[0];
                        b = param[1];

                        if (a == 0)
                        {
                            opcodePointer = (int)b;
                        }
                        else
                        {
                            opcodePointer += 3;
                        }
                        break;

                    // less than
                    case 7:
                        param = GetParameters(opcodePointer, 2);
                        a = param[0];
                        b = param[1];

                        if ((memory[opcodePointer] / 100_0_0) % 10 == 2)
                        {
                            SetMemory(memory[opcodePointer + 3] + relativeBase, a < b ? 1 : 0);
                        }
                        else if ((memory[opcodePointer] / 100_0_0) % 10 == 0)
                        {
                            SetMemory(memory[opcodePointer + 3], a < b ? 1 : 0);
                        }
                        else
                        {
                            Console.WriteLine($"Opcode {opCode} wasn't handled");
                        }

                        opcodePointer += 4;
                        break;

                    // equals
                    case 8:
                        param = GetParameters(opcodePointer, 2);
                        a = param[0];
                        b = param[1];

                        if ((memory[opcodePointer] / 100_0_0) % 10 == 2)
                        {
                            SetMemory(memory[opcodePointer + 3] + relativeBase, a == b ? 1 : 0);
                        }
                        else if ((memory[opcodePointer] / 100_0_0) % 10 == 0)
                        {
                            SetMemory(memory[opcodePointer + 3], a == b ? 1 : 0);
                        }
                        else
                        {
                            Console.WriteLine($"Opcode {opCode} wasn't handled");
                        }

                        opcodePointer += 4;
                        break;

                    // relative base offset
                    case 9:
                        param = GetParameters(opcodePointer, 1);
                        a = param[0];

                        relativeBase += (int)a;

                        opcodePointer += 2;
                        break;

                    // stop program normally
                    case 99:
                        LastHaltCode = HaltCode.Finished;
                        return HaltCode.Finished;

                    // stop at any other opCode (error)
                    default:
                        Console.WriteLine($"Critical VM error: Unknown opcode {opCode} after {instructionsExecuted} instructions");
                        LastHaltCode = HaltCode.UnknownOpCode;
                        return HaltCode.UnknownOpCode;
                }

                instructionsExecuted++;
            }
        }

        /// <summary>
        /// Try to load a new program into memory
        /// </summary>
        /// <param name="newProgram">New program integers</param>
        /// <param name="startOffset">Load into an offset of memory</param>
        /// <param name="clearBefore">Clear integers before program</param>
        /// <param name="clearAfter">Clear integers after program</param>
        /// <param name="clearValue">Value to clear memory with</param>
        /// <returns></returns>
        public bool LoadProgram(List<long> newProgram, int startOffset = 0, bool clearBefore = false, bool clearAfter = false, int clearValue = 0)
        {
            // check size and offset
            if (newProgram.Count + startOffset > memory.Length)
            {
                return false;
            }

            // write into memory
            for (int i = 0; i < newProgram.Count; i++)
            {
                memory[i + startOffset] = newProgram[i];
            }

            // clear other memory regions
            if (clearBefore)
            {
                for (int i = 0; i < startOffset; i++)
                {
                    memory[i] = clearValue;
                }
            }

            if (clearAfter)
            {
                for (int i = startOffset + newProgram.Count - 1; i < memory.Length; i++)
                {
                    memory[i] = clearValue;
                }
            }

            // keep a reference to the program
            loadedProgram = newProgram;

            return true;
        }

        /// <summary>
        /// Reload program into memory
        /// </summary>
        public void Reset()
        {
            LoadProgram(loadedProgram);
        }

        /// <summary>
        /// Override current memory
        /// </summary>
        /// <param name="newMemory"></param>
        public void OverrideMemory(List<long> newMemory)
        {
            memory = newMemory.ToArray();
        }

        /// <summary>
        /// Resize memory of VM
        /// </summary>
        /// <param name="newSize">New size of VM memory</param>
        /// <param name="startOffset">Offset to copy from original memory into new memory</param>
        /// <returns>If operation succeeded</returns>
        public bool ResizeMemory(long newSize, long startOffset = 0)
        {
            if (newSize > 0)
            {
                long[] newMemory = new long[newSize];

                if (startOffset >= 0 && startOffset < memory.Length)
                {
                    // bigger size
                    if (newSize > memory.Length - startOffset)
                    {
                        Array.Copy(memory, startOffset, newMemory, 0, memory.Length - startOffset);
                    }
                    // smaller size
                    else
                    {
                        Array.Copy(memory, startOffset, newMemory, 0, newMemory.Length);
                    }
                }

                // overwrite memory
                memory = newMemory;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get memory from a certain address
        /// </summary>
        /// <param name="address">Address to get</param>
        /// <returns></returns>
        public long GetMemory(long address)
        {
            if (address > 0 && address < memory.Length)
            {
                return memory[address];
            }

            return 0;
        }

        /// <summary>
        /// Set memory address to a given value
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetMemory(long address, long value)
        {
            if (address >= 0 && address < memory.Length)
            {
                memory[address] = value;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Clear output list
        /// </summary>
        public void ClearOutput()
        {
            outputs.Clear();
        }

        /// <summary>
        /// Reason why the VM stopped executing the program
        /// </summary>
        public enum HaltCode
        {
            Finished,
            WaitingForInput,
            UnknownOpCode,
            InvalidPointer,
            NotStarted
        }
    }
}
