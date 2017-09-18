using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace test
{
    class TestMulticastOptionSend
    {
        static IPAddress mcastAddress;
        static int mcastPort;
        static Socket mcastSocket;

        static void JoinMulticastGroup()
        {
            try
            {
                mcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPAddress localIPAddr = IPAddress.Any;      
                IPEndPoint IPlocal = new IPEndPoint(localIPAddr, 0);
                mcastSocket.Bind(IPlocal);

                MulticastOption mcastOption;
                mcastOption = new MulticastOption(mcastAddress, localIPAddr);
                mcastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mcastOption);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.ToString());
            }
        }

        static void BroadcastMessage(string message)
        {
            IPEndPoint endPoint;
            try
            {
                endPoint = new IPEndPoint(mcastAddress, mcastPort);
                mcastSocket.SendTo(ASCIIEncoding.ASCII.GetBytes(message), endPoint);
                Console.WriteLine("Multicast data sent.....");
<<<<<<< HEAD

                // IPEndPoint myEnd = new IPEndPoint(IPAddress.Parse("192.168.1.99"), mcastPort);
                // mcastSocket.SendTo(ASCIIEncoding.ASCII.GetBytes("my message"), myEnd);
=======
>>>>>>> elisa
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.ToString());
            }
            mcastSocket.Close();
        }
  
        public static void Run()
        {           
            mcastAddress = IPAddress.Parse("224.168.100.2");
            mcastPort = 11000;

            JoinMulticastGroup();

            BroadcastMessage("Hello multicast listener.");
        }        
    }
}