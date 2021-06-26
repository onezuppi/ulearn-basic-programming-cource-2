using System;
using System.Text;

namespace hashes
{
    public class GhostsTask :
        IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>,
        IMagic
    {
        private readonly byte[] content;
        private readonly Vector vector;
        private readonly Segment segment;
        private readonly Cat cat;
        private readonly Document document;
        private readonly Robot robot;

        public GhostsTask()
        {
            var encoding = Encoding.UTF8;
            content = encoding.GetBytes("abcdefghijklmnopqrstuvwxyz");
            vector = new Vector(0, 0);
            segment = new Segment(vector, vector);
            cat = new Cat("Pop", "Mixed", DateTime.Now);
            document = new Document("new", encoding, content);
            robot = new Robot("rd-d2");
        }

        Vector IFactory<Vector>.Create() => vector;

        Segment IFactory<Segment>.Create() => segment;

        Cat IFactory<Cat>.Create() => cat;

        Document IFactory<Document>.Create() => document;

        Robot IFactory<Robot>.Create() => robot;

        public void DoMagic()
        {
            content[0]++;
            vector.Add(new Vector(0, 1));
            segment.End.Add(new Vector(0, 2));
            cat.Rename("Pop-pop");
            Robot.BatteryCapacity += 1;
        }
    }
}