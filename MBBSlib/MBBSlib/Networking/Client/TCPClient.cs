using MBBSlib.Networking.Shared;
using System;
using System.Net;
using System.Net.Sockets;
using static MBBSlib.Networking.Shared.ConnectionData;
namespace MBBSlib.Networking.Client
{
    /// <summary>
    /// Default cient networking class for tcp connection.
    /// </summary>
    public class TCPClient : IDisposable
    {
        /// <summary>
        /// Id of a client instance
        /// </summary>
        public int Id { get; private set; } = -1;
        /// <summary>
        /// Event that fires when client recieves a command.
        /// </summary>
        public event Action<XMLCommand> OnCommandRecieved;
        /// <summary>
        /// Event that fires whent client succesfuly connects to a remote host.
        /// </summary>
        public event Action OnConnected;
        /// <summary>
        /// Event that fires when client sents command to a remote host.
        /// </summary>
        public event Action OnCommandSent;
        /// <summary>
        /// Event that fires when client recieves a command and wasnt managed by preprocessor.
        /// </summary>
        public event Action<XMLCommand> OnNotManagedCommand;
        /// <summary>
        /// Event that fires when another client connects to the remote host.
        /// </summary>
        public event Action<int> OnClientConnected;
        /// <summary>
        /// Event that fires when another client disconnects to the remote host.
        /// </summary>
        public event Action<int> OnClientDisconnected;

        readonly TcpClient _socket;

        private NetworkStream _stream;
        private readonly byte[] recieveBuffer = new byte[BUFFER_SIZE];
        public TCPClient()
        {
            _socket = new TcpClient
            {
                ReceiveBufferSize = BUFFER_SIZE,
                SendBufferSize = BUFFER_SIZE
            };
        }
        /// <summary>
        /// Starts connection with a remote host.
        /// </summary>
        /// <param name="ip">ipv4 address (4 bytes string)</param>
        /// <param name="port">Server port</param>
        public void Connect(string ip, int port)
        {
            _socket.BeginConnect(IPAddress.Parse(ip), port, ConnectCallback, _socket);
        }
        /// <summary>
        /// Sends data directly to connected remote host.
        /// </summary>
        /// <param name="cmd">Id of data type (1-int.max)</param>
        /// <param name="data">1024 byte data array</param>
        [Obsolete]
        public void SendData(int cmd, byte[] data)
        {
            if (Id == -1)
                throw new Exception("Client has not connected to a remote host.");

            Command c = new Command(cmd, Id, data);
            _stream.BeginWrite(c, 0, c.Size, SendCallback, null);


        }
        /// <summary>
        /// Sends data directly to connected remote host.
        /// </summary>
        /// <param name="cmd"></param>
        public void SendData(XMLCommand cmd)
        {
            if (Id == -1)
                throw new Exception("Client has not been connected to a remote host.");
            cmd.Sender = Id;

            byte[] c = cmd.Serialize();
            _stream.BeginWrite(c, 0, c.Length, SendCallback, null);
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
            _stream.BeginRead(recieveBuffer, 0, BUFFER_SIZE, RecieveCallback, null);
        }

        private void RecieveCallback(IAsyncResult ar)
        {
            int bytes = _stream.EndRead(ar);
            byte[] input = new byte[bytes];
            Array.Copy(recieveBuffer, 0, input, 0, bytes);
            PacketRecieved(new XMLCommand(input));
            _stream.BeginRead(recieveBuffer, 0, BUFFER_SIZE, RecieveCallback, null);
        }
        private void PacketRecieved(XMLCommand cmd)
        {
            switch (cmd.Id)
            {
                case 0:
                    OnClientConnected?.Invoke(cmd.Sender);
                    break;
                case 1:
                    Id = cmd.GetInt("grantedId");
                    break;
                case 2:
                    OnClientDisconnected?.Invoke(cmd.Sender);
                    break;
                default:
                    OnNotManagedCommand?.Invoke(cmd);
                    break;
            }
            OnCommandRecieved?.Invoke(cmd);
        }

        public void Dispose()
        {
            _socket.Close();
            _stream.Close();
            _socket.Dispose();
            _stream.Dispose();
            _stream = null;
        }
    }
}
