using MBBSlib.Networking.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public Action<Command> OnCommandRecieved;
        /// <summary>
        /// Event that fires when excentpion is thrown
        /// </summary>
        public Action<Exception> OnSocketException;
        /// <summary>
        /// Event that fires when servers outputs debug information
        /// </summary>
        public Action<string> OnMessageBroadcast;

        TcpListener _server;
        readonly List<ConnectedClient> _clients = new List<ConnectedClient>();
        internal readonly Dictionary<int, ICommandInterpreter> _interpreters = new Dictionary<int, ICommandInterpreter>();
        /// <summary>
        /// Registers command interpreter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ici"></param>
        public void RegisterInterpreter(int id, ICommandInterpreter ici)
        {
            _interpreters.Add(id, ici);
        }
        /// <summary>
        /// Unregisters command interpreter
        /// </summary>
        /// <param name="ici"></param>
        public void UnregisterInterpreter(ICommandInterpreter ici)
        {
            foreach (var i in _interpreters.Keys)
            {
                if (_interpreters[i] == ici)
                    _interpreters.Remove(i);
            }
        }
        /// <summary>
        /// Start listening on predefined port
        /// </summary>
        public void Start()
        {
            _server = new TcpListener(IPAddress.Any, Port);
            _server.Start();
            _server.BeginAcceptTcpClient(AccepetedClientCallback, null);
            OnMessageBroadcast?.Invoke("Server started.");
        }
        /// <summary>
        /// Sends data to specified client
        /// </summary>
        /// <param name="clientid">Id of the client data will be sent</param>
        /// <param name="cmd">Command that will be delivered to specified client</param>
        public void SendData(int clientid, Command cmd)
        {
            foreach (ConnectedClient c in _clients)
            {
                if (c.Id == clientid)
                {
                    c.SendData(cmd);
                }
            }
        }
        /// <summary>
        /// Sends data to all connected clients exept specified ones
        /// </summary>
        /// <param name="cmd">Command that will be delivered to clients</param>
        /// <param name="ids">List of ids to which command will not be sent</param>
        public void BroadcastData(Command cmd, params int[] ids)
        {
            foreach (ConnectedClient c in _clients)
            {
                if (!ids.Contains(c.Id))
                    c.SendData(cmd);
            }
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
                    if (isValid)
                        id = i;
                }

                ConnectedClient cl = new ConnectedClient(id, client, this);
                _clients.Add(cl);
                OnClientConnected?.Invoke(cl.Id);
                OnMessageBroadcast?.Invoke($"{client.Client.RemoteEndPoint} connected.");
                _server.BeginAcceptTcpClient(AccepetedClientCallback, null);
            }
            catch (Exception e)
            {
                OnSocketException?.Invoke(e);
            }
        }
        private void ClearClientList()
        {
            for (int i = 0; i < _clients.Count; i++)
            {
                if (_clients[i].Id == -1) _clients.RemoveAt(i);
            }
        }
        /// <summary>
        /// Clears memory and disconects all clients
        /// </summary>
        public void Dispose()
        {
            foreach (ConnectedClient c in _clients)
            {
                c.Dispose();
            }
            _server.Stop();
        }
    }
}
