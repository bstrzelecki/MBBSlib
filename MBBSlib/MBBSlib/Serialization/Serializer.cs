using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MBBSlib.Serialization
{
    public class Serializer
    {
        static Dictionary<string, ISerializable> objs = new Dictionary<string, ISerializable>();

        public static void Register(string id, ISerializable serializable)
        {
            objs.Add(id, serializable);
        }
        public static void Save(string file)
        {
            XDocument doc = new XDocument();
            doc.Add(new XElement("Root"));
            XElement root = doc.Root;

            foreach(string id in objs.Keys)
            {
                NBTCompund compund = new NBTCompund(id);
                objs[id].Save(compund);
                root.Add(compund.GetData());
            }
            doc.Save(file);
        }
        public static void Load()
        {

        }
    }
}
