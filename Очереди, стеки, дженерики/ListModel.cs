using System.Collections.Generic;

namespace TodoApplication
{
    public abstract class CommandBase<TItem>
    {
        protected int index;
        protected TItem item;

        protected CommandBase(TItem item, int index)
        {
            this.item = item;
            this.index = index;
        }

        public abstract void Execute(List<TItem> items);

        public abstract void Undo(List<TItem> items);
    }

    public class AddCommand<TItem> : CommandBase<TItem>
    {
        public AddCommand(TItem item, int index) : base(item, index)
        {
        }

        public override void Execute(List<TItem> items)
        {
            items.Add(item);
        }

        public override void Undo(List<TItem> items)
        {
            items.RemoveAt(index);
        }
    }

    public class RemoveCommand<TItem> : CommandBase<TItem>
    {
        public RemoveCommand(TItem item, int index) : base(item, index)
        {
        }

        public override void Execute(List<TItem> items)
        {
            items.RemoveAt(index);
        }

        public override void Undo(List<TItem> items)
        {
            items.Insert(index, item);
        }
    }

    public class ListModel<TItem>
    {
        public List<TItem> Items {get; private set;}
        public int Limit {get; private set;}
        
        private readonly LimitedSizeStack<CommandBase<TItem>> history;

        public ListModel(int limit)
        {
            Items = new List<TItem>();
            Limit = limit;
            history = new LimitedSizeStack<CommandBase<TItem>>(limit);
        }

        public void AddItem(TItem item)
        {
            ExecuteCommand(new AddCommand<TItem>(item, Items.Count));
        }

        public void RemoveItem(int index)
        {
            ExecuteCommand(new RemoveCommand<TItem>(Items[index], index));
        }

        public bool CanUndo()
        {
            return history.Count > 0;
        }

        public void Undo()
        {
            if (CanUndo())
                history.Pop().Undo(Items);
        }

        private void ExecuteCommand(CommandBase<TItem> command)
        {
            command.Execute(Items);
            history.Push(command);
        }
    }
}