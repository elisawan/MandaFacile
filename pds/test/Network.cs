using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;


namespace test
{
    class Network
    {
        /*
         * Send "Hello!" message to everyone in the multicast group
         */
        static public void Hello()
        {
            Debug.WriteLine("inside Hello()");

            IPAddress mcastAddr;
            int mcastPort;
            Socket mcastSocket;
            IPEndPoint mcastEndP;
            MulticastOption mcastOpt;

            IPAddress localAddr;           
            IPEndPoint localEndP;      
           
            byte[] msg = new byte[100];

            mcastAddr = IPAddress.Parse("224.168.100.3");
            mcastPort = 11000;
            mcastEndP = new IPEndPoint(mcastAddr, mcastPort);
            mcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            localAddr = IPAddress.Parse("192.168.1.98");
            localEndP = new IPEndPoint(localAddr, 0);

            //mcastSocket.Bind(localEndP);

            mcastOpt = new MulticastOption(mcastAddr);
            mcastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mcastOpt);

            mcastSocket.SendTo(ASCIIEncoding.ASCII.GetBytes("Hello!"), mcastEndP);

            EndPoint remoteEndP = new IPEndPoint(IPAddress.Any, 0);
            mcastSocket.ReceiveFrom(msg, ref remoteEndP);
            Console.WriteLine(Encoding.ASCII.GetString(msg, 0, msg.Length));

            mcastSocket.Close();

            return;
        }
            
        
        
    }
}
