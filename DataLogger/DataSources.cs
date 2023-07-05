using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Data.Odbc;


namespace SQLDataSources
{
    public struct OdbcSource
    {
        public string ServerName;
        public string DriverName;
    }

    public static class OdbcWrapper
    {
        [DllImport("odbc32.dll")]
        /*public static extern int SQLDataSources(int EnvHandle, int Direction, StringBuilder ServerName, int ServerNameBufferLenIn,
        ref int ServerNameBufferLenOut, StringBuilder Driver, int DriverBufferLenIn, ref int DriverBufferLenOut);*/
        public static extern short SQLDataSources(IntPtr EnvironmentHandle, ushort Direction, StringBuilder ServerName, short BufferLength1,
            ref short NameLength1Ptr, StringBuilder Description, short BufferLength2, ref short NameLength2Ptr);
        [DllImport("odbc32.dll")]
        public static extern int SQLAllocEnv(ref int EnvHandle);
        [DllImport("odbc32.dll", CharSet = CharSet.Ansi)]
        public static extern short SQLAllocHandle(short HandleType, IntPtr InputHandle, out IntPtr OutputHandle);

        [DllImport("odbc32", CharSet = CharSet.Unicode)]
        public static extern short SQLSetEnvAttr(IntPtr envHandle, ushort attribute, IntPtr val, int stringLength);
        public static List<OdbcSource> ListODBCsources()
        {
            int envHandle = 0;
            const int SQL_FETCH_NEXT = 1;
            const int SQL_FETCH_FIRST = 2;
            const int SQL_FETCH_FIRST_USER = 31;
            const int SQL_FETCH_FIRST_SYSTEM = 32;
            const int SQL_SUCCESS = 0;
            const int SQL_ERROR = -1;
            const int SQL_HANDLE_ENV = 1;
            const int SQL_ATTR_ODBC_VERSION = 200;

            List<OdbcSource> sources = new List<OdbcSource>();
            try
            {
                IntPtr lhEnvIn = (IntPtr)0;
                IntPtr lhEnv = (IntPtr)0;
                //if (OdbcWrapper.SQLAllocEnv(ref envHandle) != -1)
                if (OdbcWrapper.SQLAllocHandle(SQL_HANDLE_ENV, lhEnvIn, out lhEnv) == SQL_SUCCESS)
                  
                {
                    int ret;
                    StringBuilder serverName = new StringBuilder(1024);
                    StringBuilder driverName = new StringBuilder(1024);
                    //int snLen = 0;
                    //int driverLen = 0;
                    short snLen = 0;
                    short driverLen = 0;
                    

                    OdbcWrapper.SQLSetEnvAttr(lhEnv, SQL_ATTR_ODBC_VERSION, (IntPtr)3, 0);
                    //ret = OdbcWrapper.SQLDataSources(envHandle, SQL_FETCH_FIRST_SYSTEM, serverName, serverName.Capacity, ref snLen,
                    //            driverName, driverName.Capacity, ref driverLen);
                    //ret = OdbcWrapper.SQLDataSources(envHandle, SQL_FETCH_FIRST_USER, serverName, serverName.Capacity, ref snLen,
                    //            driverName, driverName.Capacity, ref driverLen);
                    ret = OdbcWrapper.SQLDataSources(lhEnv, SQL_FETCH_FIRST, serverName, (short)serverName.Capacity, ref snLen, driverName, (short)driverName.Capacity, ref driverLen);
                    while (ret == SQL_SUCCESS)
                    {
                        OdbcSource source = new OdbcSource();
                        source.ServerName = serverName.ToString();
                        source.DriverName = driverName.ToString();
                        sources.Add(source);
                        ret = OdbcWrapper.SQLDataSources(lhEnv, SQL_FETCH_NEXT, serverName, (short)serverName.Capacity, ref snLen, driverName, (short)driverName.Capacity, ref driverLen);
                    }
                }
                // Sorting by server name
                sources.Sort(delegate (OdbcSource s1, OdbcSource s2) { return s1.ServerName.CompareTo(s2.ServerName); });
            }
            catch (Exception e)
            {
                DataManager.Config.Log.WriteEntry("Error while trying to find ODBC servers: " + e.Message);
            }

            return sources;
        }
    }
}