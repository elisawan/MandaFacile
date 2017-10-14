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
using System.Net.Sockets;

namespace test
{
    public class User
    {
        [JsonProperty]
        private string nomeUtente;
        [JsonProperty]
        private string IpAddress;
        [JsonProperty]
        private string ImmagineUtente;
        private string immaginePath;
        [JsonProperty]
        private string immagineBase64;

        public User(string nomeUtente, string IpAddress, string PercorsoImmagine, string nomeImmagine, string immagineBase64) {
            this.nomeUtente = nomeUtente;
            if(IpAddress == null)
            {
                this.IpAddress = FindLocalIP();
            }
            else
            {
                this.IpAddress = IpAddress;
            }
            this.immaginePath = PercorsoImmagine;
            this.ImmagineUtente = nomeImmagine;
            this.immagineBase64 = immagineBase64;
            
        }

        private string FindLocalIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork) // IPv4
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        public void set_immagine(string path)
        {
            this.ImmagineUtente = path;
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

        public String get_immagineBase64()
        {
            return this.immagineBase64;
        }

        private void ImageToBase64()
        {
            if(this.ImmagineUtente != null)
            {
                Image image = Image.FromFile(this.immaginePath);
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
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    class UserList : ObservableCollection<User>
    {
        UserList()
        {

        }
    }
}
