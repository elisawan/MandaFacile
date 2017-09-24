using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string [] args)
        {
            if (args.Length > 0)
            {
                //il programma è stato avviato cliccando col destro su un file da inviare
                string filepath = args[0];
                if (File.Exists(filepath))
                {
                    FileInfo fi = new FileInfo(filepath);
                    
                    MessageBox.Show("File scelto per l'invio: " + fi.Name + "\nDimensione: " + fi.Length + " bytes");
                    
                    //MessageBox.Show(filename);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Mandafacile(filepath));
                }
            }else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Mandafacile());
            }
            

            
        }

        
    }
}
