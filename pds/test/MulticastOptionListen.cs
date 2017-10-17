using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

/*
 * Thread in background 
 * Socket UDP sempre in ascolto sul indirizzo multicast 224.168.100.2 e posta 11000
 */
namespace test
{
    /* classe con socket udp in ascolto su indirizzo multicast = 224.168.100.2 e porta = 11000 */
    public class MulticastOptionListen
    {
        private static Socket mcastSocket;
        private static MulticastOption mcastOption;
        static private string path_fotoProfilo = @"C:\Users\" + Environment.UserName + @"\Documents\Mandafacile\FotoProfilo";
        private static bool stop = false;
        private Mandafacile mf;

        public MulticastOptionListen(Mandafacile mf)
        {
            this.mf = mf;
        }

        private void StartMulticast()
        {
            try
            {
                mcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPAddress localIPAddr = IPAddress.Any;
                EndPoint localEP = (EndPoint)new IPEndPoint(localIPAddr, Networking.mcastPort);
                mcastSocket.Bind(localEP);
                mcastOption = new MulticastOption(Networking.mcastAddress);
                mcastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mcastOption);
                //mcastSocket.MulticastLoopback = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveBroadcastMessages()
        {
            IPEndPoint groupEP = new IPEndPoint(Networking.mcastAddress, Networking.mcastPort);
            EndPoint remoteEP = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
            try
            {
                while (!stop)
                {
                    byte[] bytes = new Byte[Networking.UDP_limit];
                    Console.WriteLine("Waiting for multicast packets...");
                    mcastSocket.ReceiveFrom(bytes, ref remoteEP);
                    string stringBuffer = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    if (stringBuffer.Contains("--WHO-IS-HERE--"))
                    {
                        if (Properties.Settings.Default.pubblico)
                        {
                            MulticastOptionSend.Run(MulticastOptionSend.MsgType.IAmHere);
                        }
                    }
                    else
                    {
                        User newUser = JsonConvert.DeserializeObject<User>(stringBuffer);
                        newUser.set_immagine(path_fotoProfilo + @"\" + newUser.get_immagine());
                        lock (mf.users)
                        {
                            mf.users.Add(newUser);
                        }
                        // salvare la foto profilo nella cartella designata
                        bytes = Convert.FromBase64String(newUser.get_immagineBase64());
                        using (FileStream image = new FileStream(newUser.get_immagine(), FileMode.Create))
                        {
                            image.Write(bytes, 0, bytes.Length);
                        }
                        mf.Invoke(mf.updateUserDelegate);
                    }
                }
                mcastSocket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void Stop(Object s, System.Timers.ElapsedEventArgs e)
        {
            stop = true;
        }

        public void Listen()
        {
            StartMulticast();
            ReceiveBroadcastMessages();
        }

        public void Run()
        {
            Thread thread = new Thread(Listen);
            thread.IsBackground = true;
            thread.Name = "MulticastOptionListen";
            thread.Start();
        }
    }
}