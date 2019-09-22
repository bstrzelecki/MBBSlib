using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace MBBSlib.MonoGame
{
    public static class StaticResources
    {
        public static Dictionary<string, object> Resources { get; set; } = new Dictionary<string, object>();
        public static List<ISearchMethod> SearchLocations { get; set; } = new List<ISearchMethod>();

        public static object GetResource(string key)
        {
            if (Resources.ContainsKey(key))
            {
                return Resources[key];
            }
            foreach(ISearchMethod s in SearchLocations)
            {
                if (s.Get().ContainsKey(key))
                {
                    return s.Get()[key];
                }
            }
            return null;
        }
        internal static void SupplyResources(Dictionary<string, object> ts)
        {
            Resources = Merge(Resources, ts);
        }
        private static Dictionary<TKey, TValue> Merge<TKey, TValue>(Dictionary<TKey, TValue> a, Dictionary<TKey, TValue> b)
        {
            var result = new Dictionary<TKey, TValue>();
            foreach(var key in a.Keys)
            {
                if (result.ContainsKey(key)) continue;
                result.Add(key, a[key]);
            }
            foreach (var key in b.Keys)
            {
                if (result.ContainsKey(key)) continue;
                result.Add(key, b[key]);
            }
            return result;
        }
    }
}
