using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MBBSlib
{
    class Settings
    {
        static string filePath = Environment.CurrentDirectory;

        static XElement settings;

        public static void Load()
        {
            XDocument doc = XDocument.Load(filePath + "settings.xml");
            settings = doc.Root;
        }

        public static string GetSetting(string key)
        {
            return settings.Element(key).Value;
        }
    }
}
