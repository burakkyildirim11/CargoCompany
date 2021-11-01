using CargoCompany.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CargoCompany
{
    static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Thread th2 = new Thread(new ThreadStart(MapsForm));
            //Thread th1 = new Thread(new ThreadStart(SingIn));

            //th1.Start();
            //th2.Start();

            //Application.Run(new MapsForm());
            Application.Run(new SingIn());

        }


        //private static void MapsForm()
        //{
        //    Application.Run(new MapsForm());
        //}

        //private static void SingIn()
        //{
        //    Application.Run(new SingIn());

        //}
    }
}


