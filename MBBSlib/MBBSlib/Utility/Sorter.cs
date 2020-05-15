using System.Collections.Generic;

namespace MBBSlib.Utility
{
    class Sorter
    {
        readonly List<int> a = new List<int>();
        public Sorter()
        {

        }
    }
    public static class SortExtensions
    {
        public static IEnumerable<T> S<T>(this IEnumerable<T> tab) => tab;
    }
}
