using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using DataLogger.Properties;
using DataManager;
using Tools;

namespace DataLogger
{
    delegate void NewStatisticsCallback(Form form, DataTable dt, ConfigStatisticsEventArgs e);
    
    public partial class formMain : Form
    {
        private bool closing = false;
        
        public formMain()
        {
            InitializeComponent();
        }        

        private void frmMain_Load(object sender, EventArgs e)
        {
            hide();
            UpdateConfigState(Config.State);
            Config.StateChange += ConfigStateChange;
            Config.Statistics += ConfigStatistics;
        }

        private void ConfigStateChange(object sender, ConfigStateEventArgs e)
        {
            UpdateConfigState(e.State);
        }

        private void UpdateRowStatistics(DataRow row, ConfigStatisticsEventArgs e)
        {
            row["OPC"] = e.OPCUAConnState;
            // row["OPC"] = e.OPCConnState;
            row["ODBC"] = e.ODBCConnState;
            row["Total"] = e.TransactionStatistics.Total;
            row["Passed"] = e.TransactionStatistics.Passed;
            row["Failed"] = e.TransactionStatistics.Failed;
            row["% Passed"] = e.TransactionStatistics.Percent;
        }
        
        private void NewStatistics(Form form, DataTable dt, ConfigStatisticsEventArgs e)
        {
            if (form.InvokeRequired)
            {
                NewStatisticsCallback callback = new NewStatisticsCallback(NewStatistics);
                form.Invoke(callback, form, dt, e);
            }
            else
            {
                DataRow[] foundRows = dt.Select("Transaction = '" + e.TransactionName + "'");
                if (foundRows.Length > 0)
                {
                    foreach (DataRow row in foundRows) UpdateRowStatistics(row, e);
                }
                else
                {
                    DataRow row = dt.NewRow();
                    row["Transaction"] = e.TransactionName;
                    UpdateRowStatistics(row, e);
                    dt.Rows.Add(row);                    
                }
            }
        }
        
        private void ConfigStatistics(object sender, ConfigStatisticsEventArgs e)
        {
            NewStatistics(this, dtTransaction, e);
        }

        private void UpdateConfigState(ConfigState state)
        {
            trayNotifyIcon.Text = Application.ProductName + " - " + state.ToString();            

            SafeThread.SetTextStripItem(statusbar, statusConfigLabel, "Configuration state: " + state.ToString());
            switch (state)
            {
                case ConfigState.Starting:
                    trayNotifyIcon.Icon = Properties.Resources.servicepaused;
                    SafeThread.SetImageStripItem(statusbar, statusConfigLabel, Resources.Pause);
                    SafeThread.SetEnableStripItem(toolbar, startButton, false);
                    SafeThread.SetEnableStripItem(menu, startMenuItem, false);
                    SafeThread.SetEnableStripItem(trayMenu, startTrayMenuItem, false);
                    SafeThread.SetEnableStripItem(toolbar, stopButton, true);
                    SafeThread.SetEnableStripItem(menu, stopMenuItem, true);
                    SafeThread.SetEnableStripItem(trayMenu, stopTrayMenuItem, true);
                    break;
                case ConfigState.Started:
                    trayNotifyIcon.Icon = Properties.Resources.servicerunning;
                    SafeThread.SetImageStripItem(statusbar, statusConfigLabel, Resources.Run);
                    SafeThread.SetEnableStripItem(toolbar, startButton, false);
                    SafeThread.SetEnableStripItem(menu, startMenuItem, false);
                    SafeThread.SetEnableStripItem(trayMenu, startTrayMenuItem, false);
                    SafeThread.SetEnableStripItem(toolbar, stopButton, true);
                    SafeThread.SetEnableStripItem(menu, stopMenuItem, true);
                    SafeThread.SetEnableStripItem(trayMenu, stopTrayMenuItem, true);
                    break;
                case ConfigState.Stopping:
                    trayNotifyIcon.Icon = Properties.Resources.servicepaused;
                    SafeThread.SetImageStripItem(statusbar, statusConfigLabel, Resources.Pause);
                    SafeThread.SetEnableStripItem(toolbar, startButton, false);
                    SafeThread.SetEnableStripItem(menu, startMenuItem, false);
                    SafeThread.SetEnableStripItem(trayMenu, startTrayMenuItem, false);
                    SafeThread.SetEnableStripItem(toolbar, stopButton, false);
                    SafeThread.SetEnableStripItem(menu, stopMenuItem, false);
                    SafeThread.SetEnableStripItem(trayMenu, stopTrayMenuItem, false);
                    break;
                case ConfigState.Stopped:
                    trayNotifyIcon.Icon = Properties.Resources.servicestopped;
                    SafeThread.SetImageStripItem(statusbar, statusConfigLabel, Resources.Stop);
                    if (Config.Ready)
                    {
                        SafeThread.SetEnableStripItem(toolbar, startButton, true);
                        SafeThread.SetEnableStripItem(menu, startMenuItem, true);
                        SafeThread.SetEnableStripItem(trayMenu, startTrayMenuItem, true);
                    }
                    else
                    {
                        SafeThread.SetEnableStripItem(toolbar, startButton, false);
                        SafeThread.SetEnableStripItem(menu, startMenuItem, false);
                        SafeThread.SetEnableStripItem(trayMenu, startTrayMenuItem, false);
                    }
                    SafeThread.SetEnableStripItem(toolbar, stopButton, false);
                    SafeThread.SetEnableStripItem(menu, stopMenuItem, false);
                    SafeThread.SetEnableStripItem(trayMenu, stopTrayMenuItem, false);
                    break;
            }
        }

        

        //----OPCUA---
        private void defineOPCUA()
        {
            formDefineOPCUA form = new formDefineOPCUA();
            form.ShowDialog(this);
        }
        //----OPCUA---

        private void defineODBC()
        {
            formDefineODBC form = new formDefineODBC();
            form.ShowDialog(this);            
            checkStartPossibility();
        }

        private void defineTransaction()
        {
            formDefineTran form = new formDefineTran();
            form.ShowDialog(this);
            checkStartPossibility();
        }

        private void about()
        {
            formAbout form = new formAbout();
            form.ShowDialog(this);
           
        }

        private void checkStartPossibility()
        {
            if (!Config.Sets.Running)
            {
                if (Config.Ready)
                {
                    startButton.Enabled = true;
                    startMenuItem.Enabled = true;
                    startTrayMenuItem.Enabled = true;
                }
                else
                {
                    startButton.Enabled = false;
                    startMenuItem.Enabled = false;
                    startTrayMenuItem.Enabled = false;
                }
            }
        }

        private void start(bool saving)
        {
            Config.Start();
            if (saving) Config.Save();
        }

        private void stop(bool saving)
        {
            Config.Stop();
            if (saving) Config.Save();
        }

        private void exit()
        {
            DialogResult result = MessageBox.Show("Application will be closed. You sure?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                closing = true;
                Close();
            }
        }

        private void hide()
        {
            foreach (Form form in OwnedForms) form.Close();
            this.Hide();
            hideTrayMenuItem.Visible = false;
            restoreTrayMenuItem.Visible = true;
        }

        private void restore()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            hideTrayMenuItem.Visible = true;
            restoreTrayMenuItem.Visible = false;
        }


        private void odbcConnectorMenuItem_Click(object sender, EventArgs e)
        {
            defineODBC();
        }

        private void odbcConnectorButton_Click(object sender, EventArgs e)
        {
            defineODBC();
        }

        private void transactionMenuItem_Click(object sender, EventArgs e)
        {
            defineTransaction();
        }

        private void transactionButton_Click(object sender, EventArgs e)
        {
            defineTransaction();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            start(true);
        }

        private void startMenuItem_Click(object sender, EventArgs e)
        {
            start(true);
        }

        private void startTrayMenuItem_Click(object sender, EventArgs e)
        {
            start(true);
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            stop(true);
        }

        private void stopMenuItem_Click(object sender, EventArgs e)
        {
            stop(true);
        }

        private void stopTrayMenuItem_Click(object sender, EventArgs e)
        {
            stop(true);
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            exit();
        }

        private void exitTrayMenuItem_Click(object sender, EventArgs e)
        {
            exit();
        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            about();
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            about();
        }

        private void formMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                if (this.Visible) hide();
            }
        }

        private void trayNotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.Visible)
                {
                    hide();
                }
                else
                {
                    restore();
                }
            }
        }

        private void formMain_FormClosing(object sender, FormClosingEventArgs e)
        {           
            if (e.CloseReason == CloseReason.UserClosing && !closing)
            {
                if (this.Visible) hide();
                e.Cancel = true;
            }
            else
            {
                Config.Statistics -= ConfigStatistics;
                Config.StateChange -= ConfigStateChange;
                Config.Dispose();
                while (!Config.IsDisposed) Application.DoEvents();
            }
        }

        private void restoreTrayMenuItem_Click(object sender, EventArgs e)
        {
            restore();
        }

        private void hideTraMenuItem_Click(object sender, EventArgs e)
        {
            hide();
        }

        //---OPCUA----
        private void opcuaConnectorButton_Click(object sender, EventArgs e)
        {
            defineOPCUA();
        }

        private void opcuaConnectorMenuItem_Click(object sender, EventArgs e)
        {
            defineOPCUA();
        }
        
        //---OPCUA----
    }
}
