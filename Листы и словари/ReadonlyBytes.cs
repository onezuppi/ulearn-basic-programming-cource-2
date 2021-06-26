using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable<byte>
    {
        private const int PrimeNumber = 16777619;
        private readonly byte[] values;
        private (int Value, bool IsCalсulated) hash;

        public ReadonlyBytes(params byte[] bytes)
        {
            values = bytes ?? throw new ArgumentNullException();
            hash = (0, false);
        }

        public int Length => values.Length;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<byte> GetEnumerator() => ((IEnumerable<byte>) values).GetEnumerator();

        public override string ToString() => $"[{string.Join(", ", values)}]";

        public override bool Equals(object obj)
        {
            var point = obj as ReadonlyBytes;
            if (point == null || point.Length != values.Length || obj.GetType() != GetType())
                return false;
                
            return point == this || !point.Where((item, index) => values[index] != item).Any();
        }

        public override int GetHashCode()
        {
            if (hash.IsCalсulated)
                return hash.Value;
            unchecked
            {
                hash.Value = values.Aggregate(hash.Value, (r, o) => (r ^ o.GetHashCode()) * PrimeNumber);
            }
            hash.IsCalсulated = true;

            return hash.Value;
        }

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index >= values.Length)
                    throw new IndexOutOfRangeException();

                return values[index];
            }
        }
    }
}