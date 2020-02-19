using System.Net.Sockets;

namespace MBBSLib.Networking.Server
{
    internal class ConnectedClient
    {
        readonly Socket _socket;
        static int _id;
        public int Id { get; set; } = 0;
        public ConnectedClient(Socket client)
        {
            _socket = client;
            Id = ++_id;
        }
        public Socket GetSocket()
        {
            return _socket;
        }
    }
}