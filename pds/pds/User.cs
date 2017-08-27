using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace pds
{
   
    public class User 
    {   
        
        public string username { get; set; }
        public string ip_address { get; set; }
        public int port { get; set; }
        //public int foto_utente { get; set; } //è solo un segnaposto, bisogna capire come mettere la foto nel profilo utente
        public Image foto_utente { get; set; }
    }
}
