using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using DataManager;
using SQLDataSources;
using Tools;

namespace DataLogger
{
    public partial class formDefineODBC : Form
    {
        private List<OdbcSource> sources;

        public formDefineODBC()

        {
            NLogger.logger.Trace("Service. formDefineODBC has initialized");
            InitializeComponent();
        }

        private void UpdateConfigState(ConfigState state)
        {
            if (state == ConfigState.Stopped)
            {
                SafeThread.SetEnableControl(btnApply, true);
                SafeThread.SetEnableControl(btnTest, true);
                SafeThread.SetEnableControl(comboBoxDSN, true);
                SafeThread.SetEnableControl(textBoxUserName, true);
                SafeThread.SetEnableControl(textBoxPassword, true);
            }
            else
            {
                SafeThread.SetEnableControl(btnApply, false);
                SafeThread.SetEnableControl(btnTest, false);
                SafeThread.SetEnableControl(comboBoxDSN, false);
                SafeThread.SetEnableControl(textBoxUserName, false);
                SafeThread.SetEnableControl(textBoxPassword, false);
            }
        }

        private void frmDefineODBC_Load(object sender, EventArgs e)
        {
            sources = OdbcWrapper.ListODBCsources();

            int selectedIndex = -1;
            foreach (OdbcSource dsn in sources)
            {
                int index = comboBoxDSN.Items.Add(dsn.ServerName);
                if (dsn.ServerName == Config.Sets.Primary_ODBC_DSN) selectedIndex = index;
            }

            if (selectedIndex >= 0)
            {
                comboBoxDSN.SelectedIndex = selectedIndex;
            }
            else
            {
                comboBoxDSN.Text = Config.Sets.Primary_ODBC_DSN;
            }

            textBoxUserName.Text = Config.Sets.Primary_ODBC_User;
            textBoxPassword.Text = Config.Sets.Primary_ODBC_Pass;

            UpdateConfigState(Config.State);
            Config.StateChange += ConfigStateChange;
        }

        private void ConfigStateChange(object sender, ConfigStateEventArgs e)
        {
            UpdateConfigState(e.State);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (comboBoxDSN.SelectedIndex >= 0)
            {
                Config.Sets.Primary_ODBC_DSN = sources[comboBoxDSN.SelectedIndex].ServerName;
            }
            if (textBoxUserName.Text != "")
            {
                Config.Sets.Primary_ODBC_User = textBoxUserName.Text;
                Config.Sets.Primary_ODBC_Pass = textBoxPassword.Text;
            }
            else
            {
                Config.Sets.Primary_ODBC_User = "";
                Config.Sets.Primary_ODBC_Pass = "";
            }
            Config.Save();
            this.Close();
        }

        private void btnOdbc_Click(object sender, EventArgs e)
        {
            try
            {
                NLogger.logger.Trace("Service. formDefineODBC  ODBC button click");
                Process process = new Process();
                process.StartInfo.FileName = "odbcad32.exe";
                process.Start();
            }
            catch
            {
                return;
            }

            this.Close();
            NLogger.logger.Trace("Service. formDefineODBC closed");
        }

        private void comboBoxDSN_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            textBoxDriverName.Text = sources[comboBox.SelectedIndex].DriverName;
        }

        private void comboBoxDSN_TextUpdate(object sender, EventArgs e)
        {
            textBoxDriverName.Text = "";
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (textBoxDriverName.Text != "")
            {
                using (new WaitCursor())
                {
                    Exception exception = ODBCConnector.TestConnection(comboBoxDSN.Text, textBoxUserName.Text, textBoxPassword.Text);
                    if (exception == null)
                    {
                        MessageBox.Show("Success", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(exception.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
