using System;
using System.Net.Sockets;
using MBBSlib.Networking.Shared;

namespace MBBSlib.Networking.Server
{
    public class ConnectedClient
    {
        readonly TcpClient _socket;
        static int _id;
        public int Id { get; set; } = 0;
        internal static int bufferSize = 1024;
        private NetworkStream _stream;
        private TCPServer _server;
        private byte[] recieveBuffer = new byte[bufferSize];
        internal ConnectedClient(TcpClient client, TCPServer server)
        {
            Id = ++_id;
            _server = server;
            _socket = client;
            _socket.ReceiveBufferSize = bufferSize;
            _socket.SendBufferSize = bufferSize;

            _stream = _socket.GetStream();
            SendData(new Command(1, 0, BitConverter.GetBytes(Id)));

            _stream.BeginRead(recieveBuffer, 0, bufferSize, RecieveCallBack, null);
        }
        public void SendData(Command cmd)
        {
            _stream.BeginWrite(cmd, 0, cmd.Size, SendCallback, null);
        }

        private void SendCallback(IAsyncResult ar)
        {
            
        }

        private void RecieveCallBack(IAsyncResult ar)
        {
            int bytes = _stream.EndRead(ar);
            byte[] input = new byte[bytes];
            Array.Copy(recieveBuffer, 0, input, 0, bytes);
            Command cmd = new Command(input);

            PacketRecieved(cmd);
            _stream.BeginRead(recieveBuffer, 0, bufferSize, RecieveCallBack, null);
        }

        private void PacketRecieved(Command cmd)
        {
            _server.OnCommandRecieved?.Invoke(this, cmd);   
        }

        public TcpClient GetSocket()
        {
            return _socket;
        }
    }
}