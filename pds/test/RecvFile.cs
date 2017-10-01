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
    static class Listen
    {
        const int BUF_LEN = 1024;
        const string RECV_FILE = "R";
       
        const Int32 port = 15000;

        static private volatile bool listen;

        static public void Start()
        {
            Console.WriteLine("Listen.Start()");
            listen = true;
            Thread th = new Thread(Receive);
            th.IsBackground = true;
            th.Name = "RecvFileListen";
            th.Start();
        }

        static private void Receive()
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
                th.Name = "RecvFile";
                th.Start();
            }

            // Close server
            server.Stop();
        }

        static public void Stop()
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
        const string _STRING_END_ = "--FINE--";
        const int _STRING_END_LEN_ = 8;
        const string _STRING_ERR_ = "--ERROR--";
        const int _STRING_ERR_LEN_ = 9;

        public AutoResetEvent terminateRecv = new AutoResetEvent(false);
        public ManualResetEvent doRecv = new ManualResetEvent(true);

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

            byte[] byteBuffer = new byte[BUF_LEN];
            string stringBuffer = null;

            WaitHandle[] handle = new WaitHandle[2];
            handle[0] = terminateRecv;
            handle[1] = doRecv;

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

            // comando
            nRead = nStream.Read(byteBuffer, 0, 1);
            stringBuffer = Encoding.ASCII.GetString(byteBuffer, 0, nRead);
            Console.WriteLine(stringBuffer);
            // Richiesta di ricezione file
            if (stringBuffer.Equals(RECV_FILE))
            {
                // Leggere la lunghezza del nome del file
                nRead = nStream.Read(byteBuffer, 0, sizeof(Int32));
                fileName_len = BitConverter.ToInt32(byteBuffer, 0);
                // Leggere il file name 
                nRead = nStream.Read(byteBuffer, 0, fileName_len);
                fileName = Encoding.UTF8.GetString(byteBuffer).TrimEnd('\0');

                // TO DO: update GUI with fileName
                // TO DO: get confermation from GUI
                // TO DO: get path from GUI 

                DialogResult dialogResult = MessageBox.Show("Ricezione file: " + fileName, "MandaFacile", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    if ((path = Properties.Settings.Default.Percorso) == null)
                    {
                        Console.WriteLine("Path null");
                        Properties.Settings.Default.Percorso = @"C:\Users\" + Environment.UserName + @"\Documents\Mandafacile";
                        Properties.Settings.Default.Save();
                        path = Properties.Settings.Default.Percorso;
                    }
                    path = path + @"\" + fileName;
                    // TO DO: manage file name conflicts
                    fStream = File.OpenWrite(path);

                    // Read file length
                    nRead = nStream.Read(byteBuffer, 0, sizeof(Int32));
                    fileLength = BitConverter.ToInt32(byteBuffer, 0);

                    // Read file data from socket and write it on local disk
                    nLeft = fileLength;
                    bool stop = false;
                    while (nLeft > 0 && !stop)
                    {
                        Console.WriteLine("READ");
                        if (nLeft >= BUF_LEN)
                        {
                            nRead = nStream.Read(byteBuffer, 0, BUF_LEN);
                        }
                        else
                        {
                            nRead = nStream.Read(byteBuffer, 0, nLeft);
                        }
                        if (Encoding.ASCII.GetString(byteBuffer).Contains(_STRING_ERR_))
                        {
                            stop = true;
                            Console.WriteLine("errore");
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
                    nStream.Close();
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