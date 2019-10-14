using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MBBSlib.Local
{
    public static class Localizations
    {
        static Dictionary<string, string> locals = new Dictionary<string, string>();

        public static void LoadTranslation(string fileName)
        {
            XDocument doc = XDocument.Load(fileName);
            XElement root = doc.Root;

            foreach(var n in root.Elements("t"))
            {
                locals.Add(n.Element("key").Value, n.Element("string").Value);
            }
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
