namespace Archivarius
{
    partial class formMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.configurationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defineMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opcuaConnectorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.odbcConnectorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transactionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.startButton = new System.Windows.Forms.ToolStripButton();
            this.stopButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.opcuaConnectorButton = new System.Windows.Forms.ToolStripButton();
            this.odbcConnectorButton = new System.Windows.Forms.ToolStripButton();
            this.transactionButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutButton = new System.Windows.Forms.ToolStripButton();
            this.dsMonitor = new System.Data.DataSet();
            this.dtTransaction = new System.Data.DataTable();
            this.dcTransactionName = new System.Data.DataColumn();
            this.dcOPCState = new System.Data.DataColumn();
            this.dcODBCState = new System.Data.DataColumn();
            this.dcTransactionTotal = new System.Data.DataColumn();
            this.dcTransactionPassed = new System.Data.DataColumn();
            this.dcTransactionFailed = new System.Data.DataColumn();
            this.dcPercent = new System.Data.DataColumn();
            this.statusbar = new System.Windows.Forms.StatusStrip();
            this.statusConfigLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.trayNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.hideTrayMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreTrayMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.startTrayMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopTrayMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitTrayMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.DB_transactions = new System.Windows.Forms.TabPage();
            this.OPCUA_transactions = new System.Windows.Forms.TabPage();
            this.dataGridMonitor = new System.Windows.Forms.DataGridView();
            this.transactionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oPCDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oDBCDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.passedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.failedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.passedDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewOPC = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menu.SuspendLayout();
            this.toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dsMonitor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTransaction)).BeginInit();
            this.statusbar.SuspendLayout();
            this.trayMenu.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.DB_transactions.SuspendLayout();
            this.OPCUA_transactions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMonitor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOPC)).BeginInit();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurationMenuItem,
            this.defineMenuItem,
            this.helpMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(557, 24);
            this.menu.TabIndex = 0;
            // 
            // configurationMenuItem
            // 
            this.configurationMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startMenuItem,
            this.stopMenuItem,
            this.toolStripMenuItem2,
            this.exitMenuItem});
            this.configurationMenuItem.Name = "configurationMenuItem";
            this.configurationMenuItem.Size = new System.Drawing.Size(93, 20);
            this.configurationMenuItem.Text = "Configuration";
            // 
            // startMenuItem
            // 
            this.startMenuItem.Enabled = false;
            this.startMenuItem.Image = global::Archivarius.Properties.Resources.PlayHS;
            this.startMenuItem.Name = "startMenuItem";
            this.startMenuItem.Size = new System.Drawing.Size(98, 22);
            this.startMenuItem.Text = "Start";
            this.startMenuItem.Click += new System.EventHandler(this.startMenuItem_Click);
            // 
            // stopMenuItem
            // 
            this.stopMenuItem.Enabled = false;
            this.stopMenuItem.Image = global::Archivarius.Properties.Resources.StopHS;
            this.stopMenuItem.Name = "stopMenuItem";
            this.stopMenuItem.Size = new System.Drawing.Size(98, 22);
            this.stopMenuItem.Text = "Stop";
            this.stopMenuItem.Click += new System.EventHandler(this.stopMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(95, 6);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(98, 22);
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // defineMenuItem
            // 
            this.defineMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.opcuaConnectorMenuItem,
            this.odbcConnectorMenuItem,
            this.transactionMenuItem});
            this.defineMenuItem.Name = "defineMenuItem";
            this.defineMenuItem.Size = new System.Drawing.Size(53, 20);
            this.defineMenuItem.Text = "Define";
            // 
            // opcuaConnectorMenuItem
            // 
            this.opcuaConnectorMenuItem.Image = global::Archivarius.Properties.Resources.OPCUA_database;
            this.opcuaConnectorMenuItem.Name = "opcuaConnectorMenuItem";
            this.opcuaConnectorMenuItem.Size = new System.Drawing.Size(173, 22);
            this.opcuaConnectorMenuItem.Text = "OPCUA Connector";
            this.opcuaConnectorMenuItem.Click += new System.EventHandler(this.opcuaConnectorMenuItem_Click);
            // 
            // odbcConnectorMenuItem
            // 
            this.odbcConnectorMenuItem.Image = global::Archivarius.Properties.Resources.VSProject_database;
            this.odbcConnectorMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.odbcConnectorMenuItem.Name = "odbcConnectorMenuItem";
            this.odbcConnectorMenuItem.Size = new System.Drawing.Size(173, 22);
            this.odbcConnectorMenuItem.Text = "ODBC Connector";
            this.odbcConnectorMenuItem.Click += new System.EventHandler(this.odbcConnectorMenuItem_Click);
            // 
            // transactionMenuItem
            // 
            this.transactionMenuItem.Image = global::Archivarius.Properties.Resources.Control_DataNavigator;
            this.transactionMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.transactionMenuItem.Name = "transactionMenuItem";
            this.transactionMenuItem.Size = new System.Drawing.Size(173, 22);
            this.transactionMenuItem.Text = "Transaction";
            this.transactionMenuItem.Click += new System.EventHandler(this.transactionMenuItem_Click);
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutMenuItem});
            this.helpMenuItem.Name = "helpMenuItem";
            this.helpMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpMenuItem.Text = "Help";
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Image = global::Archivarius.Properties.Resources.Help;
            this.aboutMenuItem.Name = "aboutMenuItem";
            this.aboutMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.aboutMenuItem.Size = new System.Drawing.Size(126, 22);
            this.aboutMenuItem.Text = "About";
            this.aboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
            // 
            // toolbar
            // 
            this.toolbar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startButton,
            this.stopButton,
            this.toolStripSeparator1,
            this.opcuaConnectorButton,
            this.odbcConnectorButton,
            this.transactionButton,
            this.toolStripSeparator2,
            this.aboutButton});
            this.toolbar.Location = new System.Drawing.Point(0, 24);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(557, 27);
            this.toolbar.TabIndex = 2;
            // 
            // startButton
            // 
            this.startButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.startButton.Enabled = false;
            this.startButton.Image = global::Archivarius.Properties.Resources.PlayHS;
            this.startButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(24, 24);
            this.startButton.Text = "Start configuration";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stopButton.Enabled = false;
            this.stopButton.Image = global::Archivarius.Properties.Resources.StopHS;
            this.stopButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(24, 24);
            this.stopButton.Text = "Stop configuration";
            this.stopButton.ToolTipText = "Stop configuration";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // opcuaConnectorButton
            // 
            this.opcuaConnectorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.opcuaConnectorButton.Image = global::Archivarius.Properties.Resources.OPCUA_database;
            this.opcuaConnectorButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.opcuaConnectorButton.Name = "opcuaConnectorButton";
            this.opcuaConnectorButton.Size = new System.Drawing.Size(24, 24);
            this.opcuaConnectorButton.Text = "Define OPCUA Connector";
            this.opcuaConnectorButton.Click += new System.EventHandler(this.opcuaConnectorButton_Click);
            // 
            // odbcConnectorButton
            // 
            this.odbcConnectorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.odbcConnectorButton.Image = global::Archivarius.Properties.Resources.VSProject_database;
            this.odbcConnectorButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.odbcConnectorButton.Name = "odbcConnectorButton";
            this.odbcConnectorButton.Size = new System.Drawing.Size(24, 24);
            this.odbcConnectorButton.Text = "Define ODBC Connector";
            this.odbcConnectorButton.ToolTipText = "Define ODBC Connector";
            this.odbcConnectorButton.Click += new System.EventHandler(this.odbcConnectorButton_Click);
            // 
            // transactionButton
            // 
            this.transactionButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.transactionButton.Image = global::Archivarius.Properties.Resources.Control_DataNavigator;
            this.transactionButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.transactionButton.Name = "transactionButton";
            this.transactionButton.Size = new System.Drawing.Size(24, 24);
            this.transactionButton.Text = "Define transaction";
            this.transactionButton.ToolTipText = "Define transaction";
            this.transactionButton.Click += new System.EventHandler(this.transactionButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // aboutButton
            // 
            this.aboutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.aboutButton.Image = global::Archivarius.Properties.Resources.Help;
            this.aboutButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.Size = new System.Drawing.Size(24, 24);
            this.aboutButton.Text = "About";
            this.aboutButton.ToolTipText = "About";
            this.aboutButton.Click += new System.EventHandler(this.aboutButton_Click);
            // 
            // dsMonitor
            // 
            this.dsMonitor.DataSetName = "NewDataSet";
            this.dsMonitor.Tables.AddRange(new System.Data.DataTable[] {
            this.dtTransaction});
            // 
            // dtTransaction
            // 
            this.dtTransaction.Columns.AddRange(new System.Data.DataColumn[] {
            this.dcTransactionName,
            this.dcOPCState,
            this.dcODBCState,
            this.dcTransactionTotal,
            this.dcTransactionPassed,
            this.dcTransactionFailed,
            this.dcPercent});
            this.dtTransaction.TableName = "dtTransaction";
            // 
            // dcTransactionName
            // 
            this.dcTransactionName.Caption = "Transaction Name";
            this.dcTransactionName.ColumnName = "Transaction";
            // 
            // dcOPCState
            // 
            this.dcOPCState.ColumnName = "OPC";
            // 
            // dcODBCState
            // 
            this.dcODBCState.ColumnName = "ODBC";
            // 
            // dcTransactionTotal
            // 
            this.dcTransactionTotal.ColumnName = "Total";
            this.dcTransactionTotal.DataType = typeof(long);
            // 
            // dcTransactionPassed
            // 
            this.dcTransactionPassed.ColumnName = "Passed";
            this.dcTransactionPassed.DataType = typeof(long);
            // 
            // dcTransactionFailed
            // 
            this.dcTransactionFailed.ColumnName = "Failed";
            this.dcTransactionFailed.DataType = typeof(long);
            // 
            // dcPercent
            // 
            this.dcPercent.ColumnName = "% Passed";
            this.dcPercent.DataType = typeof(float);
            // 
            // statusbar
            // 
            this.statusbar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusConfigLabel});
            this.statusbar.Location = new System.Drawing.Point(0, 494);
            this.statusbar.Name = "statusbar";
            this.statusbar.Size = new System.Drawing.Size(557, 27);
            this.statusbar.TabIndex = 4;
            // 
            // statusConfigLabel
            // 
            this.statusConfigLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.statusConfigLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.statusConfigLabel.Image = global::Archivarius.Properties.Resources.Stop;
            this.statusConfigLabel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.statusConfigLabel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.statusConfigLabel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.statusConfigLabel.Name = "statusConfigLabel";
            this.statusConfigLabel.Padding = new System.Windows.Forms.Padding(15, 2, 0, 3);
            this.statusConfigLabel.Size = new System.Drawing.Size(144, 24);
            this.statusConfigLabel.Text = "Configuration state";
            // 
            // trayNotifyIcon
            // 
            this.trayNotifyIcon.ContextMenuStrip = this.trayMenu;
            this.trayNotifyIcon.Visible = true;
            this.trayNotifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.trayNotifyIcon_MouseClick);
            // 
            // trayMenu
            // 
            this.trayMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hideTrayMenuItem,
            this.restoreTrayMenuItem,
            this.toolStripSeparator3,
            this.startTrayMenuItem,
            this.stopTrayMenuItem,
            this.toolStripSeparator4,
            this.exitTrayMenuItem});
            this.trayMenu.Name = "trayContextMenu";
            this.trayMenu.Size = new System.Drawing.Size(118, 146);
            // 
            // hideTrayMenuItem
            // 
            this.hideTrayMenuItem.Name = "hideTrayMenuItem";
            this.hideTrayMenuItem.Size = new System.Drawing.Size(117, 26);
            this.hideTrayMenuItem.Text = "Hide";
            this.hideTrayMenuItem.Click += new System.EventHandler(this.hideTraMenuItem_Click);
            // 
            // restoreTrayMenuItem
            // 
            this.restoreTrayMenuItem.Name = "restoreTrayMenuItem";
            this.restoreTrayMenuItem.Size = new System.Drawing.Size(117, 26);
            this.restoreTrayMenuItem.Text = "Restore";
            this.restoreTrayMenuItem.Click += new System.EventHandler(this.restoreTrayMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(114, 6);
            // 
            // startTrayMenuItem
            // 
            this.startTrayMenuItem.Image = global::Archivarius.Properties.Resources.PlayHS;
            this.startTrayMenuItem.Name = "startTrayMenuItem";
            this.startTrayMenuItem.Size = new System.Drawing.Size(117, 26);
            this.startTrayMenuItem.Text = "Start";
            this.startTrayMenuItem.Click += new System.EventHandler(this.startTrayMenuItem_Click);
            // 
            // stopTrayMenuItem
            // 
            this.stopTrayMenuItem.Image = global::Archivarius.Properties.Resources.StopHS;
            this.stopTrayMenuItem.Name = "stopTrayMenuItem";
            this.stopTrayMenuItem.Size = new System.Drawing.Size(117, 26);
            this.stopTrayMenuItem.Text = "Stop";
            this.stopTrayMenuItem.Click += new System.EventHandler(this.stopTrayMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(114, 6);
            // 
            // exitTrayMenuItem
            // 
            this.exitTrayMenuItem.Name = "exitTrayMenuItem";
            this.exitTrayMenuItem.Size = new System.Drawing.Size(117, 26);
            this.exitTrayMenuItem.Text = "Exit";
            this.exitTrayMenuItem.Click += new System.EventHandler(this.exitTrayMenuItem_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(461, 20);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(83, 31);
            this.button2.TabIndex = 7;
            this.button2.Text = "Test Oracle";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.DB_transactions);
            this.tabControl1.Controls.Add(this.OPCUA_transactions);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControl1.Location = new System.Drawing.Point(0, 51);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(557, 443);
            this.tabControl1.TabIndex = 8;
            // 
            // DB_transactions
            // 
            this.DB_transactions.Controls.Add(this.dataGridMonitor);
            this.DB_transactions.Location = new System.Drawing.Point(4, 22);
            this.DB_transactions.Name = "DB_transactions";
            this.DB_transactions.Padding = new System.Windows.Forms.Padding(3);
            this.DB_transactions.Size = new System.Drawing.Size(549, 417);
            this.DB_transactions.TabIndex = 0;
            this.DB_transactions.Text = "DB_transactions";
            this.DB_transactions.UseVisualStyleBackColor = true;
            // 
            // OPCUA_transactions
            // 
            this.OPCUA_transactions.Controls.Add(this.dataGridViewOPC);
            this.OPCUA_transactions.Location = new System.Drawing.Point(4, 22);
            this.OPCUA_transactions.Name = "OPCUA_transactions";
            this.OPCUA_transactions.Padding = new System.Windows.Forms.Padding(3);
            this.OPCUA_transactions.Size = new System.Drawing.Size(549, 417);
            this.OPCUA_transactions.TabIndex = 1;
            this.OPCUA_transactions.Text = "OPCUA_transactions";
            this.OPCUA_transactions.UseVisualStyleBackColor = true;
            // 
            // dataGridMonitor
            // 
            this.dataGridMonitor.AllowUserToAddRows = false;
            this.dataGridMonitor.AllowUserToDeleteRows = false;
            this.dataGridMonitor.AutoGenerateColumns = false;
            this.dataGridMonitor.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridMonitor.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridMonitor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridMonitor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.transactionDataGridViewTextBoxColumn,
            this.oPCDataGridViewTextBoxColumn,
            this.oDBCDataGridViewTextBoxColumn,
            this.totalDataGridViewTextBoxColumn,
            this.passedDataGridViewTextBoxColumn,
            this.failedDataGridViewTextBoxColumn,
            this.passedDataGridViewTextBoxColumn1});
            this.dataGridMonitor.DataMember = "dtTransaction";
            this.dataGridMonitor.DataSource = this.dsMonitor;
            this.dataGridMonitor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridMonitor.Location = new System.Drawing.Point(3, 3);
            this.dataGridMonitor.Name = "dataGridMonitor";
            this.dataGridMonitor.ReadOnly = true;
            this.dataGridMonitor.RowHeadersWidth = 51;
            this.dataGridMonitor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridMonitor.Size = new System.Drawing.Size(543, 411);
            this.dataGridMonitor.TabIndex = 4;
            // 
            // transactionDataGridViewTextBoxColumn
            // 
            this.transactionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.transactionDataGridViewTextBoxColumn.DataPropertyName = "Transaction";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.transactionDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.transactionDataGridViewTextBoxColumn.HeaderText = "Transaction";
            this.transactionDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.transactionDataGridViewTextBoxColumn.Name = "transactionDataGridViewTextBoxColumn";
            this.transactionDataGridViewTextBoxColumn.ReadOnly = true;
            this.transactionDataGridViewTextBoxColumn.Width = 88;
            // 
            // oPCDataGridViewTextBoxColumn
            // 
            this.oPCDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.oPCDataGridViewTextBoxColumn.DataPropertyName = "OPC";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.oPCDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.oPCDataGridViewTextBoxColumn.HeaderText = "OPC";
            this.oPCDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.oPCDataGridViewTextBoxColumn.Name = "oPCDataGridViewTextBoxColumn";
            this.oPCDataGridViewTextBoxColumn.ReadOnly = true;
            this.oPCDataGridViewTextBoxColumn.Width = 54;
            // 
            // oDBCDataGridViewTextBoxColumn
            // 
            this.oDBCDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.oDBCDataGridViewTextBoxColumn.DataPropertyName = "ODBC";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.oDBCDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.oDBCDataGridViewTextBoxColumn.HeaderText = "ODBC";
            this.oDBCDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.oDBCDataGridViewTextBoxColumn.Name = "oDBCDataGridViewTextBoxColumn";
            this.oDBCDataGridViewTextBoxColumn.ReadOnly = true;
            this.oDBCDataGridViewTextBoxColumn.Width = 62;
            // 
            // totalDataGridViewTextBoxColumn
            // 
            this.totalDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.totalDataGridViewTextBoxColumn.DataPropertyName = "Total";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N0";
            dataGridViewCellStyle5.NullValue = null;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.totalDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.totalDataGridViewTextBoxColumn.HeaderText = "Total";
            this.totalDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.totalDataGridViewTextBoxColumn.Name = "totalDataGridViewTextBoxColumn";
            this.totalDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalDataGridViewTextBoxColumn.Width = 56;
            // 
            // passedDataGridViewTextBoxColumn
            // 
            this.passedDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.passedDataGridViewTextBoxColumn.DataPropertyName = "Passed";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N0";
            dataGridViewCellStyle6.NullValue = null;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.passedDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.passedDataGridViewTextBoxColumn.HeaderText = "Passed";
            this.passedDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.passedDataGridViewTextBoxColumn.Name = "passedDataGridViewTextBoxColumn";
            this.passedDataGridViewTextBoxColumn.ReadOnly = true;
            this.passedDataGridViewTextBoxColumn.Width = 67;
            // 
            // failedDataGridViewTextBoxColumn
            // 
            this.failedDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.failedDataGridViewTextBoxColumn.DataPropertyName = "Failed";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N0";
            dataGridViewCellStyle7.NullValue = null;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.failedDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle7;
            this.failedDataGridViewTextBoxColumn.HeaderText = "Failed";
            this.failedDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.failedDataGridViewTextBoxColumn.Name = "failedDataGridViewTextBoxColumn";
            this.failedDataGridViewTextBoxColumn.ReadOnly = true;
            this.failedDataGridViewTextBoxColumn.Width = 60;
            // 
            // passedDataGridViewTextBoxColumn1
            // 
            this.passedDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.passedDataGridViewTextBoxColumn1.DataPropertyName = "% Passed";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N2";
            dataGridViewCellStyle8.NullValue = null;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.passedDataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle8;
            this.passedDataGridViewTextBoxColumn1.HeaderText = "% Passed";
            this.passedDataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.passedDataGridViewTextBoxColumn1.Name = "passedDataGridViewTextBoxColumn1";
            this.passedDataGridViewTextBoxColumn1.ReadOnly = true;
            this.passedDataGridViewTextBoxColumn1.Width = 78;
            // 
            // dataGridViewOPC
            // 
            this.dataGridViewOPC.AllowUserToAddRows = false;
            this.dataGridViewOPC.AllowUserToDeleteRows = false;
            this.dataGridViewOPC.AutoGenerateColumns = false;
            this.dataGridViewOPC.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewOPC.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridViewOPC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOPC.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7});
            this.dataGridViewOPC.DataMember = "dtTransaction";
            this.dataGridViewOPC.DataSource = this.dsMonitor;
            this.dataGridViewOPC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewOPC.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewOPC.Name = "dataGridViewOPC";
            this.dataGridViewOPC.ReadOnly = true;
            this.dataGridViewOPC.RowHeadersWidth = 51;
            this.dataGridViewOPC.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewOPC.Size = new System.Drawing.Size(543, 411);
            this.dataGridViewOPC.TabIndex = 4;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Transaction";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridViewTextBoxColumn1.HeaderText = "Transaction";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 88;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "OPC";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridViewTextBoxColumn2.HeaderText = "OPC";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 54;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "ODBC";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridViewTextBoxColumn3.HeaderText = "ODBC";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 62;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Total";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle13.Format = "N0";
            dataGridViewCellStyle13.NullValue = null;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle13;
            this.dataGridViewTextBoxColumn4.HeaderText = "Total";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 56;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Passed";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle14.Format = "N0";
            dataGridViewCellStyle14.NullValue = null;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridViewTextBoxColumn5.HeaderText = "Passed";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 67;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Failed";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle15.Format = "N0";
            dataGridViewCellStyle15.NullValue = null;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle15;
            this.dataGridViewTextBoxColumn6.HeaderText = "Failed";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 60;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn7.DataPropertyName = "% Passed";
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle16.Format = "N2";
            dataGridViewCellStyle16.NullValue = null;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn7.DefaultCellStyle = dataGridViewCellStyle16;
            this.dataGridViewTextBoxColumn7.HeaderText = "% Passed";
            this.dataGridViewTextBoxColumn7.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 78;
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 521);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.statusbar);
            this.Controls.Add(this.toolbar);
            this.Controls.Add(this.menu);
            this.MainMenuStrip = this.menu;
            this.Name = "formMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Archivarius CS";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.formMain_Resize);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dsMonitor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTransaction)).EndInit();
            this.statusbar.ResumeLayout(false);
            this.statusbar.PerformLayout();
            this.trayMenu.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.DB_transactions.ResumeLayout(false);
            this.OPCUA_transactions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMonitor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOPC)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStrip toolbar;
        private System.Windows.Forms.ToolStripMenuItem configurationMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defineMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem odbcConnectorMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transactionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripStatusLabel statusConfigLabel;
        private System.Windows.Forms.StatusStrip statusbar;
        private System.Windows.Forms.NotifyIcon trayNotifyIcon;
        private System.Windows.Forms.ToolStripButton startButton;
        private System.Windows.Forms.ToolStripButton stopButton;
        private System.Windows.Forms.ToolStripButton odbcConnectorButton;
        private System.Windows.Forms.ToolStripButton transactionButton;
        private System.Windows.Forms.ToolStripButton aboutButton;
        private System.Windows.Forms.ContextMenuStrip trayMenu;
        private System.Windows.Forms.ToolStripMenuItem startTrayMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopTrayMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitTrayMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreTrayMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideTrayMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Data.DataSet dsMonitor;
        private System.Data.DataTable dtTransaction;
        private System.Data.DataColumn dcTransactionName;
        private System.Data.DataColumn dcOPCState;
        private System.Data.DataColumn dcODBCState;
        private System.Data.DataColumn dcTransactionTotal;
        private System.Data.DataColumn dcTransactionPassed;
        private System.Data.DataColumn dcTransactionFailed;
        private System.Data.DataColumn dcPercent;
        private System.Windows.Forms.ToolStripButton opcuaConnectorButton;
        private System.Windows.Forms.ToolStripMenuItem opcuaConnectorMenuItem;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage DB_transactions;
        private System.Windows.Forms.DataGridView dataGridMonitor;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oPCDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oDBCDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn passedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn failedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn passedDataGridViewTextBoxColumn1;
        private System.Windows.Forms.TabPage OPCUA_transactions;
        private System.Windows.Forms.DataGridView dataGridViewOPC;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
    }
}

