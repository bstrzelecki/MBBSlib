using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBBSlib.Networking.Shared
{
    public class Command
    {
        readonly byte[] _data = new byte[256];
        public int Size { get {
                return _data.Length;
            } }

        public int Id { get; private set; }
        public byte[] DataForm { get; private set; }
        public int Sender { get; private set; }
        public Command(int commandId, int sender, byte[] data)
        {
            Array.Copy(BitConverter.GetBytes(commandId), 0, _data, 0, 4);
            Array.Copy(BitConverter.GetBytes(sender), 0, _data, 4, 4);
            Array.Copy(data, 0, _data, 8, data.Length);


            Id = BitConverter.ToInt32(_data[0..4]);
            Sender = BitConverter.ToInt32(_data[4..8]);
            DataForm = _data[8..];
        }
        public Command(byte[] data)
        {
            _data = data;

            Id = BitConverter.ToInt32(_data[0..4]);
            Sender = BitConverter.ToInt32(_data[4..8]);
            DataForm = data[8..];
        }
        public static implicit operator byte[](Command cmd)
        {
            return cmd._data;
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