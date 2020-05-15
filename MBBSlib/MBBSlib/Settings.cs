using System;
using System.Xml.Linq;

namespace MBBSlib
{
    class Settings
    {
        static readonly string filePath = Environment.CurrentDirectory;

        static XElement settings;

        public static void Load()
        {
            var doc = XDocument.Load(filePath + "settings.xml");
            settings = doc.Root;
        }

        public static string GetSetting(string key) => settings.Element(key).Value;
    }
}
