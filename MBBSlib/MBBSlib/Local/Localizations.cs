using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.Local
{
    public static class Localizations
    {
        static Dictionary<string, string> locals = new Dictionary<string, string>();

        public static void LoadTranslation(string fileName)
        {
            //TODO
        }
        public static string T(this string t)
        {
            if (!locals.ContainsKey(t))
            {
                return t;
            }
            return locals[t];
        }
        public static string Translate(this string t)
        {
            if (!locals.ContainsKey(t))
            {
                return t;
            }
            return locals[t];
        }
    }
}
