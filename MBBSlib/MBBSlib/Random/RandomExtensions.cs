using System;
using System.Collections.Generic;

namespace MBBSlib.Random
{
    public static class RandomExtensions
    {
        public static IEnumerable<int> Take(this System.Random random, int i)
        {
            return new RandomEnumarable(random, i);
        }
        public static IEnumerable<int> Take(this System.Random random, int i, int max)
        {
            return new RandomEnumarable(random, i, max);
        }
        public static IEnumerable<int> Take(this System.Random random, int i, int min, int max)
        {
            return new RandomEnumarable(random, i, min, max);

        }
    }
}
