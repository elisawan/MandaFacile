using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Net;

namespace test
{
    class User
    {
        private String nomeUtente;
        private String IpAddress;
        private int TCPPort;
        //private foto

        public User(String nomeUtente, String IpAddress, int TCPPort) {
            this.nomeUtente = nomeUtente;
            this.IpAddress = IpAddress;
            this.TCPPort = TCPPort;
        }

        public String get_username()
        {
            return this.nomeUtente;
        }

        public String get_address()
        {
            return this.IpAddress;
        }

        public int get_TCPPort()
        {
            return this.TCPPort;
        }
    }

    class UserList : ObservableCollection<User>
    {
        UserList()
        {

        }
    }
}
