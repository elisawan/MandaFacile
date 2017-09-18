using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace test
{
    class User
    {
        private String nomeUtente;
        private String IpAddress;
        private String ImmagineUtente;

        public User(String nomeUtente, String IpAddress, String PercorsoImmagine) {
            this.nomeUtente = nomeUtente;
            this.IpAddress = IpAddress;
            this.ImmagineUtente = PercorsoImmagine;
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
    }

    class UserList : ObservableCollection<User>
    {
        UserList()
        {

        }
    }
}
