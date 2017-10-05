using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;

/*
 * Classe per l'invio di uno o più pacchetti UDP all'indirizzo multicast 224.168.100.2 e porta 11000
 * 
 */
namespace test
{
    class MulticastOptionSend
    {
        static IPAddress mcastAddress = IPAddress.Parse("224.168.100.2");
        static int mcastPort = 11000;
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
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.ToString());
            }
            mcastSocket.Close();
        }
  
        public static void Run()
        {
            Console.Write("MulticastOptionSend.Run");
            string userName = null;
            string fotoProfilo = null;
            if ((userName = Properties.Settings.Default.UserName) == null)
            {
                userName = "io";
            }
            if((fotoProfilo = Properties.Settings.Default.FotoProfilo) == null)
            {
                fotoProfilo = "don.jpg";
            }
            User me = new User(userName, "127.0.0.1", Path.GetFileName(fotoProfilo), null);
            String s = me.Serialize();
            JoinMulticastGroup();
            BroadcastMessage(s);
        }        
    }
}