using System;
using System.Collections.Generic;

namespace MBBSlib.Randomization
{
    public static class Extensions
    {
        public static IEnumerable<int> Take(this Random random, int i)
        {
            return new RandomEnumarable(random, i);
        }
    }
}
