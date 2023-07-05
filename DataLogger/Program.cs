using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DataManager;
using NLog; //rfgdg

namespace DataLogger
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        [STAThread]
        static void Main()
        {
            Console.WriteLine($"Start App");
            logger.Trace($"Trace Log");
            logger.Debug($"Debug Log");
            logger.Info($"Info Log");
            logger.Warn($"Warn Log");
            logger.Error($"Error Log");
            logger.Fatal($"Fatal Log");
            Console.ReadLine();


            if (Config.Sets.Running) Config.Start();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new formMain());
        }
    }
}
