using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace test
{
    class SendFile
    {
        String fileName { get; set; }
        User receiver { get; set; }

        public SendFile(User receiver, String fileName)
        {
            this.fileName = fileName;
            this.receiver = receiver;
        }

        void Send()
        {
            Socket sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint recvEnd = new IPEndPoint(IPAddress.Parse(receiver.get_address()), receiver.get_TCPPort());

            sendSocket.Connect(recvEnd);
            Console.WriteLine("sending socket created");

            string length = String.Format("File length: {0}", 20);
            byte[] preBuf = Encoding.ASCII.GetBytes(length);
            sendSocket.SendFile(fileName, preBuf, null, TransmitFileOptions.UseDefaultWorkerThread);
            Console.WriteLine("file sent");

            sendSocket.Shutdown(SocketShutdown.Both);
            sendSocket.Close();
        }

        public void Run()
        {
            Thread th = new Thread(Send);
            //th.IsBackground = true;
            th.Start();
        }
    }
}
