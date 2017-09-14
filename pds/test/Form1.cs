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

        public Mandafacile()
        {
            InitializeComponent();

            Message message = new Message();
            message.Start();

            //riempie la lista -> da inserire in un thread?
            fillListView();
            //gestisce l'icona nella barra delle notifiche
            set_notifyIconMenu();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

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

        //Metodo che viene invocato cliccando sul tasto menuitem1
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

        public void fillListView()
        {
            //ListView listView1 = listvi
            //listView1.Bounds = new Rectangle(new Point(10, 10), new Size(300, 200));

            // Set the view to show details.
            listView1.View = View.Details;
            // Allow the user to edit item text.
            listView1.LabelEdit = true;
            // Allow the user to rearrange columns.
            listView1.AllowColumnReorder = true;
            // Display check boxes.
            listView1.CheckBoxes = true;
            // Select the item and subitems when selection is made.
            listView1.FullRowSelect = true;
            // Display grid lines.
            listView1.GridLines = true;
            // Sort the items in the list in ascending order.
            listView1.Sorting = SortOrder.Ascending;

            // Create three items and three sets of subitems for each item.
            ListViewItem item1 = new ListViewItem("Pinuccio", 0);
            // Place a check mark next to the item.
            item1.Checked = false;
            item1.SubItems.Add("80901");
            item1.SubItems.Add("192.168.1.2");
            //item1.SubItems.Add("3");
            ListViewItem item2 = new ListViewItem("Margherita", 1);
            item2.Checked = false;
            item2.SubItems.Add("80901");
            item2.SubItems.Add("192.168.1.15");
            //item2.SubItems.Add("6");
            ListViewItem item3 = new ListViewItem("Federico", 0);
            // Place a check mark next to the item.
            item3.Checked = false;
            item3.SubItems.Add("80901");
            item3.SubItems.Add("192.168.1.26");
            //item3.SubItems.Add("9");

            // Create columns for the items and subitems.
            listView1.Columns.Add("Utente", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Porta", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Indirizzo IP", -2, HorizontalAlignment.Left);
            //listView1.Columns.Add("Column 4", -2, HorizontalAlignment.Center);
            
            //Add the items to the ListView.
            listView1.Items.AddRange(new ListViewItem[] { item1, item2, item3 });

            // Create two ImageList objects.
            ImageList imageListSmall = new ImageList();
            ImageList imageListLarge = new ImageList();

            // Initialize the ImageList objects with bitmaps.
            imageListSmall.Images.Add(Bitmap.FromFile("don.jpg"));
            imageListSmall.Images.Add(Bitmap.FromFile("don.jpg"));
            imageListLarge.Images.Add(Bitmap.FromFile("don.jpg"));
            imageListLarge.Images.Add(Bitmap.FromFile("don.jpg"));

            //Assign the ImageList objects to the ListView.
            listView1.LargeImageList = imageListLarge;
            listView1.SmallImageList = imageListSmall;

            // Add the ListView to the control collection.
            this.Controls.Add(listView1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //bottone invia
        private void button1_Click(object sender, EventArgs e)
        {

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
        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
        }
    }
}
