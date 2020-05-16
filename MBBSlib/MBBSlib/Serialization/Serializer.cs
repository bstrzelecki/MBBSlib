using System.Collections.Generic;
using System.Xml.Linq;

namespace MBBSlib.Serialization
{
    public class Serializer
    {
        static readonly Dictionary<string, ISerializable> objs = new Dictionary<string, ISerializable>();

        /// <summary>
        /// Registers objects and issues serialization during static calls
        /// </summary>
        /// <param name="id">key of an object</param>
        /// <param name="serializable"></param>
        public static void Register(string id, ISerializable serializable) => objs.Add(id, serializable);
        public static void Save(string file)
        {
            var doc = new XDocument();
            doc.Add(new XElement("Root"));
            XElement root = doc.Root;

            foreach(string id in objs.Keys)
            {
                var compund = new NBTCompund(id);
                objs[id].Save(compund);
                root.Add(compund.GetData());
            }
            doc.Save(file);
        }
        public static void Load(string file)
        {
            var doc = XDocument.Load(file);
            XElement root = doc.Root;
            foreach(var id in objs.Keys)
            {
                objs[id].Load(new NBTCompund(root.Element(id)));
            }
        }
    }
}
