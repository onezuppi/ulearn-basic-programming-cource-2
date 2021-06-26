using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public static class StatisticsTask
    {
        public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
        {
            var times = visits
                .GroupBy(visit => visit.UserId)
                .SelectMany(group => group
                    .OrderBy(visitRecord => visitRecord.DateTime)
                    .Bigrams()
                    .Where(bigram => bigram.Item1.SlideType == slideType)
                    .Select(bigram => (bigram.Item2.DateTime - bigram.Item1.DateTime).TotalMinutes)
                    .Where(minutes => minutes >= 1 && minutes <= 120)
                ).ToArray();
                
            return times.Length == 0 ? 0 : times.Median();
        }
    }
}