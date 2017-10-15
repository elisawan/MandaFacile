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
     class Networking
    {
        // Multicast
        static public IPAddress mcastAddress = IPAddress.Parse("224.168.100.2");
        static public int mcastPort = 11000;
        static public int UDP_limit = 64 * 1024 * 8;

        // TCP
        static public int TCP_limit = 1500 * 8;
        static public Int32 TCP_port = 15000;
        static public string RECV_FILE = "R";
        static public string _STRING_END_ = "--FINE--";
        static public int _STRING_END_LEN_ = 8;
        static public string _STRING_ERR_ = "--ERROR--";
        static public int _STRING_ERR_LEN_ = 9;
        static public string _STRING_OK_ = "--OK--";
        static public int _STRING_OK_LEN_ = 6;
        static public string _STRING_NO_ = "--NO--";
        static public int _STRING_NO_LEN_ = 6;
    }

    class TCP : Networking
    {
        protected NetworkStream netStream;

        public bool ReadnNetStream(ref Byte[] buffer, int n)
        {
            int nLeft = n;
            int i = 0;
            int ret;
            try
            {
                while (nLeft > 0)
                {
                    ret = netStream.Read(buffer, i, nLeft);
                    i += ret;
                    nLeft -= ret;
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
    }
}
