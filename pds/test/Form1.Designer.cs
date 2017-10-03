namespace test
{
    partial class Mandafacile
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mandafacile));
            this.button1 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.buttonOpzioniProfilo = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.buttonStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(389, 264);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Invia";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(533, 246);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged_1);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipText = "Mandafacile";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Mandafacile";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // buttonOpzioniProfilo
            // 
            this.buttonOpzioniProfilo.Location = new System.Drawing.Point(13, 265);
            this.buttonOpzioniProfilo.Name = "buttonOpzioniProfilo";
            this.buttonOpzioniProfilo.Size = new System.Drawing.Size(75, 23);
            this.buttonOpzioniProfilo.TabIndex = 4;
            this.buttonOpzioniProfilo.Text = "Opzioni";
            this.buttonOpzioniProfilo.UseVisualStyleBackColor = true;
            this.buttonOpzioniProfilo.Click += new System.EventHandler(this.buttonOpzioniProfilo_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(125, 264);
            this.progressBar1.Maximum = 40;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(227, 25);
            this.progressBar1.TabIndex = 5;
            this.progressBar1.Visible = false;
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 296);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(553, 22);
            this.statusBar1.TabIndex = 6;
            this.statusBar1.Text = "...";
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(470, 264);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 7;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // Mandafacile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 318);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.buttonOpzioniProfilo);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Mandafacile";
            this.Text = "Mandafacile";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button buttonOpzioniProfilo;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.StatusBar statusBar1;
        private System.Windows.Forms.Button buttonStop;
    }
}

