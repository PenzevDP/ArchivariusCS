using System;
using System.Data;
using System.Text;
using System.Data.Odbc;
using System.Windows.Forms;
using System.Threading;
using System.Timers;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Settings;
using Tools;
using NLog;
using NLog.Fluent;
using System.Data.SqlTypes;
using Archivarius;

namespace DataOracle
{
    
    public class NLogger
    {
        public static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
    
    public class ConfigStatisticsEventArgs : EventArgs
    {
        private ConnectionState odbcState;
        public ConnectionState ODBCConnState
        {
            get
            {
                return odbcState;
            }
        }
    }
    public class ODBCConnector 
    {
        private bool disposed = false;
        private SingleEventLog log;
        public OdbcConnection conn;
        public OdbcDataAdapter da;
       
        private OdbcCommand cmd;
        private DataTable dt;
        public string comm1;
        

        public event StateChangeEventHandler StateChange;

       
        public ODBCConnector()
        {
            
            conn = new OdbcConnection();
           
        }
        public void Connect(string dsn, string uid, string pwd)
        {
            
            
                OdbcConnectionStringBuilder connStringBuilder = new OdbcConnectionStringBuilder();
                connStringBuilder.Dsn = dsn;
                connStringBuilder.Add("Uid", uid);
                connStringBuilder.Add("Pwd", pwd);

                conn.ConnectionString = connStringBuilder.ConnectionString;

                try
                {
                    conn.Open();

                NLogger.logger.Trace(conn.State);
            
                }
                catch (Exception e)
                {
                    log.WriteEntry(e.Message);
               
                 }
          

        }
        public void Disconnect()
        {
            if (conn.State != ConnectionState.Closed)
            {
                try
                {
                    conn.Close();
                    NLogger.logger.Trace(conn.State);
                }
                catch (Exception e)
                {
                    log.WriteEntry(e.Message);
                }
            }
        }

        public void Command(string command)
        {

           string value = new DateTime(2028, 7, 28, 08, 0, 0, 12).ToString("yyyy/MM/dd HH:mm:ss.ff");
            string value1 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff");
            comm1 = string.Format("insert into test (tdt) values (to_timestamp('" + value1 + "', 'YYYY/MM/DD HH24:Mi:SS.ff'));");
            cmd = new OdbcCommand(comm1,conn);
            da = new OdbcDataAdapter(cmd);
            NLogger.logger.Trace(cmd.CommandText);
          

            OdbcConnectionStringBuilder connStringBuilder = new OdbcConnectionStringBuilder();
            connStringBuilder.Dsn = "toOracle";
            connStringBuilder.Add("Uid", "Archivarius");
            connStringBuilder.Add("Pwd", "112358");

            conn.ConnectionString = connStringBuilder.ConnectionString;
            conn.Open();
            
            
            da.InsertCommand = cmd;
            cmd.ExecuteNonQuery();
            conn.Close();
            
        }

    }
     
           
            
              





    internal class DataOracle
    {
    }
}
