using System;
using System.Collections;
using System.Collections.Generic;

namespace MBBSlib.Randomization
{
    public class RandomEnumarable : IEnumerable<int>
    {
        readonly Random r;
        readonly int i;
        readonly int min = 0;
        readonly int max = int.MaxValue;
        public RandomEnumarable(Random random, int amount)
        {
            r = random;
            i = amount;
        }
        public RandomEnumarable(Random random, int amount, int max)
        {
            r = random;
            i = amount;
            this.max = max;
        }
        public RandomEnumarable(Random random, int amount, int min, int max)
        {
            r = random;
            i = amount;
            this.min = min;
            this.max = max;
        }
        public IEnumerator<int> GetEnumerator()
        {
            return new RandomEnumerator(r, i, min, max);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new RandomEnumerator(r, i, min, max);
        }
    }
    public class RandomEnumerator : IEnumerator<int>
    {

        public int Current { get; private set; } = 0;

        object IEnumerator.Current => Current;
        Random rng;
        int i;
        readonly int min;
        readonly int max;
        public RandomEnumerator(Random random, int amount, int min, int max)
        {
            rng = random;
            i = amount;
            this.min = min;
            this.max = max;
        }
        public void Dispose()
        {
            rng = null;
        }

        public bool MoveNext()
        {
            if (i > 0)
            {
                Current = rng.Next(min, max);
                i--;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            rng = new Random();
        }
    }
}
