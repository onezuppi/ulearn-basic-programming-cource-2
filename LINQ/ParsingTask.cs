using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public static class ParsingTask
    {
        public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
        {
            return lines
                .Skip(1)
                .Select(ParseSlideRecord)
                .Where(slideRecord => slideRecord != null)
                .ToDictionary(slideRecord => slideRecord.SlideId);
        }

        public static IEnumerable<VisitRecord> ParseVisitRecords(
            IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
        {
            return lines
                .Skip(1)
                .Select(visitRecord => ParseVisitRecord(visitRecord, slides));
        }

        private static SlideRecord ParseSlideRecord(string line)
        {
            var data = line.Split(';');
            if (data.Length == 3 && int.TryParse(data[0], out var id) &&
                Enum.TryParse(data[1], true, out SlideType type))
                return new SlideRecord(id, type, data[2]);
                
            return null;
        }

        private static VisitRecord ParseVisitRecord(string line, IDictionary<int, SlideRecord> slides)
        {
            try
            {
                var data = line.Split(';');
                var slideId = int.Parse(data[1]);

                return new VisitRecord(int.Parse(data[0]), slideId,
                    DateTime.Parse($"{data[2]} {data[3]}"), slides[slideId].SlideType);
            }
            catch
            {
                throw new FormatException($"Wrong line [{line}]");
            }
        }
    }
}