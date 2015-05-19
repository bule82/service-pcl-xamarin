using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ClassLibrary1
{
    public class Portable 
    {
        public int i = 0;
        public bool run = false;
        public Portable()
        {           
           

          
        }

        public void start(){


            CancellationTokenSource wtoken = new CancellationTokenSource();
            run = true;
            Task task = Task.Run(async () =>  // <- marked async
            {
                while (run)
                {
                    System.Diagnostics.Debug.WriteLine("ciao " + DateTime.Now.ToString());
                    i++;
                    await Task.Delay(10000, wtoken.Token); // <- await with cancellation
                }
            }, wtoken.Token);
        }

        public void stop()
        {
            run = false;
        }



     
    }
}
