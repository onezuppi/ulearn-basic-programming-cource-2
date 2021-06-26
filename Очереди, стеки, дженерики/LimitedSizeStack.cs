using System.Collections.Generic;

namespace TodoApplication
{
    public class LimitedSizeStack<T>
    {
        private LinkedList<T> stack;
        private int limit;
        public LimitedSizeStack(int limit)
        {
            this.limit = limit;
            stack = new LinkedList<T>();
        }

        public void Push(T value)
        {
            stack.AddLast(value);
            if (stack.Count > limit)
                stack.RemoveFirst();
        }

        public T Pop()
        {
            var result = stack.Last;
            stack.RemoveLast();
            return result.Value;
        }

        public int Count => stack.Count;
    }
}
