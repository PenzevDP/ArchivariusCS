using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using DataLogger.Properties;
using DataManager;
//using SQLDataSources;
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
            NLogger.logger.Trace($"Service. formMain has initialized");
        }        

        private void frmMain_Load(object sender, EventArgs e)
        {
            hide();
            NLogger.logger.Trace($"Service. formMain hide method has done");
            UpdateConfigState(Config.State);
            Config.StateChange += ConfigStateChange;
            Config.Statistics += ConfigStatistics;
        }

        private void ConfigStateChange(object sender, ConfigStateEventArgs e)
        {
            UpdateConfigState(e.State);
            NLogger.logger.Trace("Service. formMain config state has changet to: {state}" ,  e.State);
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
            NLogger.logger.Trace("Service. formMain updated statistic: OPC - {OPC}, ODBC - {ODBC}, Total - {total}, Passed - {passed}, Failed - {failed}", e.OPCUAConnState, e.ODBCConnState, e.TransactionStatistics.Total, e.TransactionStatistics.Passed, e.TransactionStatistics.Failed);
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

            SafeThread.SetTextStripItem(statusbar, statusConfigLabel, "Config state is: " + state.ToString());
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

        private void testSQLOracle()
        {
            NLogger.logger.Trace("Service. formTestOracle has called");
            formTestOracle form = new formTestOracle();
            form.ShowDialog(this);
            
        }

        //----OPCUA---
        private void defineOPCUA()
        {
            NLogger.logger.Trace("Service. formDefineOPCUA has called");

            formDefineOPCUA form = new formDefineOPCUA();
            form.ShowDialog(this);
        }
        //----OPCUA---

        private void defineODBC()
        {
            NLogger.logger.Trace("Service. formDefineODBC has called");
            formDefineODBC form = new formDefineODBC();
            form.ShowDialog(this);            
            checkStartPossibility();
        }

        private void defineTransaction()
        {
            NLogger.logger.Trace("Service. formDefineTran has called");
            formDefineTran form = new formDefineTran();
            form.ShowDialog(this);
            checkStartPossibility();
        }

        private void about()
        {
            NLogger.logger.Trace("Service. formAbout has called");
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
            NLogger.logger.Trace("Service. formMain. Action - Start");
            Config.Start();
            if (saving) Config.Save();
        }

        private void stop(bool saving)
        {
            NLogger.logger.Trace("Service. formMain. Action - Stop");
            Config.Stop();
            if (saving) Config.Save();
        }

        private void exit()
        {
            NLogger.logger.Trace("Service. formMain try to EXIT");
            DialogResult result = MessageBox.Show("Application will be closed. You sure?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                closing = true;
                Close();
                NLogger.logger.Trace("Service. formMain has Closed");
            }
        }

        private void hide()
        {
            foreach (Form form in OwnedForms) form.Close();
            this.Hide();
            hideTrayMenuItem.Visible = false;
            restoreTrayMenuItem.Visible = true;
            NLogger.logger.Trace("Service. formMain has moved to TRAY");
        }

        private void restore()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            hideTrayMenuItem.Visible = true;
            restoreTrayMenuItem.Visible = false;
            NLogger.logger.Trace("Service. formMain has restored from TRAY");
        }


        private void odbcConnectorMenuItem_Click(object sender, EventArgs e)
        {
            NLogger.logger.Trace("Service. formMain ODBC click");
            defineODBC();
        }

        private void odbcConnectorButton_Click(object sender, EventArgs e)
        {
            NLogger.logger.Trace("Service. formMain ODBC button click");
            defineODBC();
        }

        private void transactionMenuItem_Click(object sender, EventArgs e)
        {
            NLogger.logger.Trace("Service. formMain transaction menu click");
            defineTransaction();
        }

        private void transactionButton_Click(object sender, EventArgs e)
        {
            NLogger.logger.Trace("Service. formMain transaction button click");
            defineTransaction();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            NLogger.logger.Trace("Service. formMain start button click");
            start(true);
        }

        private void startMenuItem_Click(object sender, EventArgs e)
        {
            NLogger.logger.Trace("Service. formMain startmenu click");
            start(true);
        }

        private void startTrayMenuItem_Click(object sender, EventArgs e)
        {
            NLogger.logger.Trace("Service. formMain tray start button click");
            start(true);
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            NLogger.logger.Trace("Service. formMain stop button click");
            stop(true);
        }

        private void stopMenuItem_Click(object sender, EventArgs e)
        {
            NLogger.logger.Trace("Service. formMain stoptmenu click");
            stop(true);
        }

        private void stopTrayMenuItem_Click(object sender, EventArgs e)
        {
            NLogger.logger.Trace("Service. formMain tray stop button click");
            stop(true);
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            NLogger.logger.Trace("Service. formMain exit menu click");
            exit();
        }

        private void exitTrayMenuItem_Click(object sender, EventArgs e)
        {
            NLogger.logger.Trace("Service. formMain tray exit button click");
            exit();
        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            NLogger.logger.Trace("Service. formMain about menu click");
            about();
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            NLogger.logger.Trace("Service. formMain about button click");
            about();
        }

        private void formMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                if (this.Visible) hide();
                NLogger.logger.Trace("Service. formMain Minimised = hiden");
            }
        }

        private void trayNotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.Visible)
                {
                    hide();
                    NLogger.logger.Trace("Service. formMain tray icon click = hide");
                    
                }
                else
                {
                    restore();
                    NLogger.logger.Trace("Service. formMain tray icon click = maximize");
                }
            }
        }

        private void formMain_FormClosing(object sender, FormClosingEventArgs e)
        {           
            if (e.CloseReason == CloseReason.UserClosing && !closing)
            {
                if (this.Visible) hide();
                e.Cancel = true;
                NLogger.logger.Trace("Service. formMain close = hide");
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
            NLogger.logger.Trace("Service. formMain restore tray button click");
        }

        private void hideTraMenuItem_Click(object sender, EventArgs e)
        {
            hide();
            NLogger.logger.Trace("Service. formMain hide tray button click");
        }

        //---OPCUA----
        private void opcuaConnectorButton_Click(object sender, EventArgs e)
        {
            NLogger.logger.Trace("Service. formMain define OPC button click");
            defineOPCUA();
            
        }

        private void opcuaConnectorMenuItem_Click(object sender, EventArgs e)
        {
            NLogger.logger.Trace("Service. formMain define OPC menu click");
            defineOPCUA();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            testSQLOracle();
        }

        //---OPCUA----
    }
}
