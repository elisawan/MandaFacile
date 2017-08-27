using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace pds
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            butt_invia.Click += butt_invia_Click;

            System.Windows.Forms.NotifyIcon notify_icon = new System.Windows.Forms.NotifyIcon();
            notify_icon.Icon = new System.Drawing.Icon("send-file-xxl.ico");
            notify_icon.Visible = true;
            //notify_icon.ShowBalloonTip(500, "Mandafacile", "Avvio ricerca utenti online", System.Windows.Forms.ToolTipIcon.None);
            System.Windows.Forms.ContextMenu notify_context = new System.Windows.Forms.ContextMenu();
            notify_context.MenuItems.Add("In linea", new EventHandler(Disponibile));
            notify_context.MenuItems.Add("Invisibile", new EventHandler(Invisibile));
            notify_icon.ContextMenu = notify_context;

            //Questo metodo inserisce nella lista gli utenti presenti
            ListBoxDataBinding();
        }
        //METODI PER LA GESTIONE DEI CLICK SULL'ICONA NELLA BARRA DEGLI STRUMENTI
        private void Disponibile(object sender, EventArgs e)
        {
            MessageBox.Show("Profilo: Disponibile");
        }
        private void Invisibile(object sender, EventArgs e)
        {
            MessageBox.Show("Profilo: Invisibile");

        }

        private void notify_icon_Click(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if(e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MessageBox.Show("Click! :)");
            }
        }

        private void butt_invia_Click(object sender, RoutedEventArgs e)
        {


            //INIZIALIZZO THREAD 1 - RICERCA SULLA RETE
            // Create the thread object. This does not start the thread.
            SenderWorker workerObject = new SenderWorker();
            Thread workerThread = new Thread(workerObject.DoWork);

            // Start the worker thread.
            workerThread.Start();
            Console.WriteLine("main thread: Inizio thread ricerca su rete...");
            // Loop until worker thread activates.
            while (!workerThread.IsAlive) ;

            // Put the main thread to sleep for 1 millisecond to
            // allow the worker thread to do some work:
            Thread.Sleep(1);

            // Request that the worker thread stop itself:
            workerObject.RequestStop();

            // Use the Join method to block the current thread 
            // until the object's thread terminates.
            workerThread.Join();
            Console.WriteLine("main thread: Ricerca sulla rete terminata.");
        
        
        }

        //TODO metodo per riempire la box list, bisogna sostituire i nomi giusti che l'ho preso da internet
        
        public void ListBoxDataBinding()
        {
            List<User> items = new List<User>();
            User u = new User();
            u.username = "Roberto";
            u.ip_address = "192.168.1.2";
            u.port = 2212;

            //
            Image img = new Image();
            Uri ur = new Uri("laura.jpg", UriKind.RelativeOrAbsolute);

            ImageSource isu = new BitmapImage(ur);
            img.SetValue(Image.SourceProperty, isu);
            //
            u.foto_utente = img;
            items.Add(u);
           
            items.Add(new User() {username = "Carolina",ip_address = "192.168.1.15", port = 4445, foto_utente =img });

            listBox.ItemsSource = items;
            listBox.BringIntoView();
        }
        
    }
}
