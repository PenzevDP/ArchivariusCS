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

//---OPCUA---
using Siemens.UAClientHelper;
using Siemens.OpcUA;
using Siemens;
using Opc.Ua;
using Opc.Ua.Client;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//---OPCUA---


namespace DataManager
{
    
    public class NLogger
    {
        public static readonly Logger logger = LogManager.GetCurrentClassLogger();
    }
     
        


    public enum ConfigState
    {
        Starting,
        Started,
        Stopping,
        Stopped,
    }

    //---OPCUA----
    public enum OPCUAState
    {
        Unknown,
        Disconnected,
        Failed,
        Running,

    }

    public enum OPCUAQuality
    {
        Running = 0,
        Failed = 1,
        NoConfiguration = 2,
        Suspended = 3,
        Shutdown = 4,
        Test = 5,
        CommunicationFault = 6,
        Unknown = 7
    }
    //---------------



    public class ConfigStateEventArgs : EventArgs
    {
        private ConfigState state;

        public ConfigStateEventArgs(ConfigState state)
        {
            this.state = state;
        }
        public ConfigState State
        {
            get
            {
                return this.state;
            }
        }
    }
    public class ConfigStatisticsEventArgs : EventArgs
    {
        private string tranName;
        private ConnectionState odbcState;
        private OPCUAState opcuaState;
        private Statistics tranStat;

        public ConfigStatisticsEventArgs(Transaction tran)
        {
            tranName = tran.tranName;
            opcuaState = tran.opcuaConn.State;
            odbcState = tran.odbcConn.State;
            tranStat = new Statistics(tran.stat);
        }
        public string TransactionName
        {
            get
            {
                return tranName;
            }
        }
        public ConnectionState ODBCConnState
        {
            get
            {
                return odbcState;
            }
        }

      
        public OPCUAState OPCUAConnState
        {
            get
            {
                return opcuaState;
            }
        }
        public Statistics TransactionStatistics
        {
            get
            {
                return tranStat;
            }
        }
    }


    //----------OPCUA----------------
    public class OPCUAEventArgs : EventArgs
    {
        private string desc;

        public OPCUAEventArgs(string text)
        {
            desc = text;
        }
        public string Description
        {
            get
            {
                return desc;
            }
        }
    }
    public class OPCUADataEventArgs : EventArgs
    {
        private int clientHandle;
        private object value;

        public OPCUADataEventArgs(int clientHandle, object value)
        {
            this.clientHandle = clientHandle;
            this.value = value;
        }
        public int ClientHandle
        {
            get
            {
                return this.clientHandle;
            }
        }
        public object Value
        {
            get
            {
                return this.value;
            }
        }
    }
    public class OPCUAStateEventArgs : EventArgs
    {
        private OPCUAState state;

        public OPCUAStateEventArgs(OPCUAState state)
        {
            this.state = state;
        }
        public OPCUAState State
        {
            get
            {
                return state;
            }
        }
    }

    public delegate void ConfigStateEventHandler(object sender, ConfigStateEventArgs e);
    public delegate void StatisticsEventHandler(object sender, ConfigStatisticsEventArgs e);

    //----------OPCUA----------------
    public delegate void OPCUAEventHandler(object sender, OPCUAEventArgs e);
    public delegate void OPCUADataEventHandler(object sender, OPCUADataEventArgs e);
    public delegate void OPCUAStateEventHandler(object sender, OPCUAStateEventArgs e);

    //---------------------------

    public static class Config
    {
        private static ConfigState state = ConfigState.Stopped;
        private static BackgroundWorker bgwStarting = null;
        private static BackgroundWorker bgwStopping = null;
        private static BackgroundWorker bgwDisposing = null;
        private static SingleEventLog appLog = null;
        private static AppSettings appSets = null;
        private static bool disposing = false;
        private static bool disposed = false;
        private static bool reconnect = false;
        private static bool stopping = false;
        private static List<Transaction> transactions = new List<Transaction>();

        private static BackgroundWorker BGWStarting
        {
            get
            {
                if (bgwStarting == null)
                {
                    bgwStarting = new BackgroundWorker();
                    bgwStarting.DoWork += Starting;
                }
                return bgwStarting;
            }
        }
        private static BackgroundWorker BGWStopping
        {
            get
            {
                if (bgwStopping == null)
                {
                    bgwStopping = new BackgroundWorker();
                    bgwStopping.DoWork += Stopping;
                }
                return bgwStopping;
            }
        }
        private static BackgroundWorker BGWDisposing
        {
            get
            {
                if (bgwDisposing == null)
                {
                    bgwDisposing = new BackgroundWorker();
                    bgwDisposing.DoWork += Disposing;
                }
                return bgwDisposing;
            }
        }

        public static SingleEventLog Log
        {
            get
            {
                if (appLog == null) appLog = new SingleEventLog(Application.ProductName);
                return appLog;
            }
        }
        public static AppSettings Sets
        {
            get
            {
                if (appSets == null)
                {
                    appSets = new AppSettings(Application.StartupPath + "\\config.xml", Log);
                    appSets.Load();
                }
                return appSets;
            }
        }

        public static ConfigState State
        {
            get
            {
                return state;
            }
        }
        public static bool IsDisposing
        {
            get
            {
                return disposing;
            }
        }
        public static bool IsDisposed
        {
            get
            {
                return disposed;
            }
        }

        

        //-------OPCUA--------
        private static void OPCUAStateChanged(object sender, OPCUAStateEventArgs e)
        {
            if (state == ConfigState.Started && e.State != OPCUAState.Running && !reconnect)
            {
                reconnect = true;
                StopTransact();
                if (!stopping)
                {
                    Thread.Sleep(1000);
                    if (!stopping)
                    {
                        CreateTransact();
                        StartTransact();
                    }
                }
                reconnect = false;
            }
        }
        //---------------------

        private static void TransactStatistics(object sender, ConfigStatisticsEventArgs e)
        {
            if (Statistics != null) Statistics(sender, e);
        }

        public static bool Ready
        {
            get
            {
                //--------OPCUA-----------
                //if (Sets.Primary_ODBC_DSN != "")
                if (Sets.Primary_ODBC_DSN != "" && Sets.Primary_OPCUA_EndpointURL != "" && Sets.Primary_OPCUA_EndpointSecurityPolicyUri != "")
                {
                    if (Sets.TransactionBase.Tables["TransactionTable"].Rows.Count > 0) return true;
                }
                return false;
            }
        }

        public static void Dispose()
        {
            disposing = true;
            if (!BGWDisposing.IsBusy) BGWDisposing.RunWorkerAsync();
            while (BGWDisposing.IsBusy) Application.DoEvents();
            disposing = false;
            disposed = true;
        }

        public static void Save()
        {
            if (appSets != null) appSets.Save();
        }
        public static void Start()
        {
            if (!BGWStarting.IsBusy) BGWStarting.RunWorkerAsync();
            if (!Sets.Running) Sets.Running = true;

        }
        public static void Stop()
        {
            Sets.Running = false;
            if (!BGWStopping.IsBusy) BGWStopping.RunWorkerAsync();
        }
        private static void CreateTransact()
        {
            string tableName;
            string fTranDT;
            string fCtrlDT;
            string fParID;
            string fParVal;
          
            transactions.Clear();

            foreach (DataRow row in Sets.TransactionBase.Tables["TransactionTable"].Rows)
            {
                tableName = row["Table Name"].ToString();
                fTranDT = row["Transaction DT"].ToString();
                fCtrlDT = row["Controller DT"].ToString();
                fParID = row["Parameter ID"].ToString();
                fParVal = row["Parameter Value"].ToString();
                
                Transaction tran = new Transaction(Log);

                tran.tranName = row["Transaction Name"].ToString();
                

                //--------OPCUA------
                tran.uaNSNumber = row["ns#"] is int ? (int)row["ns#"] : 0;
                tran.uaDbName = row["DBUA Name"].ToString();
                tran.uaSizeName = row["SizeUA Name"].ToString();
                tran.uaCounterName = row["CounterUA Name"].ToString();
                tran.uaArrName = row["ArrayUA Name"].ToString();
                //------------------------

                tran.odbcConn.GenerateCommand(tableName, fTranDT, fCtrlDT, fParID, fParVal);
                tran.Statistics += TransactStatistics;
               

                //-----OPCUA---------
                tran.OPCUAconnStateChange += OPCUAStateChanged;
                //-----------------

                tran.ConnectToODBC();

                //-----OPCUA---------
                tran.ConnectToOPCUA();
                //-----------------

                transactions.Add(tran);
                if (Statistics != null) Statistics(tran, new ConfigStatisticsEventArgs(tran));
            }
        }
        private static void Starting(object sender, DoWorkEventArgs e)
        {
            state = ConfigState.Starting;
            if (StateChange != null) StateChange(null, new ConfigStateEventArgs(state));

            stopping = false;
            CreateTransact();

            state = ConfigState.Started;
            if (StateChange != null) StateChange(null, new ConfigStateEventArgs(state));
            Log.WriteEntry("The configuration was started");
        }
        private static void Stopping(object sender, DoWorkEventArgs e)
        {
            if (state == ConfigState.Starting || state == ConfigState.Started)
            {
                state = ConfigState.Stopping;
                if (StateChange != null) StateChange(null, new ConfigStateEventArgs(state));
            }
            stopping = true;
            StopTransact();
            if (state != ConfigState.Stopped)
            {
                state = ConfigState.Stopped;
                Log.WriteEntry("The configuration was stopped");
                if (StateChange != null) StateChange(null, new ConfigStateEventArgs(state));
            }
        }
        private static void StartTransact()
        {

            foreach (Transaction tran in transactions)
            {
                tran.ConnectToODBC();

                //-------OPCUA-----------
                tran.ConnectToOPCUA();
            }
        }
        private static void StopTransact()
        {
            foreach (Transaction tran in transactions)
            {

                //-------OPCUA----------
                tran.DisonnectFromOPCUA();
                //----------------------
                tran.DisconnectFromODBC();
            }
            bool busy;
            do
            {
                busy = false;
                foreach (Transaction tran in transactions)
                {
                    if (tran.IsBusy)
                    {
                        busy = true;
                        break;
                    }
                }
            } while (busy);
        }
        private static void Disposing(object sender, DoWorkEventArgs e)
        {
            if (!BGWStopping.IsBusy)
            {
                BGWStopping.RunWorkerAsync();
                while (BGWStopping.IsBusy) Application.DoEvents();
            }
        }

        public static event ConfigStateEventHandler StateChange;
        public static event StatisticsEventHandler Statistics;
    }

    public class ODBCConnector : IDisposable
    {
        private bool disposed = false;
        private SingleEventLog log;
        private OdbcConnection conn;
        private OdbcCommand cmd;
        private DataTable dt;

        public event StateChangeEventHandler StateChange;

        public ConnectionState State
        {
            get
            {
                return conn.State;
            }
        }

        public ODBCConnector(SingleEventLog eventLog)
        {
            log = eventLog;
            conn = new OdbcConnection();
            conn.StateChange += ConnectionStateChange;
        }
        ~ODBCConnector()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (conn.State != ConnectionState.Closed) Disconnect();
                    conn.Dispose();
                }
            }
            disposed = true;
        }
        private void ConnectionStateChange(object sender, StateChangeEventArgs e)
        {
            if (StateChange != null) StateChange(this, e);
        }

        public void Connect(string dsn, string uid, string pwd)
        {
            if (conn.State == ConnectionState.Closed)
            {
                OdbcConnectionStringBuilder connStringBuilder = new OdbcConnectionStringBuilder();
                connStringBuilder.Dsn = dsn;
                connStringBuilder.Add("Uid", uid);
                connStringBuilder.Add("Pwd", pwd);

                conn.ConnectionString = connStringBuilder.ConnectionString;

                try
                {
                    conn.Open();
                }
                catch (Exception e)
                {
                    log.WriteEntry(e.Message);
                }
            }
        }
        public void Disconnect()
        {
            if (conn.State != ConnectionState.Closed)
            {
                try
                {
                    conn.Close();
                }
                catch (Exception e)
                {
                    log.WriteEntry(e.Message);
                }
            }
        }
        public static Exception TestConnection(string dsn, string uid, string pwd)
        {
            OdbcConnectionStringBuilder connStringBuilder = new OdbcConnectionStringBuilder();
            connStringBuilder.Dsn = dsn;
            connStringBuilder.Add("Uid", uid);
            connStringBuilder.Add("Pwd", pwd);

            OdbcConnection testConn = new OdbcConnection(connStringBuilder.ConnectionString);

            try
            {
                testConn.Open();
            }
            catch (Exception e)
            {
                return e;
            }
            finally
            {
                if (testConn.State == ConnectionState.Open) testConn.Close();
            }
            return null;
        }
        public bool GenerateCommand(string tableName, string fTranDT, string fCtrlDT, string fParID, string fParVal)
        {
            StringBuilder fullTableName = new StringBuilder();

            if (tableName.Contains("."))
            {
                string name;
                string[] split = tableName.Split('.');
                int length = split.Length;

                for (int i = 0; i < length; i++)
                {
                    name = split[i];
                    if (name.Contains("[") || name.Contains("]"))
                    {
                        if (i == 0) fullTableName.Append(name); else fullTableName.Append("." + name);
                    }
                    else
                    {
                        if (i == 0) fullTableName.Append("[" + name + "]"); else fullTableName.Append(".[" + name + "]");
                    }
                }
            }
            else
            {
                if (tableName.Contains("]") || tableName.Contains("["))
                {
                    fullTableName.Append(tableName);
                }
                else
                {
                    fullTableName.Append("[" + tableName + "]");
                }
            }

            dt = new DataTable(fullTableName.ToString());
            StringBuilder sqlIns = new StringBuilder("Insert into " + fullTableName.ToString() + " (");
            StringBuilder sqlVal = new StringBuilder(" Values (");
            List<OdbcParameter> parameters = new List<OdbcParameter>();
            bool prevPar = false;

            if (fTranDT != "")
            {
                sqlIns.Append(fTranDT);
                sqlVal.Append("?");
                OdbcParameter parameter = new OdbcParameter();
                parameter.ParameterName = "@tdt";
                parameter.OdbcType = OdbcType.DateTime;
                parameter.SourceColumn = fTranDT;
                parameters.Add(parameter);
                dt.Columns.Add(fTranDT, System.Type.GetType("System.DateTime"));
                prevPar = true;
            }
            if (fCtrlDT != "")
            {
                if (prevPar)
                {
                    sqlIns.Append(", " + fCtrlDT);
                    sqlVal.Append(", ?");
                }
                else
                {
                    sqlIns.Append(fCtrlDT);
                    sqlVal.Append("?");
                }
                OdbcParameter parameter = new OdbcParameter();
                parameter.ParameterName = "@dt";
                parameter.OdbcType = OdbcType.DateTime;
                parameter.SourceColumn = fCtrlDT;
                parameters.Add(parameter);
                dt.Columns.Add(fCtrlDT, System.Type.GetType("System.DateTime"));
                prevPar = true;
            }
            if (fParID != "")
            {
                if (prevPar)
                {
                    sqlIns.Append(", " + fParID);
                    sqlVal.Append(", ?");
                }
                else
                {
                    sqlIns.Append(fParID);
                    sqlVal.Append("?");
                }
                OdbcParameter parameter = new OdbcParameter();
                parameter.ParameterName = "@id";
                parameter.OdbcType = OdbcType.Int;
                parameter.SourceColumn = fParID;
                parameters.Add(parameter);
                dt.Columns.Add(fParID, System.Type.GetType("System.Int32"));
                prevPar = true;
            }
            if (fParVal != "")
            {
                if (prevPar)
                {
                    sqlIns.Append(", " + fParVal);
                    sqlVal.Append(", ?");
                }
                else
                {
                    sqlIns.Append(fParVal);
                    sqlVal.Append("?");
                }
                OdbcParameter parameter = new OdbcParameter();
                parameter.ParameterName = "@val";
                parameter.OdbcType = OdbcType.Real;
                parameter.SourceColumn = fParVal;
                parameters.Add(parameter);
                dt.Columns.Add(fParVal, System.Type.GetType("System.Single"));
            }
            sqlIns.Append(")");
            sqlVal.Append(")");

            if (parameters.Count > 0)
            {
                cmd = new OdbcCommand(sqlIns.ToString() + sqlVal.ToString(), conn);
                NLogger.logger.Trace(cmd.CommandText);
                foreach (OdbcParameter par in parameters) cmd.Parameters.Add(par);
                return true;
            }
            else
            {
                cmd = null;
                return false;
            }
        }

        private void LogInvalidRecord(Record rec, object dt, object id, object val)
        {
            if (!rec.valid)
            {
                string message = "Record was dropped. ";
                NLogger.logger.Trace($"Record was dropped. ");
                if (rec.dtValid)
                {
                    message += "Timestamp " + dt + ". ";
                }
                else
                {
                    message += "Timestamp invalid. ";
                    NLogger.logger.Trace($"Timestamp invalid. ");
                }
                if (rec.idValid)
                {
                    message += "ID #" + id + ". ";
                }
                else
                {
                    message += "ID invalid. ";
                    NLogger.logger.Trace($"ID invalid. ");
                }
                if (rec.valValid)
                {
                    message += "Value is " + val;
                }
                else
                {
                    message += "Value invalid";
                    NLogger.logger.Trace($"Value invalid");
                }

                log.WriteEntry(message);
            }
        }

        public bool MakeTransactionUA(Transaction tran)
        {
            if (conn.State == ConnectionState.Open && tran.opcuaConn.State == OPCUAState.Running)
            {
                int count;
                object objVal;
                if (tran == null) return false;
                if (tran.records == null) return false;
                if (tran.records.Count == 0) return false;
                if (cmd == null) return false;
                if (!tran.opcuaConn.ReadDIntValue(tran.uaNSNumber, tran.uaDbName, tran.uaCounterName, out count)) return false;
                if (count > tran.Size) count = tran.Size;

                // Synchronous reading OPC items

                tran.opcuaConn.ReadDiffTypeValues(tran.uaNSNumber, tran.uaDbName, tran.uaArrName, count, tran.records);

                Record rec;

                // Validating                
                for (int i = 0; i < count; i++)
                {
                    rec = tran.records[i];
                    if (!rec.valid) LogInvalidRecord(rec, rec.dtua, rec.idua, rec.valua);
                }

                bool isValid = false;
                for (int i = 0; i < count; i++)
                {
                    if (tran.records[i].valid)
                    {
                        isValid = true;
                        break;
                    }
                }
                //--------------------
                //valid if count=0;
                if (count <= 0)
                {
                    isValid = true;
                    NLogger.logger.Trace(isValid.ToString);
                    return true;
                }
                //--------------------
                // Not valid transactions
                if (!isValid)
                {
                    try
                    {
                        //tran.tagCount.Write(0);
                        //log.WriteEntry("Trying to Zeroing Counter in invalid records");
                        tran.opcuaConn.WriteDIntValue(tran.uaNSNumber, tran.uaDbName, tran.uaCounterName, 0);
                    }
                    catch (Exception ex)
                    {
                        log.WriteEntry(Application.ProductName + ". Zeroing Counter error in invalid record: " + ex.Message);
                    }
                    return false;
                }

                // Start a local transaction
                OdbcDataAdapter da = new OdbcDataAdapter();
                OdbcTransaction transaction;
                DateTime dtNow = DateTime.Now;

                try
                {
                    transaction = conn.BeginTransaction();
                    cmd.Transaction = transaction;
                    da.InsertCommand = cmd;
                    NLogger.logger.Trace(da.InsertCommand.CommandText);
                }
                catch (Exception e)
                {
                    // A transaction is currently active. 
                    // Parallel transactions are not supported
                    log.WriteEntry(e.Message);
                    return false;
                }

                DataTable newRecords = dt.Copy();
                DataRow newRow;

                for (int i = 0; i < count; i++)
                {
                    rec = tran.records[i];
                    NLogger.logger.Trace(rec.valua.ToString());
                    if (rec.valid)
                    {
                        newRow = newRecords.NewRow();
                        
                        foreach (OdbcParameter par in cmd.Parameters)
                        {
                            switch (par.ParameterName)
                            {
                                case "@tdt":
                                    newRow[par.SourceColumn] = dtNow;
                                    break;
                                case "@dt":

                                    objVal = rec.dtua;
                                    if (objVal is Decimal)
                                    {
                                        newRow[par.SourceColumn] = (DateTime)objVal;
                                        break;
                                    }

                                    // Attempt to roll back the transaction.
                                    transaction.Rollback();
                                    cmd.Transaction = null;
                                    return false;

                                case "@id":

                                    objVal = rec.idua;
                                    if (objVal is int)
                                    {
                                        newRow[par.SourceColumn] = (int)objVal;
                                        break;
                                    }

                                    // Attempt to roll back the transaction.
                                    transaction.Rollback();
                                    cmd.Transaction = null;
                                    return false;

                                case "@val":

                                    objVal = rec.valua;
                                    if (objVal is float)
                                    {
                                        newRow[par.SourceColumn] = (float)objVal;
                                        break;
                                    }

                                    // Attempt to roll back the transaction.
                                    transaction.Rollback();
                                    cmd.Transaction = null;
                                    return false;
                            }
                        }
                        newRecords.Rows.Add(newRow);
                    }
                }

                if (newRecords.Rows.Count > 0)
                {
                   // NLogger.logger.Trace(newRecords.Rows.Count);
                    try
                    {
                        NLogger.logger.Trace(newRecords.ToString()) ;
                        da.Update(newRecords);
                    }
                    catch (Exception e)
                    {
                        log.WriteEntry(e.Message);
                        try
                        {
                            // Attempt to roll back the transaction.
                            transaction.Rollback();
                            cmd.Transaction = null;
                            return false;
                        }
                        catch (Exception ex)
                        {
                            log.WriteEntry(ex.Message);
                            cmd.Transaction = null;
                            return false;
                        }
                    }

                    // Commit the transaction.
                    try
                    {
                        transaction.Commit();
                        cmd.Transaction = null;
                        //log.WriteEntry("Trying to Zeroing Counter in Committransactions");
                        tran.opcuaConn.WriteDIntValue(tran.uaNSNumber, tran.uaDbName, tran.uaCounterName, 0);
                    }
                    catch (Exception ex)
                    {
                        log.WriteEntry(Application.ProductName + ". Error in committing or zeroing counter after transaction: " + ex.Message);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    public class Record
    {


        public DateTime dtua;
        public int idua;
        public float valua;
        //-----OPCUA-----

        public bool valid;
        public bool dtValid;
        public bool idValid;
        public bool valValid;
    }

    public class Statistics
    {
        private ulong total = 0UL;
        private ulong passed = 0UL;
        private ulong failed = 0UL;

        public Statistics()
        {
        }
        public Statistics(Statistics stat)
        {
            total = stat.Total;
            passed = stat.Passed;
            failed = stat.Failed;
        }

        public void Clear()
        {
            total = 0UL;
            passed = 0UL;
            failed = 0UL;
        }
        public void Pass()
        {
            if (total == ulong.MaxValue) Clear();
            passed++;
            total++;
        }
        public void Fail()
        {
            if (total == ulong.MaxValue) Clear();
            failed++;
            total++;
        }

        public ulong Failed
        {
            get
            {
                return failed;
            }
        }
        public ulong Passed
        {
            get
            {
                return passed;
            }
        }
        public float Percent
        {
            get
            {
                return total > 0UL ? (float)(100D * (double)passed / (double)total) : 0f;
            }
        }
        public ulong Total
        {
            get
            {
                return total;
            }
        }
    }

    public class Transaction
    {
        //private int size = 0;
        private SingleEventLog log;
        private BackgroundWorker bgwODBCConn;
        private BackgroundWorker bgwTransact;




        public Statistics stat;
        public ODBCConnector odbcConn;


        public string tranName = "";


        public bool active = false;
        public bool error = false;

        //private bool disconnectOPC = false;

        public List<Record> records;



        //---OPCUA----
        private int uasize = 0;
        private BackgroundWorker bgwOPCUAConn;
        public OPCUA opcuaConn;
        //public MonitoredItem uatagSize = null;
        public MonitoredItem uatagCount = null;
        public int uaNSNumber = 0;
        public string uaDbName = "";
        public string uaSizeName = "";
        public string uaCounterName = "";
        public string uaArrName = "";
        private bool disconnectopcua = false;

        System.Timers.Timer timercheckCount = null;
        //----OPCUA----


        public event StateChangeEventHandler ODBCConnStateChange;
        //public event OPCStateEventHandler OPConnStateChange;
        public event StatisticsEventHandler Statistics;

        //-----OPCUA----
        public event OPCUAStateEventHandler OPCUAconnStateChange;
        //--------------

        public bool IsBusy
        {
            get
            {

                return bgwODBCConn.IsBusy || bgwTransact.IsBusy || bgwOPCUAConn.IsBusy;
            }
        }
        public int Size
        {
            get
            {

                return uasize;
            }
        }

        public Transaction(SingleEventLog eventLog)
        {
            log = eventLog;

            stat = new Statistics();

            odbcConn = new ODBCConnector(log);
            odbcConn.StateChange += ODBCStateChange;




            //---OPCUA-----
            opcuaConn = new OPCUA(log);
            opcuaConn.DataChanged += OPCUADataChanged;
            opcuaConn.StateChange += OPCUAStateChange;


            bgwOPCUAConn = new BackgroundWorker();
            bgwOPCUAConn.DoWork += OPCUAConnecting;
            bgwOPCUAConn.RunWorkerCompleted += OPCUAConnected;
            bgwOPCUAConn.WorkerSupportsCancellation = true;


            timercheckCount = new System.Timers.Timer(Config.Sets.UpdateRate);
            timercheckCount.Enabled = false;
            timercheckCount.Elapsed += TickDataChanged;
            //------------

            bgwODBCConn = new BackgroundWorker();
            bgwODBCConn.DoWork += ODBCConnecting;
            bgwODBCConn.RunWorkerCompleted += ODBCConnected;
            bgwODBCConn.WorkerSupportsCancellation = true;

            bgwTransact = new BackgroundWorker();
            bgwTransact.DoWork += MakeTransactions;
            bgwTransact.WorkerSupportsCancellation = true;
        }
        public void ConnectToODBC()
        {
            if (!(odbcConn.State == ConnectionState.Open || odbcConn.State == ConnectionState.Connecting || bgwODBCConn.IsBusy))
            {
                bgwODBCConn.RunWorkerAsync();
            }
        }
        public void DisconnectFromODBC()
        {
            if (bgwODBCConn.IsBusy)
            {
                bgwODBCConn.CancelAsync();
                while (bgwODBCConn.IsBusy) Application.DoEvents();
            }
            if (odbcConn.State != ConnectionState.Closed) odbcConn.Disconnect();
        }


        //----OPCUA-----
        public void ConnectToOPCUA()
        {
            disconnectopcua = false;
            if (opcuaConn.State != OPCUAState.Running)
            {
                active = false;
                if (bgwTransact.IsBusy)
                {
                    bgwTransact.CancelAsync();
                    while (bgwTransact.IsBusy) Application.DoEvents();
                }
                if (!bgwOPCUAConn.IsBusy) bgwOPCUAConn.RunWorkerAsync();
            }
        }
        //------------


        //----OPCUA-----
        public void DisonnectFromOPCUA()
        {
            disconnectopcua = true;
            StopTransact();
            uasize = 0;
            //uatagSize = null;
            uatagCount = null;
            if (bgwOPCUAConn.IsBusy)
            {
                bgwOPCUAConn.CancelAsync();
                while (bgwOPCUAConn.IsBusy) Application.DoEvents();
            }
            if (records != null)
            {
                records.Clear();
                records = null;
            }
            timercheckCount.Elapsed -= TickDataChanged;
            timercheckCount.Stop();
            opcuaConn.Disconnect();
        }
        //------------


        public void StopTransact()
        {
            active = false;
            if (bgwTransact.IsBusy)
            {
                bgwTransact.CancelAsync();
                while (bgwTransact.IsBusy) Application.DoEvents();
            }
        }

        //----------OPCUA------------
        private void OPCUAConnecting(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (opcuaConn.State != OPCUAState.Running)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                try
                {
                   opcuaConn.Connect(Config.Sets.Primary_OPCUA_EndpointURL, Config.Sets.Primary_OPCUA_EndpointSecurityPolicyUri, (MessageSecurityMode)Config.Sets.Primary_OPCUA_EndpointSecurityMode, Config.Sets.Primary_OPCUA_LoginMode, Config.Sets.Primary_OPCUA_User, Config.Sets.Primary_OPCUA_Pass, Config.Sets.UpdateRate, Config.Sets.KeepAliveInterval);

                }
                catch (Exception ex)
                {
                    log.WriteEntry(ex.Message);
                }
                if (opcuaConn.State != OPCUAState.Running)
                {
                    for (int i = 1; i <= 100; i++)
                    {
                        Application.DoEvents();
                        if (worker.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            Thread.Sleep(100);
                        }
                    }
                }
            }
        }
        private void OPCUAConnected(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!(e.Cancelled || disconnectopcua))
            {
                bool result = false;

                uatagCount = opcuaConn.AddTrigger(uaNSNumber, uaDbName, uaCounterName, "item_ ns#" + uaNSNumber + " DB:" + uaDbName + " Tag:" + uaCounterName, 1);

                if (uatagCount == null)
                {
                    if (!disconnectopcua)
                    {
                        timercheckCount.Elapsed -= TickDataChanged;
                        timercheckCount.Stop();
                        opcuaConn.Disconnect();
                        log.WriteEntry("Disconnect from server in !disconnectopcua Reason:ua tagcount=null");
                        if (!bgwOPCUAConn.IsBusy) bgwOPCUAConn.RunWorkerAsync();
                    }
                    return;
                }

                // Reading of the clipboard size
                for (int i = 0; i < 10; i++)
                {
                    result = opcuaConn.ReadDIntValue(uaNSNumber, uaDbName, uaSizeName, out uasize);
                    if (result)
                    {
                        break;
                    }
                    else
                    {
                        Application.DoEvents();
                        Thread.Sleep(1000);
                    }
                }
                // Error of reading of the clipboard size
                if (!result)
                {
                    return;
                }
                // Creation of records for the clipboard
                if (uasize > 0)
                {
                    records = new List<Record>(uasize);
                    for (int i = 0; i < uasize; i++)
                    {
                        records.Add(new Record());
                    }

                }
                else
                {
                    return;
                }

                timercheckCount.Start();

                opcuaConn.IsSubscribed = true;
              
            }
            else
            {
               if (opcuaConn.State == OPCUAState.Running)
                {
                    try
                    {
                        timercheckCount.Elapsed -= TickDataChanged;
                        timercheckCount.Stop();
                        opcuaConn.Disconnect();
                        log.WriteEntry("Disconnected from server in disconnectopcua");
                    }
                    catch (Exception ex)
                    {
                        log.WriteEntry(ex.Message);
                    }
                }
            }
        }

        //--------------------------

        private void ODBCConnecting(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (odbcConn.State != ConnectionState.Open)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                try
                {
                    String dsn = Config.Sets.Primary_ODBC_DSN;
                    String uid = Config.Sets.Primary_ODBC_User;
                    String pwd = Config.Sets.Primary_ODBC_Pass;
                    odbcConn.Connect(dsn, uid, pwd);
                }
                catch (Exception ex)
                {
                    log.WriteEntry(ex.Message);
                }
                if (odbcConn.State != ConnectionState.Open)
                {
                    for (int i = 1; i <= 100; i++)
                    {
                        if (worker.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            Application.DoEvents();
                            Thread.Sleep(100);
                        }
                    }
                }
            }

        }
        private void ODBCConnected(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                try
                {
                    if (odbcConn.State != ConnectionState.Closed) odbcConn.Disconnect();
                }
                catch (Exception ex)
                {
                    log.WriteEntry(ex.Message);
                }
            }
        }
        private void MakeTransactions(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            bool resultOK;
            while (active && !worker.CancellationPending)
            {
                Application.DoEvents();
                if (odbcConn.State == ConnectionState.Open && opcuaConn.State == OPCUAState.Running)
                {
                    if (error) Thread.Sleep(1000);
                    if (worker.CancellationPending) break;
                    try
                    {
                        resultOK = odbcConn.MakeTransactionUA(this);
                    }
                    catch
                    {
                        resultOK = false;
                        log.WriteEntry("MakeTransaction Error");
                    }
                    if (resultOK)
                    {
                        active = false;
                        error = false;
                        stat.Pass();
                    }
                    else
                    {
                        error = true;
                        stat.Fail();
                    }
                    if (Statistics != null) Statistics(this, new ConfigStatisticsEventArgs(this));
                }
                else
                {
                    if (!worker.CancellationPending)
                    {
                        ConnectToODBC();
                        ConnectToOPCUA();
                    }
                    Thread.Sleep(1000);
                }
            }
        }

        // Events

        //-------OPCUA------------
        private void OPCUADataChanged(object sender, OPCUADataEventArgs e)
        {
            int count = e.Value is int ? (int)e.Value : 0;
            //if (count > 0)
            if (count > 0 && !active && (!bgwTransact.IsBusy))
            {
                active = true;
                //log.WriteEntry("Transaction in OPCUADataChanged");
                if (!bgwTransact.IsBusy) bgwTransact.RunWorkerAsync();
            }
        }

        private void TickDataChanged(object sender, ElapsedEventArgs e)
        {
            // if (opcuaConn.State == OPCUAState.Running)

            if (opcuaConn.State == OPCUAState.Running && !active && (!bgwTransact.IsBusy))
            {
                int Tickcount = 0;
                if (opcuaConn.ReadDIntValue(uaNSNumber, uaDbName, uaCounterName, out Tickcount))
                {
                    if (Tickcount > 0)
                    {
                        active = true;
                        //log.WriteEntry("Transaction in TickDataChanged");
                        if (!bgwTransact.IsBusy) bgwTransact.RunWorkerAsync();
                    }
                }
            }
        }

        private void OPCUAStateChange(object sender, OPCUAStateEventArgs e)
        {
            if (e.State != OPCUAState.Running) active = false;
            if (OPCUAconnStateChange != null) OPCUAconnStateChange(this, e);
            if (Statistics != null) Statistics(this, new ConfigStatisticsEventArgs(this));
        }
        //----------------------

        private void ODBCStateChange(object sender, StateChangeEventArgs e)
        {
            if (ODBCConnStateChange != null) ODBCConnStateChange(this, e);
            if (Statistics != null) Statistics(this, new ConfigStatisticsEventArgs(this));
        }
    }



    //---OPCUA---
    class OpcUaEndpointWrapper
    {
        #region Construction
        public OpcUaEndpointWrapper(EndpointDescription endpoint)
        {
            m_endpoint = endpoint;
        }
        #endregion

        #region Fields
        private EndpointDescription m_endpoint;
        #endregion

        #region Properties
        /// <summary>
        /// Provides the session being established with an OPC UA server.
        /// </summary>
        public EndpointDescription Endpoint
        {
            get { return m_endpoint; }
            set { m_endpoint = value; }
        }
        #endregion

        public override string ToString()
        {
            string sRet = m_endpoint.Server.ApplicationName.Text;
            sRet += " [";
            char seperator = '#';
            string[] collection = m_endpoint.SecurityPolicyUri.Split(seperator);
            sRet += collection[1];
            sRet += ", ";
            sRet += m_endpoint.SecurityMode.ToString();
            sRet += "] [";
            sRet += m_endpoint.EndpointUrl;
            sRet += "]";
            return sRet;
        }
    }


    public class OPCUA
    {
        private bool disposed = false;
        private SingleEventLog log;
        private const string serverName = "OPC.UA";
        private OPCUAState state = OPCUAState.Disconnected;
        private OPCUAQuality Quality = OPCUAQuality.Unknown;
        private UAClientHelperAPI serverObj;
        private Subscription subscriptionObj;
        private MonitoredItem groupTriggers = null;
        private MonitoredItem groupItems = null;
        private System.Timers.Timer timer = null;

        //private int updateRate = 1000;
        //private int KeepAliveInterval = 1000;

        public event OPCUADataEventHandler DataChanged;
        public event OPCUAStateEventHandler StateChange;



        public bool IsSubscribed
        {
            get
            {
                if (groupTriggers != null)
                {
                    return groupTriggers.Subscription.Created;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (groupTriggers != null)
                {
                    bool oldValue = groupTriggers.Subscription.Created;
                    if (value != oldValue)
                    {
                        groupTriggers.Subscription.Create();
                    }
                }
            }
        }

        public OPCUAState State
        {
            get
            {
                return state;
            }
        }

        public string ServerName
        {
            get
            {
                return serverName;
            }
        }

        public OPCUA(SingleEventLog eventLog)
        {
            log = eventLog;
            timer = new System.Timers.Timer(1000d);
            timer.Enabled = false;
            timer.Elapsed += Tick;



        }

        ~OPCUA()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //if (State == OPCUAState.Running) Disconnect();
                    if (State != OPCUAState.Disconnected && State != OPCUAState.Unknown) Disconnect();
                }

                //if (groupTriggers != null)
                //{
                //     Marshal.ReleaseComObject(groupTriggers);
                //     groupTriggers = null;
                //}
                //  if (serverObj != null)
                //  {
                //      Marshal.ReleaseComObject(serverObj);
                //      serverObj = null;
                // }
            }
            disposed = true;
        }


        private void OPCUADataChange(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {

            MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
            if (notification == null)
            {
                return;
            }

            if (DataChanged != null)
            {
                DataChanged(this, new OPCUADataEventArgs((int)(notification.ClientHandle), notification.Value.WrappedValue.Value));
            }


        }



        private void OPCUAShutDown(object sender, EventArgs e)
        {
            timer.Stop();
            state = GetOPCUAserverState(serverObj);
            log.WriteEntry(serverName + "OPC Ua ShutDown");
            if (StateChange != null) StateChange(this, new OPCUAStateEventArgs(state));
        }

        private void OPCUASubscriptionStateChanged(Subscription subscription, SubscriptionStateChangedEventArgs e)
        {
            //timer.Stop();
            //state = GetOPCUAserverState(serverObj);
            //if (StateChange != null) StateChange(this, new OPCUAStateEventArgs(state));
        }

        private void Tick(object sender, ElapsedEventArgs e)
        {
            OPCUAState getstate = GetOPCUAserverState(serverObj);
            if (state != getstate)
            {
                if (state == OPCUAState.Running && getstate != OPCUAState.Running)
                {
                    Disconnect();
                }
                else
                {
                    state = getstate;
                    if (StateChange != null) StateChange(this, new OPCUAStateEventArgs(state));
                }
            }
        }



        private OPCUAState GetOPCUAserverState(UAClientHelperAPI server)
        {
            if (server == null || server.Session == null || server.Session.Disposed || server.Session.SubscriptionCount == 0) return OPCUAState.Disconnected;

            switch (Quality)
            {
                case (OPCUAQuality.Running):
                    return OPCUAState.Running;

                case (OPCUAQuality.Failed):
                    return OPCUAState.Failed;

                case (OPCUAQuality.CommunicationFault):
                    return OPCUAState.Disconnected;

                default:
                    return OPCUAState.Disconnected;


            }

        }

        public bool Connect(string url, string SecPolicy, MessageSecurityMode MesSecMode, bool userAuth, string UserName, string Pwd, int updateRate, int KeepAliveInterval)
        {
            try
            {
                if (state != OPCUAState.Running)
                {
                    serverObj = new UAClientHelperAPI();
                    subscriptionObj = new Subscription();
                    try
                    {
                        serverObj.KeepAliveNotification += new KeepAliveEventHandler(clientAPI_KeepAlive);
                        serverObj.CertificateValidationNotification += new CertificateValidationEventHandler(clientAPI_CertificateEvent);
                        serverObj.Connect(url, SecPolicy, MesSecMode, userAuth, UserName, Pwd);

                        subscriptionObj = serverObj.Subscribe(updateRate);
                        serverObj.Session.KeepAliveInterval = KeepAliveInterval;

                        
                        log.WriteEntry(serverName + " Connection established");
                    }
                    catch (Exception e)
                    {
                        log.WriteEntry(serverName + ". " + e.Message);
                    }

                    OPCUAState getstate = GetOPCUAserverState(serverObj);
                    if (getstate == OPCUAState.Running)
                    {
                        if (state != getstate)
                        {
                            state = getstate;
                            if (StateChange != null) StateChange(this, new OPCUAStateEventArgs(state));
                        }
                        timer.Start();
                    }
                    else
                    {
                        Disconnect();
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                log.WriteEntry(serverName + ". Connection Error: " + e.Message);

                return false;
            }

            //this.updateRate = updateRate;
            //this.KeepAliveInterval = KeepAliveInterval;
            return true;
        }

        public void Disconnect()
        {
            timer.Stop();
            try
            {
                OPCUAState getstate = GetOPCUAserverState(serverObj);
                //Alarm Disconnect
                if (state == OPCUAState.Running && getstate != OPCUAState.Running)
                {
                    try
                    {
                        serverObj.Disconnect();
                        log.WriteEntry(serverName + " Alarm disconnect!");
                    }
                    catch (Exception e)
                    {
                        log.WriteEntry(serverName + " Alarm disconnect error! " + e.Message);
                    }
                    getstate = GetOPCUAserverState(serverObj);
                    if (state != getstate)
                    {
                        state = getstate;
                        if (StateChange != null) StateChange(this, new OPCUAStateEventArgs(state));
                    }
                }
                //Regular disconnect
                else if (getstate == OPCUAState.Running)
                {
                    try
                    {

                        //serverObj.Session.SessionClosing -= OPCUAShutDown;
                        serverObj.KeepAliveNotification -= new KeepAliveEventHandler(clientAPI_KeepAlive);
                        serverObj.CertificateValidationNotification -= new CertificateValidationEventHandler(clientAPI_CertificateEvent);
                        //serverObj.Session.Close();
                        //serverObj.RemoveSubscription(subscriptionObj);
                        serverObj.Disconnect();
                        //log.WriteEntry(serverName + " disconnect!");
                    }
                    catch (Exception e)
                    {
                        log.WriteEntry(serverName + " disconnect error! " + e.Message);
                    }
                    getstate = GetOPCUAserverState(serverObj);
                    if (state != getstate)
                    {
                        state = getstate;
                        if (StateChange != null) StateChange(this, new OPCUAStateEventArgs(state));
                    }
                }
            }
            catch (Exception e)
            {
                log.WriteEntry(serverName + " common disconnect error! " + e.Message);
            }
        }

        public MonitoredItem AddTrigger(int NodeNum, string DBName, string VarName, string ItemName, int SamplingInterval)
        {
            //if (State == OPCUAState.Disconnected) return null;
            if (State != OPCUAState.Running) return null;
            if (subscriptionObj == null) return null;
            string itemID = "ns=" + NodeNum + ";s=" + DBName + "." + VarName + "";
            try
            {
                if (groupTriggers == null)
                {
                    groupTriggers = serverObj.AddMonitoredItem(subscriptionObj, itemID, ItemName, SamplingInterval);
                    serverObj.ItemChangedNotification += new MonitoredItemNotificationEventHandler(OPCUADataChange);
                }

                return groupTriggers;
            }
            catch (Exception ex)
            {
                log.WriteEntry(serverName + "Error of addition of the trigger " + itemID + ". " + ex.Message);
                return null;
            }
        }

        public MonitoredItem AddItem(int NodeNum, string DBName, string VarName, string ItemName, int SamplingInterval)
        {
            if (State == OPCUAState.Disconnected) return null;
            if (subscriptionObj == null) return null;
            string itemID = "ns=" + NodeNum + ";s=" + DBName + "." + VarName + "";
            try
            {
                if (groupItems == null)
                {
                    groupItems = serverObj.AddMonitoredItem(subscriptionObj, itemID, ItemName, SamplingInterval);
                    subscriptionObj.ApplyChanges();
                }
                return groupItems;
            }
            catch (Exception ex)
            {
                log.WriteEntry(serverName + "Error of addition of the item " + itemID + ". " + ex.Message);
                return null;
            }
        }

        public bool ReadDIntValue(int NodeNum, string DBName, string VarName, out int value)
        {
            string NodeID = "ns=" + NodeNum + ";s=" + DBName + "." + VarName + "";
            if (State == OPCUAState.Running)
            {

                List<string> ReadData = new List<string>();
                List<string> NodeList = new List<string>();
                try
                {
                    NodeList.Add(NodeID);
                    ReadData = serverObj.ReadValues(NodeList);
                    value = Int32.Parse(ReadData[0]);
                    return true;

                }
                catch (Exception e)
                {
                    log.WriteEntry(serverName + " read error DIntItem= " + NodeID + "! " + e.Message);

                    value = 0;
                    return false;
                }
            }
            else
            {
                log.WriteEntry(serverName + "DIntItem= " + NodeID + " doesn't Read! Server is stopped ");
                value = 0;
                return false;
            }

        }
        public bool ReadRealValue(int NodeNum, string DBName, string VarName, out float value)
        {
            string NodeID = "ns=" + NodeNum + ";s=" + DBName + "." + VarName + "";
            if (State == OPCUAState.Running)
            {
                List<string> ReadData = new List<string>();
                List<string> NodeList = new List<string>();
                try
                {
                    NodeList.Add(NodeID);
                    ReadData = serverObj.ReadValues(NodeList);
                    value = float.Parse(ReadData[0]);
                    return true;

                }
                catch (Exception e)
                {
                    log.WriteEntry(serverName + " read error RealItem= " + NodeID + "! " + e.Message);
                    value = 0.0F;
                    return false;
                }
            }
            else
            {
                log.WriteEntry(serverName + "RealItem= " + NodeID + " doesn't Read! Server is stopped ");
                value = 0.0F;
                return false;
            }

        }
        public bool ReadDateTimeValue(int NodeNum, string DBName, string VarName, out DateTime dt)
        {
            string NodeID = "ns=" + NodeNum + ";s=" + DBName + "." + VarName + "";
            if (state == OPCUAState.Running)
            {
                List<string> NodeList = new List<string>();
                List<string> readarray = new List<string>();

                try
                {
                    NodeList.Add(NodeID);
                    readarray = serverObj.ReadValues(NodeList);
                    string[] dts = readarray[0].Split(';');

                    dt = new DateTime(2000 + Int32.Parse(dts[0]), Int32.Parse(dts[1]), Int32.Parse(dts[2]), Int32.Parse(dts[3]), Int32.Parse(dts[4]), Int32.Parse(dts[5]), Int32.Parse(dts[6]) * 10);
                    return true;
                }
                catch (Exception e)
                {
                    log.WriteEntry(serverName + " read error DateTimeItem= " + NodeID + "! " + e.Message);
                    dt = new DateTime();
                    return false;

                }

            }
            else
            {
                log.WriteEntry(serverName + "DateTimeItem= " + NodeID + " doesn't Read! Server is stopped ");
                dt = new DateTime();
                return false;
            }

        }


        public bool ReadDIntValue(string Node, out int value)
        {

            if (state == OPCUAState.Running)
            {
                try
                {
                    value = (Int32)serverObj.Session.ReadValue(new NodeId(Node)).Value; ;
                    return true;
                }
                catch (Exception e)
                {
                    log.WriteEntry(serverName + " read error DInt= " + Node + "! " + e.Message);
                    value = 0;
                    return false;

                }

            }
            else
            {
                log.WriteEntry(serverName + "DIntItem= " + Node + " doesn't Read! Server is stopped ");
                value = 0;
                return false;
            }

        }
        public bool ReadRealValue(string Node, out float value)
        {

            if (state == OPCUAState.Running)
            {
                try
                {
                    value = (float)serverObj.Session.ReadValue(new NodeId(Node)).Value; ;
                    return true;
                }
                catch (Exception e)
                {
                    log.WriteEntry(serverName + " read error Real= " + Node + "! " + e.Message);
                    value = 0.0F;
                    return false;

                }

            }
            else
            {
                log.WriteEntry(serverName + "RealItem= " + Node + " doesn't Read! Server is stopped ");
                value = 0.0F;
                return false;
            }

        }
        public bool ReadDateTimeValue(string Node, out DateTime dt)
        {
            if (state == OPCUAState.Running)
            {
                try
                {
                    dt = (DateTime)serverObj.Session.ReadValue(new NodeId(Node)).Value; ;
                    return true;
                }
                catch (Exception e)
                {
                    log.WriteEntry(serverName + " read error DateTimeItem= " + Node + "! " + e.Message);
                    dt = new DateTime();
                    return false;

                }

            }
            else
            {
                log.WriteEntry(serverName + "DateTimeItem= " + Node + " doesn't Read! Server is stopped ");
                dt = new DateTime();
                return false;
            }

        }


        public bool WriteDIntValue(int NodeNum, string DBName, string VarName, in int value)
        {
            string NodeID = "ns=" + NodeNum + ";s=" + DBName + "." + VarName + "";
            if (state == OPCUAState.Running)
            {

                List<string> NodeList = new List<string>();
                List<string> values = new List<string>();

                try
                {
                    values.Add(value.ToString());
                    NodeList.Add(NodeID);
                    serverObj.WriteValues(values, NodeList);
                    return true;
                }
                catch (Exception e)
                {
                    log.WriteEntry(serverName + " Write error DIntItem= " + NodeID + "! " + e.Message);
                    return false;

                }

            }
            else
            {
                log.WriteEntry(serverName + "DIntItem= " + NodeID + " doesn't Write! Server is stopped");
                return false;
            }

        }

        public void ReadStructUdt(int NodeNum, string DBName, string VarName, int count, List<Record> records)
        {
            string NodeID = "ns=" + NodeNum + ";s=" + DBName + "." + VarName + "";
            if (state == OPCUAState.Running)
            {
                List<string[]> readarray = new List<string[]>();
                try
                {
                    readarray = serverObj.ReadStructUdt(NodeID);
                    //Разбор считанного массива на записи
                    for (int i = 0; i < count; i++)
                    {
                        try
                        {

                            GetDateTimeFromRecord(readarray[i * 5 + 2].ElementAt(1), records[i], i);
                            GetIDFormRecord(readarray[i * 5 + 3].ElementAt(1), records[i], i);
                            GetValFormRecord(readarray[i * 5 + 4].ElementAt(1), records[i], i);
                            records[i].valid = (records[i].dtValid && records[i].idValid && records[i].valValid);

                        }

                        catch (Exception ex)
                        {
                            log.WriteEntry(serverName + " Read Record# " + i + "! Error:" + ex.Message);

                        }

                    }

                }

                catch (Exception ex)
                {

                    log.WriteEntry(serverName + " Read Array Item =:" + NodeID + "!" + ex.Message);
                }


            }
            else
            {
                log.WriteEntry(serverName + "Can't Read Array. Server isn't running:");

            }

        }

        public void ReadValues(int NodeNum, string DBName, string VarName, int count, List<Record> records)
        {
            string NodeID = "ns=" + NodeNum + ";s=" + DBName + "." + VarName + "";
            if (state == OPCUAState.Running)
            {
                List<string> readarray = new List<string>();
                List<string> _nodes = new List<string>();


                try
                {

                    //Разбор считанного массива на записи
                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            _nodes.Add(NodeID + $"[{i}].DateTime");
                            _nodes.Add(NodeID + $"[{i}].ID");
                            _nodes.Add(NodeID + $"[{i}].Value");
                        }

                        catch (Exception ex)
                        {
                            log.WriteEntry(serverName + " Add NodeRecord# " + i + "! Error:" + ex.Message);
                            throw;
                        }

                    }
                    readarray = serverObj.ReadValues(_nodes);
                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            GetDateTimeFromRecord_1(readarray[i * 3], records[i], i);
                            GetIDFormRecord(readarray[i * 3 + 1], records[i], i);
                            GetValFormRecord(readarray[i * 3 + 2], records[i], i);
                            records[i].valid = (records[i].dtValid && records[i].idValid && records[i].valValid);
                        }

                        catch (Exception ex)
                        {
                            log.WriteEntry(serverName + " Read Record# " + i + "! Error:" + ex.Message);
                            throw;
                        }

                    }

                }
                catch (Exception ex)
                {

                    log.WriteEntry(serverName + " Read Array Error Item =:" + NodeID + "!" + ex.Message);
                }
            }
            else
            {
                log.WriteEntry(serverName + "Can't Read Array. Server isn't running:");

            }

        }

        public void ReadDiffTypeValues(int NodeNum, string DBName, string VarName, int count, List<Record> records)
        {
            string NodeID = "ns=" + NodeNum + ";s=" + DBName + "." + VarName + "";
            if (state == OPCUAState.Running)
            {
                List<string> readarray = new List<string>();
                List<string> _nodes = new List<string>();



                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        records[i].dtValid = ReadDateTimeValue((NodeID + $"[{i}].DateTime"), out records[i].dtua);
                        records[i].idValid = ReadDIntValue((NodeID + $"[{i}].ID"), out records[i].idua);
                        records[i].valValid = ReadRealValue((NodeID + $"[{i}].Value"), out records[i].valua);
                        NLogger.logger.Trace(records[i].valua);
                        records[i].valid = (records[i].dtValid && records[i].idValid && records[i].valValid);
                    }

                    catch (Exception ex)
                    {
                        log.WriteEntry(serverName + " Read Record# " + i + "! Error:" + ex.Message);

                    }

                }

            }
            else
            {
                NLogger.logger.Trace($"Can't Read Array. Server isn't running:");
                log.WriteEntry(serverName + "Can't Read Array. Server isn't running:");

            }

        }


        private void GetDateTimeFromRecord_1(string str, Record record, int i)
        {
            if (str != null)
            {
                try
                {
                    var s = DateTime.Parse(str);
                    //s = s.Replace(':', '_');
                    //var dts = s.Split(' ');
                    //dts[0] = Convert.ToString(Convert.ToInt32(dts[0]);
                    //int year = 2000 + (Int32.Parse(dts[0]));
                    //int year = (Int32.Parse(dts[0]));
                    ////dts[1] = Convert.ToString(Convert.ToInt32(dts[1], 10), 16);
                    //int month = (Int32.Parse(dts[1]));
                    ////dts[2] = Convert.ToString(Convert.ToInt32(dts[2], 10), 16);
                    //int day = (Int32.Parse(dts[2]));
                    ////dts[3] = Convert.ToString(Convert.ToInt32(dts[3], 10), 16);
                    //int hour = (Int32.Parse(dts[3]));
                    ////dts[4] = Convert.ToString(Convert.ToInt32(dts[4], 10), 16);
                    //int min = (Int32.Parse(dts[4]));
                    ////dts[5] = Convert.ToString(Convert.ToInt32(dts[5], 10), 16);
                    //int sec = (Int32.Parse(dts[5]));
                    ////dts[6] = Convert.ToString(Convert.ToInt32(dts[6], 10), 16);
                    //int ms = (Int32.Parse(dts[6]) * 10);
                    //record.dtua = new DateTime(year, month, day, hour, min, sec, ms);
                    //record.dtValid = true;

                    record.dtua = s;
                    record.dtValid = true;
                }
                catch (Exception ex)
                {
                    record.dtua = new DateTime();
                    record.dtValid = false;

                    log.WriteEntry(serverName + " Convert DT Record# " + i + "! Error:" + ex.Message);
                }

            }
            else
            {
                record.dtua = new DateTime();
                record.dtValid = false;
                log.WriteEntry(serverName + " Convert DT Record# " + i + "! Error - str is null");
            }


        }

        private void GetDateTimeFromRecord(string str, Record record, int i)
        {
            if (str != null)
            {
                try
                {
                    string[] dts;
                    dts = str.Split(';');
                    dts[0] = Convert.ToString(Convert.ToInt32(dts[0], 10), 16);
                    int year = 2000 + (Int32.Parse(dts[0]));
                    dts[1] = Convert.ToString(Convert.ToInt32(dts[1], 10), 16);
                    int month = (Int32.Parse(dts[1]));
                    dts[2] = Convert.ToString(Convert.ToInt32(dts[2], 10), 16);
                    int day = (Int32.Parse(dts[2]));
                    dts[3] = Convert.ToString(Convert.ToInt32(dts[3], 10), 16);
                    int hour = (Int32.Parse(dts[3]));
                    dts[4] = Convert.ToString(Convert.ToInt32(dts[4], 10), 16);
                    int min = (Int32.Parse(dts[4]));
                    dts[5] = Convert.ToString(Convert.ToInt32(dts[5], 10), 16);
                    int sec = (Int32.Parse(dts[5]));
                    dts[6] = Convert.ToString(Convert.ToInt32(dts[6], 10), 16);
                    int ms = (Int32.Parse(dts[6]) * 10);
                    record.dtua = new DateTime(year, month, day, hour, min, sec, ms);
                    record.dtValid = true;
                }
                catch (Exception ex)
                {
                    record.dtua = new DateTime();
                    record.dtValid = false;

                    log.WriteEntry(serverName + " Convert DT Record# " + i + "! Error:" + ex.Message);
                }

            }
            else
            {
                record.dtua = new DateTime();
                record.dtValid = false;
                log.WriteEntry(serverName + " Convert DT Record# " + i + "! Error - str is null");
            }


        }

        private void GetIDFormRecord(string str, Record record, int i)
        {
            if (str != null)
            {
                try
                {

                    record.idua = Int32.Parse(str);
                    record.idValid = true;

                }
                catch (Exception ex)
                {
                    record.idua = 0;
                    record.idValid = false;

                    log.WriteEntry(serverName + " Convert ID Record# " + i + "! Error:" + ex.Message);
                }

            }
            else
            {
                record.idua = 0;
                record.idValid = false;
                log.WriteEntry(serverName + " Convert ID Record# " + i + "! Error - str is null");
            }

        }

        private void GetValFormRecord(string str, Record record, int i)
        {
            if (str != null)
            {
                try
                {
                    record.valua = float.Parse(str);
                    record.valValid = true;

                }
                catch (Exception ex)
                {
                    record.valua = 0;
                    record.valValid = false;
                    log.WriteEntry(serverName + " Convert Val Record# " + i + "! Error:" + ex.Message);

                }

            }
            else
            {
                record.valua = 0;
                record.valValid = false;
                log.WriteEntry(serverName + " Convert Val Record# " + i + "! Error - str is null");

            }

        }



        private void clientAPI_CertificateEvent(CertificateValidator cert, CertificateValidationEventArgs e)
        {
            try
            {
                //Search for the server's certificate in store; if found -> accept
                X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                X509CertificateCollection certCol = store.Certificates.Find(X509FindType.FindByThumbprint, e.Certificate.Thumbprint, true);
                store.Close();
                if (certCol.Capacity > 0)
                {
                    e.Accept = true;
                }

                //Show cert dialog if cert hasn't been accepted yet
                else
                {
                    if (!e.Accept)
                    {

                        try
                        {
                            store.Open(OpenFlags.ReadWrite);
                            store.Add(e.Certificate);
                            store.Close();
                            log.WriteEntry(Application.ProductName + ". Adding new certificate to store.");
                        }
                        catch
                        {
                            log.WriteEntry("Error add new certificate to store");
                        }
                    }
                }
            }
            catch
            {
                log.WriteEntry("Error read certificate from store");
            }
        }

        private void clientAPI_KeepAlive(Session sender, KeepAliveEventArgs e)
        {
            Quality = (OPCUAQuality)((int)e.CurrentState);
        }

    }
//---OPCUA---
}




