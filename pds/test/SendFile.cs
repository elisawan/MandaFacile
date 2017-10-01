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
        const int _STRING_END_LEN_ = 8;
        const string _STRING_ERR_ = "--ERROR--";
        const int _STRING_ERR_LEN_ = 9;
        const int _BUF_LEN_ = 1024;
        private string RECV_FILE = "R";

        String path { get; set; }
        IPAddress IP_sendTo;

        public AutoResetEvent terminateSend = new AutoResetEvent(false);
        public ManualResetEvent doSend = new ManualResetEvent(true); 

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

            int port = 15000;
            TcpClient client = new TcpClient(IP_sendTo.ToString(), port);
            NetworkStream stream = client.GetStream();

            string stringBuffer;
            byte[] byteBuffer;
            WaitHandle[] handles = new WaitHandle[2];
            handles[0] = terminateSend;
            handles[1] = doSend;
            FileInfo fileInfo;

            if (path == null)
            {
                path = "text.txt";
            }
            fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                using (FileStream fileStream = fileInfo.OpenRead())
                {
                    // --> "R<File_Name_Len><File_Name><File_Length>"
                    // R
                    stream.Write(Encoding.ASCII.GetBytes(RECV_FILE), 0, 1);
                    // <File_Name_Len>
                    byteBuffer = BitConverter.GetBytes(fileInfo.Name.Length);
                    stream.Write(byteBuffer, 0, byteBuffer.Length);
                    // <File_Name>
                    byteBuffer = Encoding.ASCII.GetBytes(fileInfo.Name);
                    stream.Write(byteBuffer, 0, byteBuffer.Length);
                    // <File_Len>
                    byteBuffer = BitConverter.GetBytes((Int32)fileInfo.Length);
                    stream.Write(byteBuffer, 0, byteBuffer.Length);
                    
                    // --> <File_Content>
                    Int32 nRead = -1;
                    byteBuffer = new Byte[1024];
                    Thread.Sleep(5000);
                    while ((WaitHandle.WaitAny(handles) == 1) && (nRead = fileStream.Read(byteBuffer, 0, byteBuffer.Length)) > 0) // solo doSend è segnalato
                    {
                       
                        stream.Write(byteBuffer, 0, nRead);
                        Console.WriteLine(Encoding.ASCII.GetString(byteBuffer));
                    }

                    // --> "--FINE--"
                    if (nRead == 0) // file inviato con successo
                    {
                        Console.WriteLine("file inviato");
                        byteBuffer = Encoding.ASCII.GetBytes(_STRING_END_);
                        stream.Write(byteBuffer, 0, _STRING_END_LEN_);
                    }
                    else // invio file fallito
                    {
                        Console.WriteLine("invio fallito");
                        byteBuffer = Encoding.ASCII.GetBytes(_STRING_ERR_);
                        stream.Write(byteBuffer, 0, _STRING_ERR_LEN_);
                    }
                }
            }
            
            stream.Close();
            client.Close();
        }

        public Thread Run()
        {
            Thread th = new Thread(Send);
            th.IsBackground = false;
            th.Name = "SendFile";
            th.Start();
            return th;
        }
    }
}
