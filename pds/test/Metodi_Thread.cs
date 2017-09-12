using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace test
{
    class Metodi_Thread
    {

    }

    //TODO Questa classe servirà ad implementare l'invio di un file sulla rete
    public class SenderWorker
    {
        // This method will be called when the thread is started.
        public void DoWork()
        {
            /*while (!_shouldStop)
            {
                Console.WriteLine("worker thread: Sto inviando un file...");
            }*/
            MessageBox.Show("Ciao Eli");
            //this.Close();
            Console.WriteLine("worker thread: terminating gracefully.");
        }
        public void RequestStop()
        {
            _shouldStop = true;
        }
        // Volatile is used as hint to the compiler that this data
        // member will be accessed by multiple threads.
        private volatile bool _shouldStop;
    }
}
