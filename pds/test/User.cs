using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace test
{
    class User
    {
        [JsonProperty]
        private string nomeUtente;
        [JsonProperty]
        private string IpAddress;
        private string ImmagineUtente;
        [JsonProperty]
        private string immagineBase64;

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

        private void ImageToBase64()
        {
            if(this.ImmagineUtente != null)
            {
                Image image = Image.FromFile(this.ImmagineUtente);
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, image.RawFormat);
                    byte[] imageBytes = ms.ToArray();
                    this.immagineBase64 = Convert.ToBase64String(imageBytes);
                }
            }
        }

        public String Serialize()
        {
            ImageToBase64();
            String s = JsonConvert.SerializeObject(this, Formatting.Indented);
            Console.WriteLine(s);
            return s;
        }
    }

    class UserList : ObservableCollection<User>
    {
        UserList()
        {

        }
    }
}
