using MBBSlib.Networking.Shared;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
namespace MBBSlib.Networking.Server
{
    /// <summary>
    /// Default server networking class for tcp connection.
    /// </summary>
    public class TCPServer : IDisposable
    {
        /// <summary>
        /// Port on what server will be listening 
        /// </summary>
        public int Port { get; set; } = 25565;

        /// <summary>
        /// Event that fires when client connects to the server
        /// </summary>
        public Action<int> OnClientConnected;
        /// <summary>
        /// Event that fires when server recieves command from client
        /// </summary>
        public Action<int, Command> OnCommandRecieved;
        /// <summary>
        /// Event that fires when excentpion is thrown
        /// </summary>
        public Action<Exception> OnSocketException;
        /// <summary>
        /// Event that fires when servers outputs debug information
        /// </summary>
        public Action<string> OnMessageBroadCast;
        
        TcpListener _server;
        readonly List<ConnectedClient> _clients = new List<ConnectedClient>();
        /// <summary>
        /// Start listening on predefined port
        /// </summary>
        public void Start()
        {
            _server = new TcpListener(IPAddress.Any, Port);
            _server.Start();
            _server.BeginAcceptTcpClient(AccepetedClientCallback, null);
            OnMessageBroadCast?.Invoke("Server started.");
        }
        private void AccepetedClientCallback(IAsyncResult a)
        {
            try
            {
                TcpClient client = _server.EndAcceptTcpClient(a);
                ClearClientList();
                int id = 0;
                for (int i = 256; i > 0; i--)
                {
                    bool isValid = true;
                    foreach (ConnectedClient c in _clients)
                    {
                        if (c.Id == i)
                        {
                            isValid = false;
                            break;
                        }
                    }
                    if(isValid)
                        id = i;
                }

                ConnectedClient cl = new ConnectedClient(id, client, this);
                _clients.Add(cl);
                OnClientConnected?.Invoke(cl.Id);
                OnMessageBroadCast?.Invoke($"{client.Client.RemoteEndPoint} connected.");
                _server.BeginAcceptTcpClient(AccepetedClientCallback, null);
            }
            catch (Exception e)
            {
                OnSocketException?.Invoke(e);
            }
        }
        private void ClearClientList()
        {
            for(int i = 0; i < _clients.Count; i++)
            {
                if (_clients[i].Id == -1) _clients.RemoveAt(i);
            }
        }
        public void Dispose()
        {
            foreach(ConnectedClient c in _clients)
            {
                c.Dispose();
            }
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
