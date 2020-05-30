using System.Collections.Generic;
using System.Xml.Linq;

namespace MBBSlib.Local
{
    /// <summary>
    /// Class for handling localisations
    /// </summary>
    public static class Localizations
    {
        static readonly Dictionary<string, string> _locals = new Dictionary<string, string>();
        /// <summary>
        /// Loads and fills strings to RAM
        /// </summary>
        /// <param name="fileName">Direct path to lang.xml file</param>
        public static void LoadTranslation(string fileName)
        {
            _locals.Clear();
            var doc = XDocument.Load(fileName);
            XElement root = doc.Root;

            foreach(var n in root.Elements("t"))
            {
                _locals.Add(n.Attribute("key").Value, n.Attribute("string").Value);
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
            var t = new XElement("t");
            t.Attribute("key").Value = "";
            t.Attribute("string").Value = "";
            root.Add(t);
            return doc;
        }
        /// <summary>
        /// Replaces key with coresponding string in dictionary, alias for <see cref="Translate(string)"/>
        /// </summary>
        /// <param name="t">Translation key</param>
        /// <returns>Localised string</returns>
        public static string T(this string t) => Translate(t);
        /// <summary>
        /// Replaces key with coresponding string in dictionary
        /// </summary>
        /// <seealso cref="T(string)"/>
        /// <param name="t">Translation key</param>
        /// <returns>Localised string</returns>
        public static string Translate(this string t) => !_locals.ContainsKey(t) ? "(ULT):" + t : _locals[t];
    }
}
