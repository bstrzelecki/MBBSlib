using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;

namespace MBBSlib.Networking.Shared
{
    /// <summary>
    /// TCP packet represented as xml document
    /// </summary>
    public class XMLCommand
    {
        private XDocument doc;

        public int Id => int.Parse(Header.Element("id").Value);
        public int Sender
        {
            get => int.Parse(Header.Element("sender").Value);
            set
            {
                if(Header.Element("sender") == null)
                {
                    Header.Add(new XElement("sender"), value);
                }
                else
                {
                    Header.Element("sender").Value = value.ToString();
                }
            }
        }
        public XElement Data => doc.Root.Element("Data");
        internal XElement Header => doc.Root.Element("Header");
        /// <summary>
        /// Initializes object with default packet formating
        /// </summary>
        public XMLCommand()
        {
            InitializeXML();
        }
        /// <summary>
        /// Adds key to the serializable packet
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void AddKey(string key, object data)
        {
            var e = new XElement(key);
            e.SetAttributeValue("type", data.GetType().ToString());
            e.Value = data.ToString();
            Data.Add(e);
        }
        /// <summary>
        /// Deserializes key from packet data
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public XElement GetKey(string key) => Data.Element(key);
        /// <summary>
        /// Deserializes multiple keys from packet data with the same id
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<XElement> GetKeys(string key) => doc.Root.Elements(key);
        public object GetValue(string key)
        {
            string s = Data.Element(key).Value;
            string t = Data.Element(key).Attribute("type").Value;
            TypeConverter tc = TypeDescriptor.GetConverter(Type.GetType(t));
            object obj = tc.ConvertFromString(s);

            return obj;
        }
        public T GetValue<T>(string key) => (T)GetValue(key);
        public int GetInt(string key) => int.Parse(GetKey(key).Value);
        public string GetString(string key) => GetKey(key).Value;
        public bool ContainsKey(string s)
        {
            if (Data.Element(s) != null) return true;
            return false;
        }
        public XMLCommand(int commandId, int sender, string key, object value)
        {
            InitializeXML();
            Header.Add(new XElement("id", commandId.ToString()));
            Header.Add(new XElement("sender", sender.ToString()));

            AddKey(key, value);
        }
        private void InitializeXML()
        {
            doc = new XDocument();
            var packet = new XElement("Packet");
            packet.Add(new XElement("Header"));
            packet.Add(new XElement("Data"));
            doc.Add(packet);
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
            var output = new MemoryStream();
            using (var dstream = new DeflateStream(output, CompressionLevel.Optimal))
            {
                dstream.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        private static byte[] Decompress(byte[] data)
        {
            var input = new MemoryStream(data);
            var output = new MemoryStream();
            using (var dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }
        public static implicit operator XElement(XMLCommand cmd)
        {
            return cmd.Data;
        }
    }
}
