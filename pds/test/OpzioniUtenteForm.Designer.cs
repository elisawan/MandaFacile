namespace test
{
    partial class OpzioniUtenteForm
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
            this.userPic = new System.Windows.Forms.PictureBox();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.buttonScegliFoto = new System.Windows.Forms.Button();
            this.ButtonAnnullaModificheProfilo = new System.Windows.Forms.Button();
            this.buttonSalvaModificheProfilo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSelezionaPath = new System.Windows.Forms.Button();
            this.selezionaPercorsoDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.selezionaFotoDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.userPic)).BeginInit();
            this.SuspendLayout();
            // 
            // userPic
            // 
            this.userPic.Location = new System.Drawing.Point(93, 12);
            this.userPic.Name = "userPic";
            this.userPic.Size = new System.Drawing.Size(154, 139);
            this.userPic.TabIndex = 0;
            this.userPic.TabStop = false;
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.usernameTextBox.Location = new System.Drawing.Point(108, 186);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(129, 20);
            this.usernameTextBox.TabIndex = 1;
            this.usernameTextBox.Text = "default_name";
            this.usernameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonScegliFoto
            // 
            this.buttonScegliFoto.Location = new System.Drawing.Point(132, 157);
            this.buttonScegliFoto.Name = "buttonScegliFoto";
            this.buttonScegliFoto.Size = new System.Drawing.Size(75, 23);
            this.buttonScegliFoto.TabIndex = 2;
            this.buttonScegliFoto.Text = "Scegli foto";
            this.buttonScegliFoto.UseVisualStyleBackColor = true;
            this.buttonScegliFoto.Click += new System.EventHandler(this.buttonScegliFoto_Click);
            // 
            // ButtonAnnullaModificheProfilo
            // 
            this.ButtonAnnullaModificheProfilo.Location = new System.Drawing.Point(21, 282);
            this.ButtonAnnullaModificheProfilo.Name = "ButtonAnnullaModificheProfilo";
            this.ButtonAnnullaModificheProfilo.Size = new System.Drawing.Size(75, 23);
            this.ButtonAnnullaModificheProfilo.TabIndex = 3;
            this.ButtonAnnullaModificheProfilo.Text = "Annulla";
            this.ButtonAnnullaModificheProfilo.UseVisualStyleBackColor = true;
            // 
            // buttonSalvaModificheProfilo
            // 
            this.buttonSalvaModificheProfilo.Location = new System.Drawing.Point(242, 282);
            this.buttonSalvaModificheProfilo.Name = "buttonSalvaModificheProfilo";
            this.buttonSalvaModificheProfilo.Size = new System.Drawing.Size(75, 23);
            this.buttonSalvaModificheProfilo.TabIndex = 4;
            this.buttonSalvaModificheProfilo.Text = "Salva";
            this.buttonSalvaModificheProfilo.UseVisualStyleBackColor = true;
            this.buttonSalvaModificheProfilo.Click += new System.EventHandler(this.buttonSalvaModificheProfilo_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 230);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(259, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Seleziona il percorso predefinito per memorizzare i file:";
            // 
            // buttonSelezionaPath
            // 
            this.buttonSelezionaPath.Location = new System.Drawing.Point(132, 246);
            this.buttonSelezionaPath.Name = "buttonSelezionaPath";
            this.buttonSelezionaPath.Size = new System.Drawing.Size(75, 23);
            this.buttonSelezionaPath.TabIndex = 6;
            this.buttonSelezionaPath.Text = "Seleziona";
            this.buttonSelezionaPath.UseVisualStyleBackColor = true;
            this.buttonSelezionaPath.Click += new System.EventHandler(this.buttonSelezionaPath_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // OpzioniUtenteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 317);
            this.Controls.Add(this.buttonSelezionaPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonSalvaModificheProfilo);
            this.Controls.Add(this.ButtonAnnullaModificheProfilo);
            this.Controls.Add(this.buttonScegliFoto);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.userPic);
            this.Name = "OpzioniUtenteForm";
            this.Text = "OpzioniUtenteForm";
            ((System.ComponentModel.ISupportInitialize)(this.userPic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox userPic;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Button buttonScegliFoto;
        private System.Windows.Forms.OpenFileDialog cercaFotoUtenteDialog;
        private System.Windows.Forms.Button ButtonAnnullaModificheProfilo;
        private System.Windows.Forms.Button buttonSalvaModificheProfilo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSelezionaPath;
        private System.Windows.Forms.FolderBrowserDialog selezionaPercorsoDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.FolderBrowserDialog selezionaFotoDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}