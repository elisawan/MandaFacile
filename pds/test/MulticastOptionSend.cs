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
    public class MulticastOptionSend
    {
        static Socket mcastSocket;
        private Mandafacile mf;

        public MulticastOptionSend(Mandafacile mf)
        {
            this.mf = mf;
        }

        public enum MsgType
        {
            whoIsHere,
            IAmHere,
        };

        public void JoinMulticastGroup()
        {
            try
            {
                mcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPAddress localIPAddr = IPAddress.Any;
                IPEndPoint IPlocal = new IPEndPoint(localIPAddr, 0);
                mcastSocket.Bind(IPlocal);
                MulticastOption mcastOption;
                mcastOption = new MulticastOption(Networking.mcastAddress, localIPAddr);
                mcastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mcastOption);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                mf.Invoke(mf.fatalError, "Errore di connessione alla rete");
            }
        }

        public void BroadcastMessage(string message)
        {
            IPEndPoint endPoint;
            try
            {
                endPoint = new IPEndPoint(Networking.mcastAddress, Networking.mcastPort);
                mcastSocket.SendTo(ASCIIEncoding.ASCII.GetBytes(message), endPoint);
                Console.WriteLine("Multicast data sent.....");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                mcastSocket.Close();
                mf.Invoke(mf.fatalError, "Errore di connessione alla rete");
            }
            mcastSocket.Close();
        }

        public void Run(MsgType type)
        {
            Console.Write("MulticastOptionSend.Run");
            string s = null;
            if (type == MsgType.IAmHere)
            {
                string userName = null;
                string fotoProfilo = null;
                if ((userName = Properties.Settings.Default.UserName) == null)
                {
                    userName = System.Environment.UserName;
                }
                if ((fotoProfilo = Properties.Settings.Default.FotoProfilo) == null)
                {
                    fotoProfilo = "default.png";
                }
                User me = new User(userName, null, fotoProfilo, Path.GetFileName(fotoProfilo), null);
                s = me.Serialize();
            }
            else if (type == MsgType.whoIsHere)
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