using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pds
{
    //TODO metodo per riempire la box list, bisogna sostituire i nomi giusti che l'ho preso da internet
    public partial class ListBoxDataBindingSample : Window
    {
        public ListBoxDataBindingSample()
        {
            InitializeComponent();
            List<TodoItem> items = new List<TodoItem>();
            items.Add(new TodoItem() { Title = "Complete this WPF tutorial", Completion = 45 });
            items.Add(new TodoItem() { Title = "Learn C#", Completion = 80 });
            items.Add(new TodoItem() { Title = "Wash the car", Completion = 0 });

            lbTodoList.ItemsSource = items;
        }
    }


    class User 
    {
        public string username { get; set; }
        public string ip_address { get; set; }
        public int port { get; set; }
        public int foto_utente { get; set; } //è solo un segnaposto, bisogna capire come mettere la foto nel profilo utente
    }
}
