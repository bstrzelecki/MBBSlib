using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml.Linq;

namespace MBBSlib.Local
{
    /// <summary>
    /// Class for handling localisations
    /// </summary>
    public static class Localizations
    {
        static readonly Dictionary<string, string> _locals = new Dictionary<string, string>();

        static readonly string _systemLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        /// <summary>
        /// ISO 629-1 language code that will by loaded by <see cref="LoadDefault()"/> when user's system language is not supported
        /// </summary>
        public static string DefaultLanguage = "en";
        /// <summary>
        /// Loads default translation based on <see cref="CultureInfo.CurrentCulture"/> in local folder
        /// </summary>
        public static void LoadDefault() {
            try { 
                LoadTranslation($"{Environment.CurrentDirectory} + \\local\\ + {_systemLanguage} + .xml");
            }
            catch
            {
                Debug.WriteLine($"Language with ISO 639-1 : {_systemLanguage} was not found in {$"{Environment.CurrentDirectory} + \\local\\ + {_systemLanguage} + .xml"} folder");
                LoadTranslation($"{Environment.CurrentDirectory} + \\local\\ + {DefaultLanguage} + .xml");
            }
        }

        /// <summary>
        /// Loads and fills strings to RAM
        /// </summary>
        /// <param name="fileName">Direct path to lang.xml file</param>
        public static void LoadTranslation(string fileName)
        {
            if(!File.Exists(fileName)) throw new FileNotFoundException();
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
        /// Replaces key with coresponding string in dictionary, alias for <see cref="Translate(string, string)"/>
        /// </summary>
        /// <param name="t">Translation key</param>
        /// <param name="defaultString">String that will be returnded if key is not present in dictionary</param>
        /// <returns>Localised string</returns>
        public static string T(this string t, string defaultString) => Translate(t, defaultString);
        /// <summary>
        /// Replaces key with coresponding string in dictionary
        /// </summary>
        /// <seealso cref="T(string)"/>
        /// <param name="t">Translation key</param>
        /// <returns>Localised string</returns>
        public static string Translate(this string t) => t.Translate("(ULT):" + t);

        /// <summary>
        /// Replaces key with coresponding string in dictionary
        /// </summary>
        /// <param name="t">Translation key</param>
        /// <param name="defaultString">String that will be returnded if key is not present in dictionary</param>
        /// /// <seealso cref="T(string,string)"/>
        /// <returns>Localised string</returns>
        public static string Translate(this string t, string defaultString) => !_locals.ContainsKey(t) ? defaultString : _locals[t];
    }
}
