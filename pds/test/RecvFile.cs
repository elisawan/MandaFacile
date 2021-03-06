﻿using System;
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
        TcpListener server;

        public List<RecvFile> recvFiles = new List<RecvFile>();
        bool listening = true;
        Mandafacile mf;

        public Listen(Mandafacile mf)
        {
            this.mf = mf;
        }

        public void Start()
        {
            Console.WriteLine("Listen.Start()");
            try
            {
                Thread th = new Thread(Receive);
                th.IsBackground = true;
                th.Name = "RecvFileListen";
                th.Start();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                mf.Invoke(mf.fatalError, "Errore fatale");
            }
        }

        private void Receive()
        {
            Console.WriteLine("Listen.Receive()");

            // Buffers
            Byte[] byteBuffer = new Byte[Networking.TCP_limit];

            // Create socket
            server = new TcpListener(IPAddress.Any, Networking.TCP_port);

            // Start listening
            server.Start();

            // Infinite loop
            while (listening)
            {
                try
                {
                    Console.WriteLine("Listen.Receive(): listening...");
                    RecvFile receive = new RecvFile(this);
                    receive.set_client(server.AcceptTcpClient());
                    recvFiles.Add(receive);
                    // => Connection established
                    Console.WriteLine("Listen.Receive(): connected");
                    // Create new Thread only for managing this connection
                    Thread th = new Thread(receive.ServeClient);
                    th.Name = "RecvFile";
                    th.Start();
                }
                catch(SocketException e)
                {
                    if ((e.SocketErrorCode == SocketError.Interrupted))
                    {
                        listening = false;
                    }
                }
            }
        }

        public void Stop()
        {
            Console.WriteLine("Listen.Stop()");

            //verificare che non ci siano trasferimenti in corso
            if (recvFiles.Count == 0)
            {
                server.Stop();
            }
            else
            {
                MessageBox.Show("Impossibile passare alla modalità privata, trasferimento file in corso. Attendere la fine della trasmissione e ritentare");
                Properties.Settings.Default.pubblico = true;
            }
        }
    }

    class RecvFile : TCP
    {
        private TcpClient client;
        private int nRead;
        Listen listen;

        public AutoResetEvent terminateRecv = new AutoResetEvent(false);
        public ManualResetEvent doRecv = new ManualResetEvent(true);

        public RecvFile(Listen l)
        {
            listen = l;
        }

        public void set_client(TcpClient c)
        {
            client = c;
        }
        public void ServeClient()
        {
            Console.WriteLine("RecvFile.ServeClient()");
            string fileName;
            Int32 fileName_len;
            string path;
            Int32 fileLength;
            Int32 nLeft;
            FileStream fStream;

            byte[] byteBuffer = new byte[Networking.TCP_limit];
            string stringBuffer = null;

            WaitHandle[] handle = new WaitHandle[2];
            handle[0] = terminateRecv;
            handle[1] = doRecv;

            netStream = client.GetStream();


            // comando
            ReadnNetStream(ref byteBuffer, 1);
            stringBuffer = Encoding.ASCII.GetString(byteBuffer, 0, 1);
            // Richiesta di ricezione file
            if (stringBuffer.Equals(Networking.RECV_FILE))
            {
                // Leggere la lunghezza del nome del file
                ReadnNetStream(ref byteBuffer, sizeof(Int32));
                fileName_len = BitConverter.ToInt32(byteBuffer, 0);
                // Leggere il file name 
                ReadnNetStream(ref byteBuffer, fileName_len);
                fileName = Encoding.UTF8.GetString(byteBuffer).TrimEnd('\0');

                // TO DO: update GUI with fileName
                // TO DO: get confermation from GUI
                // TO DO: get path from GUI 
                bool accepted;
                if (Properties.Settings.Default.AcceptAll)
                {
                    accepted = true;
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Ricezione file: " + fileName, "MandaFacile", MessageBoxButtons.OKCancel);

                    if (dialogResult == DialogResult.OK)
                    {
                        accepted = true;
                    }
                    else
                    {
                        accepted = false;
                    }
                }

                if (accepted)
                {
                    byteBuffer = Encoding.ASCII.GetBytes(Networking._STRING_OK_);
                    netStream.Write(byteBuffer, 0, byteBuffer.Length);

                    if ((path = Properties.Settings.Default.Percorso) == null)
                    {
                        Console.WriteLine("Path null");
                        Properties.Settings.Default.Percorso = @"C:\Users\" + Environment.UserName + @"\Documents\Mandafacile";
                        Properties.Settings.Default.Save();
                        path = Properties.Settings.Default.Percorso;
                    }

                    // Manage file name conflicts
                    string fullPath = path + @"\" + fileName;
                    string fileNameOnly = Path.GetFileNameWithoutExtension(fileName);
                    int count = 1;
                    string extension = Path.GetExtension(fileName);
                    string newFullPath = fullPath;
                    while (File.Exists(newFullPath))
                    {
                        string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                        newFullPath = Path.Combine(path, tempFileName + extension);
                    }
                    fStream = File.OpenWrite(newFullPath);

                    // Read file length
                    ReadnNetStream(ref byteBuffer, sizeof(Int32));
                    fileLength = BitConverter.ToInt32(byteBuffer, 0);

                    // Read file data from socket and write it on local disk
                    byteBuffer = new byte[Networking.TCP_limit];
                    nLeft = fileLength;
                    bool stop = false;
                    while (nLeft > 0 && !stop)
                    {
                        if (nLeft >= Networking.TCP_limit)
                        {
                            ReadnNetStream(ref byteBuffer, Networking.TCP_limit);
                            nRead = Networking.TCP_limit;
                        }
                        else
                        {
                            ReadnNetStream(ref byteBuffer, nLeft);
                            nRead = nLeft;
                        }
                        if (Encoding.ASCII.GetString(byteBuffer).Contains(Networking._STRING_ERR_))
                        {
                            stop = true;
                            Console.WriteLine("Ricezione file fallito.");
                            fStream.Close();
                            File.Delete(path);
                        }
                        else
                        {
                            fStream.Write(byteBuffer, 0, nRead);
                            stringBuffer = Encoding.ASCII.GetString(byteBuffer);
                            nLeft -= nRead;
                        }
                    }
                    
                    fStream.Close();
                    netStream.Close();
                }
                else
                {
                    byteBuffer = Encoding.ASCII.GetBytes(Networking._STRING_NO_);
                    netStream.Write(byteBuffer, 0, byteBuffer.Length);
                    Console.WriteLine("ricezione file rifiutata");
                    netStream.Close();
                    //do something else
                }
            }
            listen.recvFiles.Remove(this);
        }
    }
}
