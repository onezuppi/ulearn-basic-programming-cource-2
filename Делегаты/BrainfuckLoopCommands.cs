using System.Collections.Generic;

namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    {
        public static void RegisterTo(IVirtualMachine vm)
        {
            var blocks = GetBracketIndexes(vm.Instructions);
            vm.RegisterCommand('[', b =>
            {
                if (vm.Memory[vm.MemoryPointer] == 0)
                    vm.InstructionPointer = blocks[vm.InstructionPointer];
            });
            vm.RegisterCommand(']', b =>
            {
                if (vm.Memory[vm.MemoryPointer] != 0)
                    vm.InstructionPointer = blocks[vm.InstructionPointer];
            });
        }

        private static Dictionary<int, int> GetBracketIndexes(string commands)
        {
            var blocks = new Dictionary<int, int>();
            var stack = new Stack<int>();
            for (var i = 0; i < commands.Length; i++)
            {
                if (commands[i] == '[')
                    stack.Push(i);
                else if (commands[i] == ']')
                    (blocks[stack.Peek()], blocks[i]) = (i, stack.Pop());
            }

            return blocks;
        }
    }
}