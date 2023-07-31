
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DataManager;
using SQLDataSources;

namespace Archivarius
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
      
        [STAThread]
        static void Main()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            /*  Console.WriteLine($"Start App");
              logger.Trace($"Trace Log");
              logger.Debug($"Debug Log");
              logger.Info($"Info Log");
              logger.Warn($"Warn Log");
              logger.Error($"Error Log");
              logger.Fatal($"Fatal Log");
              Console.ReadLine();
            */
            NLogger.logger.Trace($"Service. Programm has started");
            if (Config.Sets.Running) Config.Start();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            NLogger.logger.Trace($"Service. formMain has called");
            Application.Run(new formMain());
               
        }
    }
}
