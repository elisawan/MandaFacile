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
        const string _STRING_END_ = "--FINE--";
        const int _STRING_END_LEN = 8;

        String fileName { get; set; }
        User receiver { get; set; }

        public SendFile(User receiver, String fileName)
        {
            this.fileName = fileName;
            this.receiver = receiver;
        }

        void Send()
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(receiver.get_address()), receiver.get_TCPPort());

            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("ip: " + receiver.get_address());
            client.Connect(ipEndPoint);

            string buffer;
            byte[] preBuf;
            byte[] postBuf;

            buffer = String.Format("R {0} 20", fileName);
            Console.WriteLine(buffer);
            preBuf = Encoding.ASCII.GetBytes(buffer);
            postBuf = Encoding.ASCII.GetBytes(_STRING_END_);
            client.SendFile(fileName, preBuf, postBuf, TransmitFileOptions.UseDefaultWorkerThread);
            Console.WriteLine("file sent");

            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        public void Run()
        {
            Thread th = new Thread(Send);
            th.IsBackground = false;
            th.Start();
        }
    }
}
