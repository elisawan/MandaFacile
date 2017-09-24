using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class OpzioniUtenteForm : Form
    {
        public OpzioniUtenteForm()
        {
            InitializeComponent();
            caricaPref();

        }

        public void caricaPref()
        {
            if (Properties.Settings.Default.UserName != null)
                this.usernameTextBox.Text = Properties.Settings.Default.UserName;

            if (Properties.Settings.Default.FotoProfilo != null)
            {
                this.userPic.Image = Bitmap.FromFile(Properties.Settings.Default.FotoProfilo);
                this.userPic.SizeMode = PictureBoxSizeMode.Zoom;
            }

        }

        private void buttonScegliFoto_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                this.userPic.Image = Bitmap.FromFile(file);
                this.userPic.SizeMode = PictureBoxSizeMode.Zoom;
                Properties.Settings.Default.FotoProfilo = file;
                Properties.Settings.Default.Save();
            }
        }

        private void buttonSalvaModificheProfilo_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.UserName = this.usernameTextBox.Text;
            Properties.Settings.Default.Save();
            Close();
        }

        private void buttonSelezionaPath_Click(object sender, EventArgs e)
        {
            DialogResult result = selezionaPercorsoDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                String folderName = selezionaPercorsoDialog.SelectedPath;

                MessageBox.Show("Il percorso selezionato è: " + folderName);
                Properties.Settings.Default.Percorso = folderName;
                Properties.Settings.Default.Save();
            }
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }
    }
}
