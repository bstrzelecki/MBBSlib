using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MBBSlib.Networking.Shared;
namespace MBBSlib.Networking.Server
{
    public class TCPServer :IDisposable
    {
        public int Port { get; set; } = 25565;

        TcpListener _server;
        readonly List<ConnectedClient> _clients = new List<ConnectedClient>();
        public Action<ConnectedClient> OnClientConnected;
        public Action<ConnectedClient, Command> OnCommandRecieved;
        public void Start()
        {
            _server = new TcpListener(IPAddress.Any, Port);
            _server.Start();
            _server.BeginAcceptTcpClient(AccepetedClientCallback, null);
            Console.WriteLine("Server started.");
        }
        private void AccepetedClientCallback(IAsyncResult a)
        {
            TcpClient client = _server.EndAcceptTcpClient(a);

            ConnectedClient cl = new ConnectedClient(client, this);
            _clients.Add(cl);
            OnClientConnected?.Invoke(cl);
            Console.WriteLine($"{client.Client.RemoteEndPoint} connected.");
            _server.BeginAcceptTcpClient(AccepetedClientCallback, null);
        }
        public void Dispose()
        {
            _server.Stop();
        }
    }
    public enum CommandId
        {
            Error,
            Join,
            Disconnect,
            ConnectionAccepted,
            Ping
        }
}
