using MBBSLib.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MBBSLib.Networking.Client
{
    public class TCPClient
    {
        Socket _socket;
        int id = -1;
        public int Id{ get { return id;} }

        readonly volatile Queue<Command> recievedData = new Queue<Command>();
        public TCPClient(string ip, int port)
        {
            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            SendData(1, new byte[0]);
            Command cmd = RecieveData();
            if(cmd == 1)
            {
                id = BitConverter.ToInt32(cmd.DataForm);
            }
            new Thread(() => 
            {
                recievedData.Enqueue(RecieveData());
            }).Start();

        }
        public Command GetRecievedData()
        {
            if(recievedData.Count > 0)
            {
                return recievedData.Dequeue();
            }
            else
            {
                return null;
            }
        }
        public void SendData(int cmd, byte[] data)
        {
            byte[] command = new byte[(8 + data.Length)];
            BitConverter.GetBytes(cmd).CopyTo(command, 0);
            BitConverter.GetBytes(id).CopyTo(command, 4);
            Debug.WriteLine(command[4..8]);
            data.CopyTo(command, 8);
            _socket.BeginSend(command, 0, command.Length, SocketFlags.None, SendCallback, null);
        }
        private void SendCallback(IAsyncResult a)
        {
            Debug.WriteLine("Package sent");
        }
        public Command RecieveData()
        {
            byte[] buff = new byte[256];

            _socket.Receive(buff);
            return new Command(buff);
        }
    }
}
