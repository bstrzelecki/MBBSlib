using MBBSlib.Networking.Shared;
using System;
using System.Net;
using System.Net.Sockets;
namespace MBBSlib.Networking.Client
{
    public class TCPClient
    {
        TcpClient _socket;
        int id = -1;
        internal static int bufferSize = 1024;
        public int Id { get { return id; } }
        public Action<Command> OnCommandRecieved;
        public Action OnConnected;
        public Action OnCommandSent;
        private NetworkStream _stream;
        private byte[] recieveBuffer = new byte[bufferSize];
        public void Connect(string ip, int port)
        {
            _socket = new TcpClient
            {
                ReceiveBufferSize = bufferSize,
                SendBufferSize = bufferSize
            };
            _socket.BeginConnect(IPAddress.Parse(ip), port, ConnectCallback, _socket);
        }
        public void SendData(int cmd, byte[] data)
        {
            Command c = new Command(cmd, id, data);
            _stream.BeginWrite(c, 0, c.Size, SendCallback, null);
        }
        private void SendCallback(IAsyncResult ar)
        {
            OnCommandSent?.Invoke();
        }
        private void ConnectCallback(IAsyncResult ar)
        {
            _socket.EndConnect(ar);
            _stream = _socket.GetStream();
            OnConnected?.Invoke();
            _stream.BeginRead(recieveBuffer, 0, bufferSize, RecieveCallback, null);
        }

        private void RecieveCallback(IAsyncResult ar)
        {
            int bytes = _stream.EndRead(ar);
            byte[] input = new byte[bytes];
            Array.Copy(recieveBuffer, 0, input, 0, bytes);
            PacketRecieved(new Command(input));
            _stream.BeginRead(recieveBuffer, 0, bufferSize, RecieveCallback, null);
        }
        private void PacketRecieved(Command cmd)
        {
            if (cmd.Id == 1)
            {
                id = BitConverter.ToInt32(cmd.DataForm);
            }
            OnCommandRecieved?.Invoke(cmd);
        }
    }
}
