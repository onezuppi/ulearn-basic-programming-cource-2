using System.Collections.Generic;

namespace yield
{
    public static class MovingMaxTask
    {
        public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
        {
            var maxes = new LinkedList<(int Index, double Value)>();
            var index = 0;
            
            foreach (var point in data)
            {
                while (maxes.Last != null && point.OriginalY > maxes.Last.Value.Value)
                    maxes.RemoveLast();
                maxes.AddLast((index, point.OriginalY));
                if (index - windowWidth >= maxes.First.Value.Index)
                    maxes.RemoveFirst();
                index++;

                yield return point.WithMaxY(maxes.First.Value.Value);
            }
        }
    }
}