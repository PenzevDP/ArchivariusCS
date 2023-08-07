using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataManager;


namespace Archivarius
{
    
    public partial class formTestOracle : Form
    {
        public string command;

        public formTestOracle()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool a;

       //     DataOracle.ODBCConnector connector = new DataOracle.ODBCConnector();
        //   connector.Connect("toOracle", "ARCHIVARIUS", "112358");
           

            
        }

        private void button3_Click(object sender, EventArgs e)
        {
       //     DataOracle.ODBCConnector connector = new DataOracle.ODBCConnector();
        //    connector.Disconnect();
       //     NLogger.logger.Trace($"Service. tried to disconnect to Oacle DB");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
      //      command = textBox1.Text;
      //      NLogger.logger.Trace(command);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataOracle.ODBCConnector connector = new DataOracle.ODBCConnector();
           connector.Command(command);
         NLogger.logger.Trace(command);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }
    }
    
}
