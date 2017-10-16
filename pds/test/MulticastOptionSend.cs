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
        static Socket mcastSocket;

        public enum MsgType
        {
            whoIsHere,
            IAmHere,
        };

        static void JoinMulticastGroup()
        {
            try
            {
                mcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPAddress localIPAddr = IPAddress.Parse("172.21.56.130");     
                IPEndPoint IPlocal = new IPEndPoint(localIPAddr, 0);
                mcastSocket.Bind(IPlocal);

                MulticastOption mcastOption;
                mcastOption = new MulticastOption(Networking.mcastAddress, localIPAddr);
                mcastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mcastOption);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.ToString());
                mcastSocket.Close();
                return;
            }
        }

        static void BroadcastMessage(string message)
        {
            IPEndPoint endPoint;
            try
            {
                endPoint = new IPEndPoint(Networking.mcastAddress, Networking.mcastPort);
                //endPoint = new IPEndPoint(IPAddress.Parse("172.20.91.41"), mcastPort);
                mcastSocket.SendTo(ASCIIEncoding.ASCII.GetBytes(message), endPoint);
                Console.WriteLine("Multicast data sent.....");
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.ToString());
                mcastSocket.Close();
            }
            mcastSocket.Close();
        }
  
        public static void Run(MsgType type)
        {
            Console.Write("MulticastOptionSend.Run");
            string s;
            if(type == MsgType.IAmHere)
            {
                string userName = null;
                string fotoProfilo = null;
                if ((userName = Properties.Settings.Default.UserName) == null)
                {
                    userName = "io";
                }
                if ((fotoProfilo = Properties.Settings.Default.FotoProfilo) == null)
                {
                    fotoProfilo = "don.jpg";
                }
                User me = new User(userName, null, fotoProfilo, Path.GetFileName(fotoProfilo), null);
                s = me.Serialize();
            }
            else if(type == MsgType.whoIsHere)
            {
                s = "--WHO-IS-HERE--";
            }
            else
            {
                s = "";
            }
            JoinMulticastGroup();
            BroadcastMessage(s);
        }        
    }
}