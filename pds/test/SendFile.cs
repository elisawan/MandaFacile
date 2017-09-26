using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;

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
            ProgressBar pBar1 = new ProgressBar();
            pBar1.Visible = true;
            // Set Minimum to 1 to represent the first file being copied.
            pBar1.Minimum = 1;
            // Set Maximum to the total number of files to copy.
            pBar1.Maximum = 8;
            // Set the initial value of the ProgressBar.
            pBar1.Value = 1;
            // Set the Step property to a value of 1 to represent each step being performed
            pBar1.Step = 1;


            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(receiver.get_address()), 15000);
            pBar1.PerformStep();

            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            pBar1.PerformStep();
            Console.WriteLine("ip: " + receiver.get_address());
            client.Connect(ipEndPoint);
            pBar1.PerformStep();

            string buffer;
            byte[] preBuf;
            byte[] postBuf;

            buffer = String.Format("R {0} 20", fileName);
            Console.WriteLine(buffer);
            pBar1.PerformStep();
            preBuf = Encoding.ASCII.GetBytes(buffer);
            pBar1.PerformStep();
            postBuf = Encoding.ASCII.GetBytes(_STRING_END_);
            pBar1.PerformStep();
            client.SendFile(fileName, preBuf, postBuf, TransmitFileOptions.UseDefaultWorkerThread);
            pBar1.PerformStep();
            Console.WriteLine("file sent");

            client.Shutdown(SocketShutdown.Both);
            pBar1.PerformStep();
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
