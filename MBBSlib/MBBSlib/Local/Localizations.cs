using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MBBSlib.Local
{
    public static class Localizations
    {
        static Dictionary<string, string> locals = new Dictionary<string, string>();
        /// <summary>
        /// Loads and fills strings to RAM
        /// </summary>
        /// <param name="fileName">Direct path to lang.xml file</param>
        public static void LoadTranslation(string fileName)
        {
            locals.Clear();
            XDocument doc = XDocument.Load(fileName);
            XElement root = doc.Root;

            foreach(var n in root.Elements("t"))
            {
                locals.Add(n.Element("key").Value, n.Element("string").Value);
            }
        }
        /// <summary>
        /// alias for Translate()
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string T(this string t)
        {
            if (!locals.ContainsKey(t))
            {
                return t;
            }
            return locals[t];
        }
        /// <summary>
        /// Replaces key with coresponding string in dictionary
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
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
