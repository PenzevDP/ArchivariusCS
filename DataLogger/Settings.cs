using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Reflection;
using System.Data;
using Tools;
using DataManager;
//--OPCUA---
using Opc.Ua;
//---

namespace Settings
{
    public class Crypt
    {
        public static byte[] Encrypt(byte[] data, string password)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            ICryptoTransform ct = sa.CreateEncryptor(
                (new PasswordDeriveBytes(password, null)).GetBytes(16),
                new byte[16]);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);

            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();

            return ms.ToArray();
        }

        public static string Encrypt(string data, string password)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(data), password));
        }

        static public byte[] Decrypt(byte[] data, string password)
        {
            BinaryReader br = new BinaryReader(InternalDecrypt(data, password));
            return br.ReadBytes((int)br.BaseStream.Length);
        }

        static public string Decrypt(string data, string password)
        {
            CryptoStream cs = InternalDecrypt(Convert.FromBase64String(data), password);
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }

        static CryptoStream InternalDecrypt(byte[] data, string password)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            ICryptoTransform ct = sa.CreateDecryptor(
                (new PasswordDeriveBytes(password, null)).GetBytes(16),
                new byte[16]);

            MemoryStream ms = new MemoryStream(data);
            return new CryptoStream(ms, ct, CryptoStreamMode.Read);
        }
    }

    /// <summary>
    /// Базовый класс для настроек, сохраняемых в xml файл
    /// </summary>
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = true)]
    public abstract class SettingsBase
    {
        /// <summary>
        /// Путь к файлу настроек
        /// </summary>
        [XmlIgnore] //Это поле не нужно сохранять в настройках
        public string Path { get; set; }

        /// <summary>
        /// Указатель на журнал событий
        /// </summary>
        [XmlIgnore] //Это поле не нужно сохранять в настройках
        private SingleEventLog Log { get; set; }

        /// <summary>
        /// При переопределении в наследуемом классе устанавливает значения по умолчанию.
        /// Значения по умолчанию используются если не найден файл настроек
        /// </summary>
        public virtual void SetDefaults()
        {
        }

        /// <summary>
        /// Выполняется при ошибке чтения в файле настроек
        /// </summary>
        /// <param name="ex">возникшая ошибка</param>
        protected virtual void OnException(Exception ex)
        {
            Log.WriteEntry("Configuration error! " + ex.Message);
        }

        /// <summary>
        /// Конструктор без параметров. 
        /// Необходим для того, чтобы можно было сериализовать данный класс
        /// </summary>
        public SettingsBase()
        {
            Path = "";
        }

        /// <summary>
        /// Конструктор выполняющий чтение из файла настроек.
        /// Если не найден файл или произошла ошибка при чтении, то используются настройки
        /// устанавливаемые в <see cref="SetDefaults"/>
        /// </summary>
        /// <param name="path">Путь к файлу настроек</param>
        public SettingsBase(string path, SingleEventLog log)
        {
            this.Path = path;
            this.Log = log;
        }

        /// <summary>
        /// Загрузка значений полей и свойств из файла
        /// </summary>
        private void Deserialize()
        {
            XmlSerializer xs = null;
            using (StreamReader sr = new StreamReader(this.Path))
            using (XmlTextReader xr = new XmlTextReader(sr))
            {
                xs = new XmlSerializer(this.GetType());
                object obj = xs.Deserialize(xr); //получаемый объект после десеарилизации

                //Копирует все св-ва и поля из объекта "obj" в текущий
                Type t = this.GetType();
                foreach (PropertyInfo p in t.GetProperties())
                {
                    
                    if (p.GetCustomAttributes(typeof(XmlIgnoreAttribute),
                        true).Length == 0)
                        p.SetValue(this, p.GetValue(obj, null), null);

                   
                }

               
                foreach (FieldInfo f in t.GetFields())                    
                {
                    if (f.GetCustomAttributes(typeof(XmlIgnoreAttribute),
                        true).Length == 0)
                        f.SetValue(this, f.GetValue(obj));

                   
                }
            }
        }

        /// <summary>
        /// Метод, выполняющий чтение из файла настроек.
        /// Если не найден файл или произошла ошибка при чтении, 
        /// то используются настройки устанавливаемые в <see cref="SetDefaults"/>
        /// </summary>
        /// <param name="path">Путь к файлу настроек</param>
        public virtual void Load()
        {
            if (File.Exists(Path))
            {
                try
                {
                    Deserialize();
                }
                catch (Exception ex)
                {
                    Log.WriteEntry("Error when loading configuration from the file " + Path);
                    SetDefaults();
                    OnException(ex);
                    return;
                }
                Log.WriteEntry("The configuration is loaded from file " + Path);
            }
            else
            {
                Log.WriteEntry("File " + Path + " not found. Empty configuration");
                SetDefaults();
            }
        }

        /// <summary>
        /// Метод, выполняющий сохранение настроек 
        /// в файл по адресу <see cref="Path"/>
        /// </summary>
        public virtual void Save()
        {
            try
            {
                StreamWriter sw = new StreamWriter(Path);
                XmlSerializer xs = new XmlSerializer(this.GetType());
                xs.Serialize(sw, this);
                sw.Close();
            }
            catch (Exception ex)
            {
                Log.WriteEntry("Error when saving configuration in the file " + Path);
                OnException(ex);
                return;
            }

            NLogger.logger.Trace("The configuration is saved in the file " + Path);
            Log.WriteEntry("The configuration is saved in the file " + Path);
        }
    }

    /// <summary>
    /// Файл настроек приложения, унаследован от SettingsBase
    /// </summary>
    public class AppSettings : SettingsBase
    {
        //---OPCUA---
        //public string Primary_OPC_Node;
        public string Primary_ODBC_DSN;
        public string Primary_ODBC_User;
        public string Primary_ODBC_Pass;
        public string Driver_Type;
        public DataSet TransactionBase;
        public int UpdateRate;
        public bool Running;

        //---OPCUA---
        public string Primary_OPCUA_EndpointURL;
        public string Primary_OPCUA_EndpointSecurityPolicyUri;
        public int Primary_OPCUA_EndpointSecurityMode;
        public string Primary_OPCUA_User;
        public string Primary_OPCUA_Pass;
        public bool Primary_OPCUA_LoginMode;
        public int KeepAliveInterval;
        //---

        private string Key = "Archivarius";

        public AppSettings(string path, SingleEventLog log)
            : base(path, log)
        {
            //Primary_OPC_Node = "";
            Primary_ODBC_DSN = "";
            Primary_ODBC_User = "";
            Primary_ODBC_Pass = null;
            Driver_Type = "";
            TransactionBase = null;
            UpdateRate = 0;
            Running = false;

            //---OPC UA---
           
            Primary_OPCUA_EndpointURL = "";
            Primary_OPCUA_EndpointSecurityPolicyUri = "";
            Primary_OPCUA_EndpointSecurityMode = 0;
            Primary_OPCUA_User = "";
            Primary_OPCUA_Pass = null;
            Primary_OPCUA_LoginMode=false;
            KeepAliveInterval=0;
            //---

        }

        public AppSettings()
                : base()
        {
            //Primary_OPC_Node = "";
            Primary_ODBC_DSN = "";
            Primary_ODBC_User = "";
            Primary_ODBC_Pass = null;
            TransactionBase = null;
            UpdateRate = 0;
            Running = false;

            //---OPC UA---
        
            Primary_OPCUA_EndpointURL = "";
            Primary_OPCUA_EndpointSecurityPolicyUri = "";
            Primary_OPCUA_EndpointSecurityMode = 0;
            Primary_OPCUA_User = "";
            Primary_OPCUA_Pass = null;
            Primary_OPCUA_LoginMode = false;
            KeepAliveInterval = 0;
            //---
        }

        private RSACryptoServiceProvider RSACryptoProvider()
        {
            // Creates the CspParameters object 
            // and sets the key container name used to store the RSA key pair
            CspParameters cspParams = new CspParameters();
            cspParams.KeyContainerName = Application.ProductName;

            // Instantiates the rsa instance accessing the key container 
            return new RSACryptoServiceProvider(cspParams);
        }

        public override void SetDefaults()
        {
            DataTable dt;
            DataColumn dc;

            // Ограничение уникальности на группу столбцов
            //DataColumn[] uniqueColumns = new DataColumn[2];
            DataColumn[] uniqueColumns = new DataColumn[2];


            //Primary_OPC_Node = "";
            Primary_ODBC_DSN = "";
            UpdateRate = 1000;
            Running = false;

            //---OPC UA---
            
            Primary_OPCUA_EndpointURL = "";
            Primary_OPCUA_EndpointSecurityPolicyUri = "";
            Primary_OPCUA_EndpointSecurityMode = 0;
            Primary_OPCUA_LoginMode = false;
            KeepAliveInterval = 1000;
            //Primary_OPCUA_User = "";
            //Primary_OPCUA_Pass = null;
            //---


            dt = new DataTable("TransactionTable");

            dc = new DataColumn("Transaction Name", System.Type.GetType("System.String"));
            dc.AllowDBNull = false;
            dc.Unique = true;
            dt.Columns.Add(dc);

       
            //---OPCUA---
            dc = new DataColumn("ns#", System.Type.GetType("System.Int32"));
            dc.AllowDBNull = false;
            dt.Columns.Add(dc);
            uniqueColumns[0] = dc;

            dc = new DataColumn("DBUA Name", System.Type.GetType("System.String"));
            dc.AllowDBNull = false;
            dt.Columns.Add(dc);
            uniqueColumns[1] = dc;

            dc = new DataColumn("SizeUA Name", System.Type.GetType("System.String"));
            dc.AllowDBNull = false;
            dt.Columns.Add(dc);

            dc = new DataColumn("CounterUA Name", System.Type.GetType("System.String"));
            dc.AllowDBNull = false;
            dt.Columns.Add(dc);

            dc = new DataColumn("ArrayUA Name", System.Type.GetType("System.String"));
            dc.AllowDBNull = false;
            dt.Columns.Add(dc);
            //--OPCUA----

            dc = new DataColumn("Table Name", System.Type.GetType("System.String"));
            dc.AllowDBNull = false;
            dt.Columns.Add(dc);

                       
            dc = new DataColumn("Transaction DT", System.Type.GetType("System.String"));
            dt.Columns.Add(dc);

            dc = new DataColumn("Controller DT", System.Type.GetType("System.String"));
            dt.Columns.Add(dc);

            dc = new DataColumn("Parameter ID", System.Type.GetType("System.String"));
            dt.Columns.Add(dc);

            dc = new DataColumn("Parameter Value", System.Type.GetType("System.String"));
            dc.AllowDBNull = false;
            dt.Columns.Add(dc);

            // Установка ограничей уникальности на группу столбцов
            dt.Constraints.Add(new UniqueConstraint(uniqueColumns));

            TransactionBase = new DataSet("Transaction Manager");
            TransactionBase.Tables.Add(dt);
        }

        public override void Load()
        {
            base.Load();
            if (Primary_ODBC_Pass != null)
            {
                if (Primary_ODBC_Pass != String.Empty)
                {
                    Primary_ODBC_Pass = Crypt.Decrypt(Primary_ODBC_Pass, Key);
                }
            }

            //---OPC UA----
            if (Primary_OPCUA_Pass != null)
            {
                if (Primary_OPCUA_Pass != String.Empty)
                {
                    Primary_OPCUA_Pass = Crypt.Decrypt(Primary_OPCUA_Pass, Key);
                }
            }
            //---OPC UA----
        }

        public override void Save()
        {
            string password = Primary_ODBC_Pass;
            //---OPCUA---
            string passwordUA = Primary_OPCUA_Pass;
            //---OPCUA---

            if (Primary_ODBC_User == "")
            {
                Primary_ODBC_Pass = "";
            }
            else
            {
                if (Primary_ODBC_Pass != null)
                {
                    Primary_ODBC_Pass = Crypt.Encrypt(password, Key);
                }
            }
            //---OPCUA---
            if (Primary_OPCUA_User == "")
            {
                Primary_OPCUA_Pass = "";
            }
            else
            {
                if (Primary_OPCUA_Pass != null)
                {
                    Primary_OPCUA_Pass = Crypt.Encrypt(passwordUA, Key);
                }
            }
            //---OPCUA---
            base.Save();

            Primary_ODBC_Pass = password;
            //---OPCUA---
            Primary_OPCUA_Pass = passwordUA;
            //---OPCUA---
        }
    }
}