using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Isis4426.Proyecto1.ConversorBatch.Timer
{
    class Program
    {
        private static System.Timers.Timer timer;

        static void Main(string[] args)
        {
            try
            {
                timer = new System.Timers.Timer { Interval = 10000, AutoReset = true, Enabled = true };
                timer.Elapsed += timer_Elapsed;
                timer.Start();

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);                
            }
        }

        private static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();

            try
            {
                var url = "http://localhost:6089/convertirParalelo";
                var webrequest = (HttpWebRequest)WebRequest.Create(url);
                webrequest.GetResponse();                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                timer.Start();
            }
        }
    }
}
