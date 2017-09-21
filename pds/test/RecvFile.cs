using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace test
{
    class RecvFile
    {
        const int BUF_LEN = 1024;

        string fileName;
        IPEndPoint endp;
        

        RecvFile(string fileName, IPEndPoint endp)
        {
            this.fileName = fileName;
            this.endp = endp;
        }

        void Receive()
        {
            Byte[] byteBuffer = new Byte[BUF_LEN];
            string stringBuffer;
            TcpClient recvSocket = new TcpClient(endp);
            NetworkStream stream = recvSocket.GetStream();
            Byte[] data = Encoding.ASCII.GetBytes("Ready to receive file: ");
            Int32 nBytes;
            Int32 left;

            FileStream fs = File.Create(fileName);

            stream.Write(data, 0, data.Length);
            stream.Write(Encoding.ASCII.GetBytes(fileName), 0, fileName.Length);
            Console.WriteLine("Ready to receive...");

            Console.WriteLine("Waiting for file length....");
            nBytes = stream.Read(byteBuffer, 0, BUF_LEN);
           
            stringBuffer = Encoding.ASCII.GetString(byteBuffer);
            string[] values = stringBuffer.Split(':');
            if((values.Length == 2) && (values[0].Equals("File length")))
            {
                left = Int32.Parse(values[1]);
                while (left > 0)
                {
                    nBytes = stream.Read(byteBuffer, 0, BUF_LEN);
                    fs.Write(byteBuffer, 0, nBytes);
                    left -= nBytes;
                }
            }
        }
    }
}
