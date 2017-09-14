using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace test
{
    class Message
    {
        const int BUF_LEN = 1024;
        IPAddress mcastAddr;
        int mcastPort;
        IPAddress localAddr;
        Socket mcastSocket;
        MulticastOption mcastOption;
        byte[] buffer = new byte[BUF_LEN];
        EndPoint endP;
        EndPoint localEnd;
        IPEndPoint mcastEndP;
        Boolean listening;

        /*
         * Message() 
         * inizialization with creation of mcastSocket
         */
        public Message()
        {
            mcastAddr = IPAddress.Parse("224.168.100.3");
            mcastPort = 11000;
            mcastEndP = new IPEndPoint(mcastAddr, mcastPort);
            mcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            

            localAddr = IPAddress.Parse("172.22.52.18");
            localEnd = (EndPoint)new IPEndPoint(IPAddress.Any, mcastPort);
            mcastSocket.Bind(localEnd); // bind to local endpoint

            mcastOption = new MulticastOption(mcastAddr);
            mcastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mcastOption); // add multicast address to mcastSocket
            mcastSocket.MulticastLoopback = false;
            Debug.WriteLine("multicast group: " + mcastOption.Group);
            Debug.WriteLine("multicast local address: " + mcastOption.LocalAddress);
        }

        ~Message()
        {
            Debug.WriteLine("Message: destructor");
            mcastSocket.Close();
        }

        /*
         * Start()
         * crea un nuovo thread che esegue la funzione ListenEveryone in background
         */
        public void Start()
        {
            var th = new Thread(ListenEveryone);
            th.IsBackground = true;
            th.Start();
        }

        /*
         * Stop()
         * ferma l'esecuzione del thread in ascolto sul socket multicast
         */
        public void Stop()
        {
            // sezione critica
            listening = false;
            // fine sezione critica
        }

        /*
         *  HelloEveryone()
         *  manda un pacchetto UDP multicast 
         *  messaggio: "Hello"
         */
        public void HelloEveryone()
        {
            Debug.WriteLine("Discover: start");
            endP = new IPEndPoint(IPAddress.Any, 0);
            mcastSocket.SendTo(ASCIIEncoding.ASCII.GetBytes("MandaFacile: Bella socio!"), mcastEndP);

            Debug.WriteLine("Discover: end");
        }

        /*
         * ListenEveryone()
         * loop infinito per la ricezione di pacchetti UDP sull'indirizzo multicast
         * e l'invio del pacchetto di presentazione ogni volta che arriva un nuovo utente
         */
        public void ListenEveryone()
        {
            // sezione critica
            listening = true;
            // fine sezione critica

            this.HelloEveryone();
            while (listening)
            {
                endP = new IPEndPoint(IPAddress.Any, 0);
                Debug.WriteLine(endP.ToString());
                mcastSocket.ReceiveFrom(buffer, ref endP);
                MessageBox.Show("Ciao Eli");

                //if (!endP.ToString().Equals("192.168.43.92"))
                //{
                Debug.WriteLine(endP.ToString());
                    Debug.WriteLine(Encoding.ASCII.GetString(buffer, 0, buffer.Length));

                    // TO DO: inviare le informazione del nuovo utente tramite qualche sorta di 
                    // canale di comunicazione al thread MainWindow

                    // inviare il proprio pacchetto di presentazione in multicast
                    // per farti conoscere dal nuovo utente 
                    // e per aggiornare le info che gli altri utenti possiedono
                   this.HelloEveryone();
                //}

                endP = null;
            }
        }
        /*
                public void DecodeMessage(ref string msg)
                {
                    msg.Split('\n')
                    if ()
                    {

                    }
                }
                */
    }
}
