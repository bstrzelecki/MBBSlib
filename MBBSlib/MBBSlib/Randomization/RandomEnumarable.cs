using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.Randomization
{
    public class RandomEnumarable : IEnumerable<int>
    {
        Random r;
        int i;
        public RandomEnumarable(Random random, int amount)
        {
            r = random;
            i = amount;
        }
        public IEnumerator<int> GetEnumerator()
        {
            return new RandomEnumerator(r, i);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new RandomEnumerator(r, i);
        }
    }
    public class RandomEnumerator : IEnumerator<int>
    {
        public int Current { get; private set; } = 0;

        object IEnumerator.Current => Current;
        Random rng;
        int i;

        public RandomEnumerator(Random random, int amount)
        {
            rng = random;
            i = amount;
        }

        public void Dispose()
        {
            rng = null;
        }

        public bool MoveNext()
        {
            if(i > 0)
            {
                Current = rng.Next();
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
