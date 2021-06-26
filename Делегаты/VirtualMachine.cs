using System;
using System.Collections.Generic;

namespace func.brainfuck
{
    public class VirtualMachine : IVirtualMachine
    {
        private readonly Dictionary<char, Action<IVirtualMachine>> commands =
            new Dictionary<char, Action<IVirtualMachine>>();

        public VirtualMachine(string program, int memorySize)
        {
            Instructions = program;
            Memory = new byte[memorySize];
        }

        public string Instructions { get; }
        public int InstructionPointer { get; set; }
        public byte[] Memory { get; }
        public int MemoryPointer { get; set; }

        public void RegisterCommand(char symbol, Action<IVirtualMachine> execute) => commands[symbol] = execute;

        public void Run()
        {
            for (; InstructionPointer < Instructions.Length; InstructionPointer++)
                if (commands.ContainsKey(Instructions[InstructionPointer]))
                    commands[Instructions[InstructionPointer]](this);
        }
    }
}