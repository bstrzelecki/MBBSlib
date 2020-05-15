using System;
using System.Text;

namespace MBBSlib.Networking.Shared
{
    /// <summary>
    /// Default class for labeling tcp data
    /// </summary>
    [Obsolete("Use XMLCommand instead.")]
    public class Command
    {
        /// <summary>
        /// Lengh of transmitted data
        /// </summary>
        public int Size => _data.Length;
        /// <summary>
        /// Command id
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Transmited data
        /// </summary>
        public byte[] DataForm { get; private set; }
        /// <summary>
        /// Id of origin ipendpoit (0 is reserved for server transmission)
        /// </summary>
        public int Sender { get; private set; }

        readonly byte[] _data = new byte[ConnectionData.BUFFER_SIZE];
        public Command(int commandId, int sender, byte[] data)
        {
            Array.Copy(BitConverter.GetBytes(commandId), 0, _data, 0, 4);
            Array.Copy(BitConverter.GetBytes(sender), 0, _data, 4, 4);
            Array.Copy(data, 0, _data, 8, data.Length);
            Array.Resize(ref _data, 8 + data.Length);

            Id = BitConverter.ToInt32(_data[0..4]);
            Sender = BitConverter.ToInt32(_data[4..8]);
            DataForm = _data[8..];
        }
        internal Command(byte[] data)
        {
            _data = data;
            Array.Resize(ref _data, 8 + data.Length);
            Id = BitConverter.ToInt32(_data[0..4]);
            Sender = BitConverter.ToInt32(_data[4..8]);
            DataForm = data[8..];
        }
        public static implicit operator XMLCommand(Command cmd)
        {
            var c = new XMLCommand();
            c.AddKey("id", cmd.Id);
            c.AddKey("sender", cmd.Sender);
            c.AddKey("data", Encoding.UTF8.GetString(cmd.DataForm));

            return c;
        }
        public static implicit operator byte[](Command cmd)
        {
            return cmd._data;
        }
        public override string ToString() => Encoding.ASCII.GetString(DataForm);
    }
}