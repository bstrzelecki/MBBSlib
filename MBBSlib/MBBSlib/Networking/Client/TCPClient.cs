﻿using MBBSlib.Networking.Shared;
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
        /// Event that fires when client reciueves a command.
        /// </summary>
        public event Action<Command> OnCommandRecieved;
        /// <summary>
        /// Event that fires whent client succesfuly connects to a remote host.
        /// </summary>
        public event Action OnConnected;
        /// <summary>
        /// Event that fires when client sents command to a remote host.
        /// </summary>
        public event Action OnCommandSent;

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
        public void SendData(int cmd, byte[] data)
        {
            if (Id == -1)
                throw new Exception("Client has not connected to a remote host."); 
            
            Command c = new Command(cmd, Id, data);
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
            _stream.BeginRead(recieveBuffer, 0, BUFFER_SIZE, RecieveCallback, null);
        }

        private void RecieveCallback(IAsyncResult ar)
        {
            int bytes = _stream.EndRead(ar);
            byte[] input = new byte[bytes];
            Array.Copy(recieveBuffer, 0, input, 0, bytes);
            PacketRecieved(new Command(input));
            _stream.BeginRead(recieveBuffer, 0, BUFFER_SIZE, RecieveCallback, null);
        }
        private void PacketRecieved(Command cmd)
        {
            if (cmd.Id == 1)
            {
                Id = BitConverter.ToInt32(cmd.DataForm);
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
