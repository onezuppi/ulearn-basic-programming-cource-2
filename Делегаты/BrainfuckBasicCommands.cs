using System;
using System.Collections.Generic;
using System.Linq;

namespace func.brainfuck
{
    public class BrainfuckBasicCommands
    {
        public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            vm.RegisterCommand('.', b => { write((char) vm.Memory[vm.MemoryPointer]); });
            vm.RegisterCommand('+',
                b => { vm.Memory[vm.MemoryPointer] = (byte) ((vm.Memory[vm.MemoryPointer] + 1) % 256); });
            vm.RegisterCommand('-',
                b =>
                {
                    vm.Memory[vm.MemoryPointer] =
                        (byte) (vm.Memory[vm.MemoryPointer] - 1 < 0 ? 255 : vm.Memory[vm.MemoryPointer] - 1);
                });
            vm.RegisterCommand(',', b => { vm.Memory[vm.MemoryPointer] = (byte) (read() % 256); });
            vm.RegisterCommand('>', b => { vm.MemoryPointer = (vm.MemoryPointer + 1) % vm.Memory.Length; });
            vm.RegisterCommand('<',
                b => { vm.MemoryPointer = vm.MemoryPointer - 1 < 0 ? vm.Memory.Length - 1 : vm.MemoryPointer - 1; });
            foreach (var (start, stop) in new[] {('A', 'Z'), ('a', 'z'), ('0', '9')})
                for (var symbol = start; symbol <= stop; symbol++)
                {
                    var symbol1 = symbol;
                    vm.RegisterCommand(symbol, b => { vm.Memory[vm.MemoryPointer] = (byte) symbol1; });
                }
        }
    }
}