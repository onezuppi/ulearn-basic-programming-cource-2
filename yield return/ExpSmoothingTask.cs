using System.Collections.Generic;

namespace yield
{
    public static class ExpSmoothingTask
    {
        public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
        {
            var smooth = 0d;
            var isFirst = true;
            
            foreach (var point in data)
            {
				if (isFirst)
                	(smooth, isFirst) = (point.OriginalY, false);
				else
					smooth = alpha * point.OriginalY + (1 - alpha) * smooth;

                yield return point.WithExpSmoothedY(smooth);
            }
        }
    }
}