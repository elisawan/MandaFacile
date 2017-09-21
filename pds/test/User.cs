using System;
using System.Collections.Generic;
using System.Drawing;
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
        private String ImmagineUtente;
        private int TCPPort;

        public User(String nomeUtente, String IpAddress, String PercorsoImmagine, int TCPPort) {
            this.nomeUtente = nomeUtente;
            this.IpAddress = IpAddress;
            this.ImmagineUtente = PercorsoImmagine;
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

        public String get_immagine()
        {
            return this.ImmagineUtente;
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
