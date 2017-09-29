using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;

namespace test
{
    class Listen
    {
        const int BUF_LEN = 1024;
        const string RECV_FILE = "R";

        Int32 port = 15000;

        private volatile bool listen;

        public void Start()
        {
            Console.WriteLine("Listen.Start()");
            listen = true;
            Thread th = new Thread(Receive);
            th.IsBackground = true;
            th.Start();
        }

        private void Receive()
        {
            Console.WriteLine("Listen.Receive()");

            // Buffers
            Byte[] byteBuffer = new Byte[BUF_LEN];

            // Create socket
            TcpListener server = new TcpListener(IPAddress.Any, port);

            // Start listening
            server.Start();

            // Infinite loop
            while (listen)
            {
                Console.WriteLine("Listen.Receive(): listening...");
                RecvFile receive = new RecvFile();
                receive.set_client(server.AcceptTcpClient());
                // => Connection established
                Console.WriteLine("Listen.Receive(): connected");
                // Create new Thread only for managing this connection
                Thread th = new Thread(receive.ServeClient);
                th.Start();
            }

            // Close server
            server.Stop();
        }

        public void Stop()
        {
            Console.WriteLine("Listen.Stop()");
            listen = false;
        }
    }

    class RecvFile
    {
        private TcpClient client;
        private int nRead;
        private int BUF_LEN = 1024;
        private string RECV_FILE = "R";

        public void set_client(TcpClient c)
        {
            client = c;
        }
        public void ServeClient()
        {
            Console.WriteLine("RecvFile.ServeClient()");
            string fileName;
            string path;
            Int32 fileLength;
            Int32 nLeft;
            FileStream fStream;

            byte[] byteBuffer = new byte[BUF_LEN];
            string stringBuffer = null;

            NetworkStream nStream = client.GetStream();

            // Read command: 'R' => Richiesta di ricezione file 

            /*//Informo l'utente che sta ricevendo un file e chiedo se vuole interrompere (o accettare?)
             * const string message = "Stai ricevendo un file. Interrompere?";
             * const string caption = "Interrompi ricezione";
             * var result = MessageBox.Show(message, caption,
             * MessageBoxButtons.YesNo, MessageBoxIcon.Question);
             * 
             * if (result == DialogResult.No)
             * //ecc....
             * 
             * 
             * 
             * 
             */


            nRead = nStream.Read(byteBuffer, 0, 1);
            if (nRead != 1)
            {
                // TO DO: Throw exception
            }
            stringBuffer = Encoding.ASCII.GetString(byteBuffer, 0, nRead);
            Console.WriteLine(stringBuffer);
            // Richiesta di ricezione file
            if (stringBuffer.Equals(RECV_FILE))
            {
                Console.WriteLine("RECV_FILE");
                // Leggere il file name 
                nRead = nStream.Read(byteBuffer, 0, 1);
                if((nRead != 1) || (!Encoding.ASCII.GetString(byteBuffer).Equals(" ")))
                {
                    // TO DO: Throw exception
                }
                nRead = nStream.Read(byteBuffer, 0, 8);
                if (nRead <= 0)
                {
                    // TO D0: Throw exception
                }
                fileName = Encoding.ASCII.GetString(byteBuffer, 0, nRead);
                Console.WriteLine(fileName);
                // TO DO: update GUI with fileName
                // TO DO: get confermation from GUI
                // TO DO: get path from GUI 
               
                if((path = Properties.Settings.Default.Percorso) == null)
                {
                    Console.WriteLine("Path null");
                    Properties.Settings.Default.Percorso = @"C:\Users\" + Environment.UserName + @"\Documents\Mandafacile";
                    Properties.Settings.Default.Save();
                    path = Properties.Settings.Default.Percorso;
                }
                Console.WriteLine(path);
                path = path + @"\" + fileName;

                DialogResult dialogResult = MessageBox.Show("Ricezione file: " + fileName, "MandaFacile", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    // TO DO: manage file name conflicts
                    fStream = File.Create(path);

                    // Read file length
                    nStream.Read(byteBuffer, 0, 1);
                    nRead = nStream.Read(byteBuffer, 0, 2);
                    if (nRead <= 0)
                    {
                        // TO DO: throw exception
                    }

                    stringBuffer = ASCIIEncoding.ASCII.GetString(byteBuffer, 0, 2);
                    fileLength = 8;
                    Console.WriteLine(fileLength);
                    // Read file data from socket and write it on local disk
                    nLeft = fileLength;
                    while (nLeft > 0)
                    {
                        if (nLeft >= BUF_LEN)
                        {
                            nRead = nStream.Read(byteBuffer, 0, BUF_LEN);
                        }
                        else
                        {
                            nRead = nStream.Read(byteBuffer, 0, nLeft);
                        }
                        nLeft -= nRead;
                        fStream.Write(byteBuffer, 0, nRead);
                    }
                    fStream.Close();
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    Console.WriteLine("ricezione file rifiutata");
                    //do something else
                }

            }
        }
    }
}
