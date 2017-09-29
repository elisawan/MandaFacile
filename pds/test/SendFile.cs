using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.IO;

namespace test
{
    class SendFile
    {
        const string _STRING_END_ = "--FINE--";
        const int _STRING_END_LEN = 8;

        String path { get; set; }
        IPAddress IP_sendTo;

        public SendFile(String IP, String path)
        {
            this.path = path;
            this.IP_sendTo = IPAddress.Parse(IP);
        }

        void Send()
        {
            ProgressBar pBar1 = new ProgressBar();
            // Display the ProgressBar control.
            pBar1.Visible = true;
            // Set Minimum to 1 to represent the first file being copied.
            pBar1.Minimum = 1;
            // Set Maximum to the total number of files to copy.
            pBar1.Maximum = 100;
            // Set the initial value of the ProgressBar.
            pBar1.Value = 1;
            // Set the Step property to a value of 1 to represent each file being copied.
            pBar1.Step = 1;
            pBar1.Style = ProgressBarStyle.Continuous;



            for (int i = 0; i < 1000; i++)
            {
                Application.DoEvents();
                pBar1.PerformStep();
                Console.WriteLine(i);
            }


            /////////////////////

            IPEndPoint ipEndPoint = new IPEndPoint(IP_sendTo, 15000);

            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("ip: " + IP_sendTo);
            client.Connect(ipEndPoint);

            string buffer;
            byte[] preBuf;
            byte[] postBuf;

            if(path == null)
            {
                path = "text.txt";
            }
            FileInfo fileInfo = new FileInfo(path);
           
            buffer = String.Format("R " + fileInfo.Name + " " + fileInfo.Length);
            Console.WriteLine(buffer);
            preBuf = Encoding.ASCII.GetBytes(buffer);
            postBuf = Encoding.ASCII.GetBytes(_STRING_END_);
            client.SendFile(path, preBuf, postBuf, TransmitFileOptions.UseDefaultWorkerThread);
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
