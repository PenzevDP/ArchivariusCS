
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using DataManager;
using SQLDataSources;
using TableDependency;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;
using TableDependency.SqlClient.Base.EventArgs;

namespace Archivarius
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
       

      
        [STAThread]
        static void Main(string[] args)
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
            NLogger.logger.Error($"Service. Programm has started");
            if (Config.Sets.Running)
            {
                NLogger.logger.Error("Config.Sets already running so void Config.Start() has called from MAIN()");
               Config.Start();
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            NLogger.logger.Error($"Service. formMain has called from MAIN()");
            Application.Run(new formMain());


        }
        
    }       
}
