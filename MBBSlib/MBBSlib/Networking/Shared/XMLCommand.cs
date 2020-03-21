using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;

namespace MBBSlib.Networking.Shared
{
    public class XMLCommand
    {
        XDocument doc;


        public byte[] Serialize()
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
    }
}
