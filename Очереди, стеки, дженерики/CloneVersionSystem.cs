using System.Collections.Generic;

namespace Clones
{
    public class Node<T>
    {
        public Node(T value, Node<T> next)
        {
            Value = value;
            Next = next;
        }

        public T Value { get; }
        public Node<T> Next { get; }
    }

    public class Stack<T>
    {
        private Node<T> head;
        private int count;

        public int Count => count;

        public Stack()
        {
        }

        private Stack(Node<T> head, int count)
        {
            this.head = head;
            this.count = count;
        }

        public void Push(T item)
        {
            head = new Node<T>(item, head);
            count++;
        }

        public T Pop()
        {
            Node<T> tmp = head;
            head = head.Next;
            count--;
            return tmp.Value;
        }

        public T Peek()
        {
            return head.Value;
        }

        public Stack<T> Clone()
        {
            return  new Stack<T>(head, count);
        }
    }

    public class Clone
    {
        private Stack<string> learnedProgramms;
        private Stack<string> rollbackedProgramms;

        public Clone()
        {
            learnedProgramms = new Stack<string>();
            rollbackedProgramms = new Stack<string>();
        }

        private Clone(Stack<string> learnedProgramms, Stack<string> rollbackedProgramms)
        {
            this.learnedProgramms = learnedProgramms.Clone();
            this.rollbackedProgramms = rollbackedProgramms.Clone();
        }

        public void Learn(string program)
        {
            learnedProgramms.Push(program);
        }

        public void RollBack()
        {
            rollbackedProgramms.Push(learnedProgramms.Pop());
        }

        public void Relearn()
        {
            learnedProgramms.Push(rollbackedProgramms.Pop());
        }

        public string Check()
        {
            return learnedProgramms.Count > 0 ? learnedProgramms.Peek() : "basic";
        }

        public Clone Copy()
        {
            return new Clone(learnedProgramms, rollbackedProgramms);
        }
    }

    public class CloneVersionSystem : ICloneVersionSystem
    {
        private readonly List<Clone> clones = new List<Clone>();

        public string Execute(string query)
        {
            var parameters = query.Split(' ');
            var cloneNumber = int.Parse(parameters[1]) - 1;
            if (cloneNumber >= clones.Count)
                clones.Add(new Clone());

            if (parameters[0] == "learn")
                clones[cloneNumber].Learn(parameters[2]);
            else if (parameters[0] == "rollback")
                clones[cloneNumber].RollBack();
            else if (parameters[0] == "relearn")
                clones[cloneNumber].Relearn();
            else if (parameters[0] == "clone")
                clones.Add(clones[cloneNumber].Copy());
            else if (parameters[0] == "check")
                return clones[cloneNumber].Check();
            return null;
        }
    }
}