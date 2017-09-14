using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class User
    {
        private String nomeUtente;
        private String IpAddress;
        //private foto

        public User(String nomeUtente, String IpAddress) {
            this.nomeUtente = nomeUtente;
            this.IpAddress = IpAddress;
        }

        public String get_username()
        {
            return this.nomeUtente;
        }

        public String get_address()
        {
            return this.IpAddress;
        }
    }
}
