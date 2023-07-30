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
using DataLogger;

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
            string fTranDT = "2023/05/05 10:11:00 AM";
            string fullTableName = "Logger";
            List<OdbcParameter> parameters = new List<OdbcParameter>();
            OdbcTransaction transaction;
            StringBuilder sqlIns = new StringBuilder("Insert into " + fullTableName + " (");
            StringBuilder sqlVal = new StringBuilder(" Values (to_timestamp(");
            
            dt = new DataTable(fullTableName.ToString());
            sqlIns.Append(fTranDT + ")");
            sqlVal.Append("? , 'YYYY/MM/DD HH24:Mi:SS.ff'));");

            OdbcParameter parameter = new OdbcParameter();
            parameter.ParameterName = "@tdt";
            parameter.OdbcType = OdbcType.DateTime;
            parameter.SourceColumn = fTranDT;
            parameters.Add(parameter);
            dt.Columns.Add(fTranDT, System.Type.GetType("System.DateTime"));
                    
            

            foreach (OdbcParameter par in cmd.Parameters)
            {
                switch (par.ParameterName)
                {
                    case "@tdt":
                        par.SourceColumn = DateTime.Now.ToString();
                        break;
                }
            }




           /* DateTime value = new DateTime(2028, 7, 28, 08, 0, 0, 12);
            comm1 = string.Format("insert into test (tdt) values (to_timestamp('" + value.ToString("yyyy/MM/dd HH:mm:ss.ff") + "', 'YYYY/MM/DD HH24:Mi:SS.ff'));");
            cmd = new OdbcCommand(comm1,conn);
            da = new OdbcDataAdapter(cmd);
            NLogger.logger.Trace(cmd.CommandText);
           */

            OdbcConnectionStringBuilder connStringBuilder = new OdbcConnectionStringBuilder();
            connStringBuilder.Dsn = "toOracle";
            connStringBuilder.Add("Uid", "Archivarius");
            connStringBuilder.Add("Pwd", "112358");

            conn.ConnectionString = connStringBuilder.ConnectionString;
            conn.Open();
            transaction = conn.BeginTransaction();
            cmd.Transaction = transaction;
            da.InsertCommand = cmd;
           // cmd.ExecuteNonQuery();
            conn.Close();
            
        }

    }
     
           
            
              





    internal class DataOracle
    {
    }
}
