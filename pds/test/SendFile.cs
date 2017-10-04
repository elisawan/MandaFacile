﻿using System;
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
        const string _STRING_OK_ = "--OK--";
        const string _STRING_NO_ = "--NO--";
        const int _STRING_NO_LEN_ = 6;
        const int _STRING_OK_LEN_ = 6;
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
            Mandafacile.progresso++;

            int port = 15000;
            TcpClient client = new TcpClient(IP_sendTo.ToString(), port);
            NetworkStream stream = client.GetStream();

            Mandafacile.progresso++;

            byte[] byteBuffer;
            string stringBuffer;
            WaitHandle[] handles = new WaitHandle[2];
            handles[0] = terminateSend;
            handles[1] = doSend;
            FileInfo fileInfo;

            Mandafacile.progresso++;

            if (path == null)
            {
                path = "text.txt";
            }
            fileInfo = new FileInfo(path);

            Mandafacile.progresso++;

            if (fileInfo.Exists)
            {
                Mandafacile.progresso++;
                using (FileStream fileStream = fileInfo.OpenRead())
                {
                    Mandafacile.progresso++;
                    // --> "R<File_Name_Len><File_Name><File_Length>"
                    // R
                    stream.Write(Encoding.ASCII.GetBytes(RECV_FILE), 0, 1);
                    Mandafacile.progresso += 5;
                    // <File_Name_Len>
                    byteBuffer = BitConverter.GetBytes(fileInfo.Name.Length);
                    stream.Write(byteBuffer, 0, byteBuffer.Length);
                    Mandafacile.progresso += 5;
                    // <File_Name>
                    byteBuffer = Encoding.ASCII.GetBytes(fileInfo.Name);
                    stream.Write(byteBuffer, 0, byteBuffer.Length);
                    Mandafacile.progresso += 5;
                    // <File_Len>
                    byteBuffer = BitConverter.GetBytes((Int32)fileInfo.Length);
                    stream.Write(byteBuffer, 0, byteBuffer.Length);
                    Mandafacile.progresso += 5;

                    // richiesta accettata ?
                    byteBuffer = new byte[6];
                    stream.Read(byteBuffer, 0, _STRING_NO_LEN_);
                    stringBuffer = Encoding.ASCII.GetString(byteBuffer);
                    if (stringBuffer.Contains(_STRING_NO_))
                    {
                        MessageBox.Show("file rifiutato");
                    }
                    else if(stringBuffer.Contains(_STRING_OK_)){
                        MessageBox.Show("richiesta di invio accettata, inizio trasferimento");
                        // --> <File_Content>
                        Int32 nRead = -1;
                        byteBuffer = new Byte[1024];
                        while ((WaitHandle.WaitAny(handles) == 1) && (nRead = fileStream.Read(byteBuffer, 0, byteBuffer.Length)) > 0) // solo doSend è segnalato
                        {
                            stream.Write(byteBuffer, 0, nRead);
                        }
                        Mandafacile.progresso += 10;
                        // --> "--FINE--"
                        if (nRead == 0) // file inviato con successo
                        {
                            Console.WriteLine("trasferimento file completato");
                            byteBuffer = Encoding.ASCII.GetBytes(_STRING_END_);
                            stream.Write(byteBuffer, 0, _STRING_END_LEN_);
                        }
                        else // invio file fallito
                        {
                            Console.WriteLine("trasferimento fallito");
                            byteBuffer = Encoding.ASCII.GetBytes(_STRING_ERR_);
                            stream.Write(byteBuffer, 0, _STRING_ERR_LEN_);
                        }
                    }
                    else
                    {
                        MessageBox.Show("errore di protocollo");
                    }
                    Mandafacile.progresso++;
                }
            }
            
            stream.Close();
            client.Close();
            Mandafacile.progresso++;
        }

        public Thread Run()
        {
            Mandafacile.progresso++;

            Thread th = new Thread(Send);
            Mandafacile.progresso++;

            th.IsBackground = false;
            th.Name = "SendFile";
            th.Start();
            Mandafacile.progresso++;
            return th;
        }
    }
}
