using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;
using MBBSLib.Networking;

namespace MBBSLib.Networking.Server
{
    public class TCPServer :IDisposable
    {
        public int BackLogSize { get; set; } = 6;
        public int Port { get; set; } = 25565;

        readonly Socket _server;
        readonly List<ConnectedClient> _clients = new List<ConnectedClient>();
        readonly Thread _serverThread;
        public Queue<Command> Requests { get; private set; } = new Queue<Command>();

        public TCPServer()
        {
            _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _server.Bind(new IPEndPoint(IPAddress.Any, Port));
            _server.ReceiveTimeout = 10;
            _server.SendTimeout = 1000;
            _serverThread = new Thread(() =>
            {
                while (true)
                {
                    _server.Listen(BackLogSize);
                    _server.BeginAccept(AccepetedSocketCallback, null);
                    try
                    {
                        foreach (ConnectedClient client in _clients)
                        {
                            Command cmd = RecieveData(client);
                            if (cmd != null)
                            {
                                if (cmd == (int)CommandId.Join)
                                {
                                    SendData(client, 1, BitConverter.GetBytes(client.Id));
                                    continue;
                                }
                                
                                Requests.Enqueue(cmd);
                            }
                        }
                    }
                    catch (Exception e) 
                    {
                        Debug.WriteLine(e.ToString());
                    }
                }
            });
            _serverThread.Start();
        }
        public Command GetRequest()
        {
            if(Requests.Count > 0)
            {
                return Requests.Dequeue();
            }
            else
            {
                return null;
            }
        }
        private void AccepetedSocketCallback(IAsyncResult a)
        {
            Socket client = _server.EndAccept(a);

            ConnectedClient cl = new ConnectedClient(client);
            _clients.Add(cl);
        }
        private void SendSocketCallback(IAsyncResult a)
        {
            Debug.WriteLine("Package sent.");
        }
        private Command RecieveData(ConnectedClient client)
        {
            byte[] buff = new byte[256];
            try
            {
                int i = client.GetSocket().Receive(buff);
                if (i > 0)
                {
                    Array.Resize(ref buff, i);
                    Command cmd = new Command(buff);
                    string s = cmd;
                    Debug.WriteLine(cmd);
                    return cmd;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        private void SendData(ConnectedClient client, int cmd, byte[] data, int sender = 0)
        {
            byte[] command = new byte[8 + data.Length];
            BitConverter.GetBytes(cmd).CopyTo(command,0);
            BitConverter.GetBytes(0).CopyTo(command,4);
            data.CopyTo(command, 8);
            client.GetSocket().BeginSend(command, 0, command.Length, SocketFlags.None, SendSocketCallback, null);
        }

        public void Dispose()
        {
            _serverThread.Abort();
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
