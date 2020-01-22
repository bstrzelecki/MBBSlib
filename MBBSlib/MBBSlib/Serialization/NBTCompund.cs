using System.Xml.Linq;

namespace MBBSlib.Serialization
{
    public class NBTCompund
    {
        XElement tree;
        public NBTCompund(string id)
        {
            tree = new XElement(id);
        }
        public NBTCompund(XElement b)
        {
            tree = b;
        }
        public void SaveTag<T>(string tag, T data)
        {
            AddElement(new XElement(tag, data.ToString()));
        }
        public void SaveTag(string tag, object data)
        {
            AddElement(new XElement(tag, data.ToString()));
        }
        public string LoadNBT(string tag)
        {
            return tree.Element(tag).Value;
        }
        public void AddElement(XElement element)
        {
            tree.Add(element);
        }
        public XElement GetData()
        {
            return tree;
        }
    }
}
