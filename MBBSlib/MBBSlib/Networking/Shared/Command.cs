using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBBSLib.Networking
{
    public class Command
    {
        readonly byte[] _data = new byte[256];

        public int Id { get; private set; }
        public byte[] DataForm { get; private set; }
        public int Sender { get; private set; }

        public Command(byte[] data)
        {
            _data = data;

            Id = BitConverter.ToInt32(_data[0..4]);
            Sender = BitConverter.ToInt32(_data[4..8]);
            DataForm = data[8..];
        }
        public static implicit operator string(Command cmd)
        {
            return Encoding.ASCII.GetString(cmd.DataForm);
        }
        public override string ToString()
        {
            return Encoding.ASCII.GetString(DataForm);
        }
        public static implicit operator int(Command cmd)
        {
            return cmd.Id;
        }
    }
}