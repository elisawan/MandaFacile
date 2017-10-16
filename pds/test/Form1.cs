using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        private string nomeFile = null;
        private System.Windows.Forms.Timer time = new System.Windows.Forms.Timer();
        private List<Thread> threads_sendFile = new List<Thread>();
        private List<SendFile> list_sendFiles = new List<SendFile>();
        public static int progresso;
        public delegate void UpdateUser();
        public UpdateUser updateUserDelegate;
        public List<User> users = new List<User>();

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


            
            //toolTip1.SetToolTip(this.updateButton, "Aggiorna lista utenti in rete");
        }

        //Costruttore senza parametro ricevuto
        public Mandafacile()
        {
            //in ascolto, per ricevere i pacchetti di presentazione degli altri
            MulticastOptionListen ml = new MulticastOptionListen(this);
            ml.Run();
            if (Properties.Settings.Default.pubblico)
            {
                Listen.Start(); //in modalità privata non posso ricevere file
            }
            //chiedo chi altri è in linea ?
            MulticastOptionSend.Run(MulticastOptionSend.MsgType.whoIsHere);

            InitializeComponent();
            initializeListView();
            //riempie la lista -> da inserire in un thread?
            fillListView();
            //gestisce l'icona nella barra delle notifiche
            set_notifyIconMenu();
            updateUserDelegate = new UpdateUser(fillListView);
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
            if (!Properties.Settings.Default.pubblico)
            {
                MessageBox.Show("Profilo impostato come pubblico");
                Properties.Settings.Default.pubblico = true;
                Listen.Start();
            }
            else {
                Properties.Settings.Default.pubblico = false;
                MessageBox.Show("Profilo impostato come privato");
                Listen.Stop();
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

            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(48, 48);
            
            listView1.LargeImageList = imageList;
            int i = 0;
            lock (users)
            {
                foreach (User u in users)
                {
                    ListViewItem item = new ListViewItem(u.get_username(), i);
                    imageList.Images.Add(Bitmap.FromFile(u.get_immagine()));
                    this.listView1.Items.Add(item);
                    MessageBox.Show(u.get_address() + u.get_username());
                    i++;
                }
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
            //Controllo se file=null
            if (nomeFile == null)
            {
                DialogResult scelta = MessageBox.Show("Vuoi mandare un singolo file? Premi OK.\nPremi No se vuoi inviare una cartella intera, Cancella altrimenti", "Scelta file", MessageBoxButtons.YesNoCancel);

                if (scelta == DialogResult.Yes)
                {
                    DialogResult result = openFileDialog1.ShowDialog();
                    if (result == DialogResult.OK)
                        nomeFile = openFileDialog1.FileName;
                }
                else if(scelta == DialogResult.No)
                {
                    DialogResult result = folderBrowserDialog1.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        string percorso = folderBrowserDialog1.SelectedPath;
                        string nomeFolder = Path.GetFileName(percorso);
                        ZipFile zip = new ZipFile();
                        zip.AddDirectory(percorso, nomeFolder);
                        zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");
                        nomeFile = @"C:\Users\" + Environment.UserName + @"\Documents\Mandafacile\tmp_s\" + nomeFolder+".zip";
                        zip.Save(nomeFile);
                    }
                }else
                    return;
            }

            progresso = 0;
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
            int i = 0;
            foreach (ListViewItem item in utenti)
            {   
                User u = users.ElementAt(i);
                SendFile sf = new SendFile(u.get_address(), nomeFile);
                list_sendFiles.Add(sf);
                threads_sendFile.Add(sf.Run());
                i++;
            }
            nomeFile = null;
        }
        /// <summary>
        /// 
        /// Incrementa la barra di progresso durante l'invio di un file.
        /// 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        public void IncreaseProgressBar(object sender, EventArgs e)
        {

            if (progresso == -1)
            {
                // Stop the timer.
                time.Stop();
                MessageBox.Show("Richiesta d'invio non accettata");
                progressBar1.Value = 0;
                progressBar1.Visible = false;
                statusBar1.Text = "...";
                buttonStop.Enabled = false;
            }else
            {
                // Increment the value of the ProgressBar a value of one each time.
                progressBar1.Increment(progresso - progressBar1.Value);
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
                    statusBar1.Text = "...";
                    buttonStop.Enabled = false;
                }
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            statusBar1.Text = "...";
            buttonStop.Enabled = false;
            progressBar1.Value = 0;
            progressBar1.Visible = false;
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

        private void updateButton_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            lock (users)
            {
                users = null;
                users = new List<User>();
            }
            MulticastOptionSend.Run(MulticastOptionSend.MsgType.whoIsHere);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            notifyIcon1.Visible = false;
            notifyIcon1.Icon = null;
            notifyIcon1.Dispose();
            Application.DoEvents();
        }
    }
}
