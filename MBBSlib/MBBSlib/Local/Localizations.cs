using System.Collections.Generic;
using System.Xml.Linq;

namespace MBBSlib.Local
{
    public static class Localizations
    {
        static readonly Dictionary<string, string> locals = new Dictionary<string, string>();
        /// <summary>
        /// Loads and fills strings to RAM
        /// </summary>
        /// <param name="fileName">Direct path to lang.xml file</param>
        public static void LoadTranslation(string fileName)
        {
            locals.Clear();
            var doc = XDocument.Load(fileName);
            XElement root = doc.Root;

            foreach(var n in root.Elements("t"))
            {
                locals.Add(n.Element("key").Value, n.Element("string").Value);
            }
        }
        /// <summary>
        /// Generates template document for quick copy/paste
        /// </summary>
        /// <returns>Save output with .Save(s) or access via IntelliSense</returns>
        public static XDocument GetTemplate()
        {
            var doc = new XDocument();
            doc.Add(new XElement("root"));
            XElement root = doc.Root;
            root.Add(new XElement("t", new XElement("key"), new XElement("string")));
            return doc;
        }
        /// <summary>
        /// Alias for Translate()
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string T(this string t)
        {
            if(!locals.ContainsKey(t))
            {
                return "(ULT):" + t;
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
            if(!locals.ContainsKey(t))
            {
                return "(ULT):" + t;
            }
            return locals[t];
        }
    }
}
