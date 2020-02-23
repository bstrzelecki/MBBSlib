using MBBSlib.Networking.Shared;
using System;
using System.Net.Sockets;
using static MBBSlib.Networking.Shared.ConnectionData;

namespace MBBSlib.Networking.Server
{
    internal class ConnectedClient : IDisposable
    {
        readonly TcpClient _socket;
        public int Id { get; protected set; } = 0;

        private readonly NetworkStream _stream;
        private readonly TCPServer _server;
        private byte[] recieveBuffer = new byte[BUFFER_SIZE];
        internal ConnectedClient(int id, TcpClient client, TCPServer server)
        {
            Id = id;
            _server = server;
            _socket = client;
            _socket.ReceiveBufferSize = BUFFER_SIZE;
            _socket.SendBufferSize = BUFFER_SIZE;

            _stream = _socket.GetStream();
            SendData(new Command(1, 0, BitConverter.GetBytes(Id)));

            _stream.BeginRead(recieveBuffer, 0, BUFFER_SIZE, RecieveCallBack, null);
        }
        internal void SendData(Command cmd)
        {
            try
            {
                _stream.BeginWrite(cmd, 0, cmd.Size, SendCallback, null);
            }
            catch (Exception e)
            {
                _server.OnSocketException?.Invoke(e);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {

        }

        private void RecieveCallBack(IAsyncResult ar)
        {
            try
            {
                int bytes = _stream.EndRead(ar);
                byte[] input = new byte[bytes];
                Array.Copy(recieveBuffer, 0, input, 0, bytes);
                Command cmd = new Command(input);

                PacketRecieved(cmd);
                _stream.BeginRead(recieveBuffer, 0, BUFFER_SIZE, RecieveCallBack, null);
            }
            catch (Exception e)
            {
                _server.OnSocketException?.Invoke(e);
                this.Dispose();
            }
        }

        private void PacketRecieved(Command cmd)
        {
            _server.OnCommandRecieved?.Invoke(cmd);
        }

        public void Dispose()
        {
            _socket.Close();
            _stream.Close();
            _socket.Dispose();
            _stream.Dispose();
            _server.OnMessageBroadCast?.Invoke($"Client with id:{Id} disconnected form the server.");
            Id = -1;
        }
    }
}