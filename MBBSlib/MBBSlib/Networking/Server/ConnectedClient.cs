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
            SendData(new XMLCommand(1, 0, "grantedId", Id.ToString()));
            _server.BroadcastData(new XMLCommand(0, Id, "grantedId", Id), Id);
            _stream.BeginRead(recieveBuffer, 0, BUFFER_SIZE, RecieveCallBack, null);
        }
        [Obsolete]
        internal void SendData(Command cmd)
        {
            try
            {
                _stream.BeginWrite(cmd, 0, cmd.Size, null, null);
            }
            catch (Exception e)
            {
                _server.OnSocketException?.Invoke(e);
            }
        }
        internal void SendData(XMLCommand cmd)
        {
            try
            {
                byte[] arr = cmd.Serialize();
                _stream.BeginWrite(arr, 0, arr.Length, null, null);
            }
            catch (Exception e)
            {
                _server.OnSocketException?.Invoke(e);
            }
        }

        private void RecieveCallBack(IAsyncResult ar)
        {
            try
            {
                int bytes = _stream.EndRead(ar);
                byte[] input = new byte[bytes];
                Array.Copy(recieveBuffer, 0, input, 0, bytes);
                XMLCommand cmd = new XMLCommand(input);

                PacketRecieved(cmd);
                _stream.BeginRead(recieveBuffer, 0, BUFFER_SIZE, RecieveCallBack, null);
            }
            catch (Exception e)
            {
                _server.OnSocketException?.Invoke(e);
                this.Dispose();
            }
        }

        private void PacketRecieved(XMLCommand cmd)
        {
            if (_server._interpreters.ContainsKey(cmd.Id))
                _server._interpreters[cmd.Id].ExecuteCommand(cmd);
            _server.OnCommandRecieved?.Invoke(cmd);
        }

        public void Dispose()
        {
            _socket.Close();
            _stream.Close();
            _socket.Dispose();
            _stream.Dispose();
            _server.OnMessageBroadcast?.Invoke($"Client with id:{Id} disconnected form the server.");
            //Send Disconnect Packet
            _server.BroadcastData(new XMLCommand(2, Id, "disconnectInfo", "null"), Id);
            Id = -1;
        }
    }
}