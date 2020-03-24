using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;

namespace MBBSlib.Networking.Shared
{
    public class XMLCommand
    {
        private readonly XDocument doc;


        public XMLCommand()
        {
            doc = new XDocument();
            doc.Add(new XElement("Packet"));
        }
        public void AddKey(string key, object data)
        {
            XElement e = new XElement(key);
            e.SetAttributeValue("type", data.GetType().ToString());
            e.Value = data.ToString();
        }
        public XElement GetKey(string key)
        {
            return doc.Root.Element(key);
        }
        public IEnumerable<XElement> GetKeys(string key)
        {
            return doc.Root.Elements(key);
        }
        internal byte[] Serialize()
        {
            string data = doc.ToString();
            byte[] arr = Encoding.UTF8.GetBytes(data);
            arr = Compress(arr);
            return arr;
        }
        internal XMLCommand(byte[] arr)
        {
            arr = Decompress(arr);
            string data = Encoding.UTF8.GetString(arr);
            doc = XDocument.Parse(data);
        }
        private static byte[] Compress(byte[] data)
        {
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
            {
                dstream.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        private static byte[] Decompress(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }
        public static implicit operator XElement(XMLCommand cmd)
        {
            return cmd.doc.Root;
        }
    }
}
