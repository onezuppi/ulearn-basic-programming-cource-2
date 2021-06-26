using System.Collections.Generic;

namespace yield
{
	public static class MovingAverageTask
	{
		public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
		{
			var queue = new Queue<double>();
			var sum = 0.0;
			
			foreach (var point in data)
			{
				queue.Enqueue(point.OriginalY);
				sum += point.OriginalY;
				if (queue.Count > windowWidth)
					sum -= queue.Dequeue();
					
				yield return point.WithAvgSmoothedY(sum / queue.Count);
			}
		}
	}
}