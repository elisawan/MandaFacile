using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
//using Newtonsoft.Json;
using System.IO;

/*
 * Thread in background 
 * Socket UDP sempre in ascolto sul indirizzo multicast 224.168.100.2 e posta 11000
 */
namespace test
{
    public class MulticastOptionListen
    {
        private static IPAddress mcastAddress = IPAddress.Parse("224.168.100.2");
        private static int mcastPort = 11000;
        private static Socket mcastSocket;
        private static MulticastOption mcastOption;
        const int UDP_limit = 64 * 1024;
        static private string path_fotoProfilo ="FotoProfilo";

        private static void StartMulticast()
        {
            try
            {
                mcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPAddress localIPAddr = IPAddress.Any;
                EndPoint localEP = (EndPoint)new IPEndPoint(localIPAddr, mcastPort);

                mcastSocket.Bind(localEP);

                mcastOption = new MulticastOption(mcastAddress);

                mcastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mcastOption);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ReceiveBroadcastMessages()
        {
            bool done = false;
            byte[] bytes = new Byte[UDP_limit];
            IPEndPoint groupEP = new IPEndPoint(mcastAddress, mcastPort);
            EndPoint remoteEP = (EndPoint)new IPEndPoint(IPAddress.Any, 0);

            try
            {
                while (!done)
                {
                    Console.WriteLine("Waiting for multicast packets...");
                    mcastSocket.ReceiveFrom(bytes, ref remoteEP);
                    string stringBuffer = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    //User newUser = JsonConvert.DeserializeObject<User>(stringBuffer);
                    User newUser = null;
                    Console.WriteLine("NEW");
                    Console.WriteLine(newUser.get_address());
                    Console.WriteLine(newUser.get_username());
                    Console.WriteLine(newUser.get_immagine());
                    // salvare la foto profilo nella cartella designata
                    bytes = Convert.FromBase64String(newUser.get_immagineBase64());
                    using (FileStream image = new FileStream(path_fotoProfilo + @"\" + newUser.get_username() + ".jpg", FileMode.Create))
                    {
                        image.Write(bytes, 0, bytes.Length);
                        image.Flush();
                    }
                }
                mcastSocket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void Listen()
        {
            StartMulticast();
            ReceiveBroadcastMessages();
        }

        public static void Run()
        {
            Thread thread = new Thread(Listen);
            thread.IsBackground = true;
            thread.Start();
        }
    }
}