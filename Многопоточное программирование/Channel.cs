using System.Collections.Generic;
using System.Linq;

namespace rocket_bot
{
    public class Channel<T> where T : class
    {
        private readonly List<T> list = new List<T>();

        public T this[int index]
        {
            get
            {
                lock (list)
                {
                    return list.Count > index && index >= 0 ? list[index] : null;
                }
            }
            set
            {
                lock (list)
                {
                    if (index == list.Count)
                        list.Add(value);
                    else if (index < list.Count && index >= 0)
                    {
                        list[index] = value;
                        list.RemoveRange(index + 1, list.Count - index - 1);
                    }
                }
            }
        }

        public T LastItem()
        {
            lock (list)
            {
                return list.Count == 0 ? null : list.Last();
            }
        }

        public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
        {
            lock (list)
            {
                if (list.Count == 0 || list.Last() == knownLastItem)
                    list.Add(item);
            }
        }

        public int Count
        {
            get
            {
                lock (list)
                {
                    return list.Count;
                }
            }
        }
    }
}