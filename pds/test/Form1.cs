using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Mandafacile : Form
    {
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItemPubblicoPrivato;
        private bool pubblico=false;
        private string nomeFile = null;
        private System.Windows.Forms.Timer time = new System.Windows.Forms.Timer();
        private List<Thread> threads_sendFile = new List<Thread>();
        private List<SendFile> list_sendFiles = new List<SendFile>(); 


        //Costruttore che riceve il nome del file 
        public Mandafacile(string filename)
        {
            MessageBox.Show(filename);
            nomeFile = filename;

            InitializeComponent();
            initializeListView();
            //riempie la lista -> da inserire in un thread?
            fillListView();
            //gestisce l'icona nella barra delle notifiche
            set_notifyIconMenu();

            
        }

        //Costruttore senza parametro ricevuto
        public Mandafacile()
        {
            InitializeComponent();
            initializeListView();
            //riempie la lista -> da inserire in un thread?
            fillListView();
            //gestisce l'icona nella barra delle notifiche
            set_notifyIconMenu();
            

        }

        //MENU' CONTESTUALE ICONA DI NOTIFICA -> Questi metodi gestiscono l'icona di notifica e le sue funzioni
        public void set_notifyIconMenu()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemPubblicoPrivato = new System.Windows.Forms.MenuItem();

            // Initialize contextMenu1
            this.contextMenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.menuItem1, this.menuItemPubblicoPrivato });

            // Initialize menuItem1
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "Exit";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);

            //inizializzo il secondo tasto
            this.menuItemPubblicoPrivato.Index = 0;
            this.menuItemPubblicoPrivato.Text = "Pubblico/Privato";
            this.menuItemPubblicoPrivato.Click += new System.EventHandler(this.menuItemPubblicoPrivato_Click);

            notifyIcon1.ContextMenu = this.contextMenu1;
            // Handle the DoubleClick event to activate the form.
            notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
        }

        private void menuItemPubblicoPrivato_Click(object sender, EventArgs e)
        {
            if (!pubblico)
            {
                MessageBox.Show("Profilo impostato come pubblico");
                pubblico = true;
            }
            else {
                pubblico = false;
                MessageBox.Show("Profilo impostato come privato");
            }
        }

        //Metodo che viene invocato cliccando sul tasto menuitem1 (che sarebbe exit, valutare se eliminarlo)
        private void menuItem1_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            this.Close();
        }

        private void notifyIcon1_DoubleClick(object Sender, EventArgs e)
        {
            // Show the form when the user double clicks on the notify icon.

            // Set the WindowState to normal if the form is minimized.
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            // Activate the form.
            this.Activate();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
        //FINE ICONA DI NOTIFICA
        //#########################################################################

        public void initializeListView()
        {
            this.listView1.Columns.Add("Nome");
            this.listView1.Columns.Add("Indirizzo IP");
            this.listView1.View = View.Tile;
            this.listView1.TileSize = new Size(180, 50);
            this.listView1.MultiSelect = true;
            this.listView1.HideSelection = false;
        }

        //LISTA -> Questi metodi gestiscono la lista degli utenti visualizzati a schermo
        public void fillListView()
        {
            //ListView listView1 = listvi
            //listView1.Bounds = new Rectangle(new Point(10, 10), new Size(300, 200));

            User u1 = new User("Don", "127.0.0.1", "don.jpg", null);
            User u2 = new User("Pikachu", "127.0.0.1", "don.jpg", null);

            User[] Users = { u1, u2 };

            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(48, 48);
            
            listView1.LargeImageList = imageList;
            int i = 0;
            foreach (User u in Users)
            {   
                ListViewItem item = new ListViewItem(u.get_username(), i);
                
                item.SubItems.Add(u.get_address());
                imageList.Images.Add(Bitmap.FromFile(u.get_immagine()));
                this.listView1.Items.Add(item);
                
                i++;
            }
            this.Controls.Add(listView1);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        
        //FINE LISTA
        //#########################################################################



        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        //METODI THREAD -> Valutarne lo spostamento/cancellazione
        //bottone invia
        private void button1_Click(object sender, EventArgs e)
        {
            
            buttonStop.Enabled = true;
            progressBar1.Visible = true;
            progressBar1.Style = ProgressBarStyle.Continuous;

            // Set the interval for the timer.
            time.Interval = 100;
            // Connect the Tick event of the timer to its event handler.
            time.Tick += new EventHandler(IncreaseProgressBar);
            // Start the timer.
            time.Start();
            
            
            //Tramite questo foreach, ottieni tutti gli utenti che sono stati selezionati
            ListView.SelectedListViewItemCollection utenti = this.listView1.SelectedItems;
            foreach (ListViewItem item in utenti)
            {   
                MessageBox.Show(item.Text + "," +item.SubItems[1].Text);
                SendFile sf = new SendFile(item.SubItems[1].Text, nomeFile);
                list_sendFiles.Add(sf);
                threads_sendFile.Add(sf.Run());
            }
            /*
            //INIZIALIZZO THREAD 1 - RICERCA SULLA RETE
            // Create the thread object. This does not start the thread.
            SenderWorker workerObject = new SenderWorker();
            Thread workerThread = new Thread(workerObject.DoWork);

            // Start the worker thread.
            workerThread.Start();
            Console.WriteLine("main thread: Inizio thread invio...");
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
            Console.WriteLine("main thread: Invio terminato.");
            */
        }

        private void IncreaseProgressBar(object sender, EventArgs e)
        {
            // Increment the value of the ProgressBar a value of one each time.
            progressBar1.Increment(1);
            // Display the textual value of the ProgressBar in the StatusBar control's first panel.
            statusBar1.Text = progressBar1.Value.ToString() + "% Completed";
            // Determine if we have completed by comparing the value of the Value property to the Maximum value.
            if (progressBar1.Value == progressBar1.Maximum)
            {
                // Stop the timer.
                time.Stop();
                MessageBox.Show("Invio completato!");
                progressBar1.Value = 0;
                progressBar1.Visible = false;
                buttonStop.Enabled = false;
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            buttonStop.Enabled = false;
            //bisogna fare in modo che si chiami abort sul thread in sendfile
            foreach (SendFile sf in list_sendFiles)
            {
                sf.terminateSend.Set();
            }
            foreach(Thread th in threads_sendFile)
            {
                th.Join();
            }
        }

        //METODO OPZIONI PROFILO
        private void buttonOpzioniProfilo_Click(object sender, EventArgs e)
        {
            OpzioniUtenteForm f = new OpzioniUtenteForm();
            f.Show();
        }

    }
}
