using System;
using System.Windows.Forms;
using Siemens.UAClientHelper;
using Siemens.OpcUA;
using Opc.Ua.Client;
using Opc.Ua;
using DataManager;
using Tools;
using Siemens.OpcUA.Client;

namespace DataLogger
{
    partial class formDefineOPCUA
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

       
        private void InitializeComponent()
        {
            Siemens.UAClientHelper.UAClientHelperAPI uaClientHelperAPI1 = new Siemens.UAClientHelper.UAClientHelperAPI();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.browseControl = new Siemens.OpcUA.Client.BrowseControl();
            this.attributeListControl = new Siemens.OpcUA.Client.AttributeListControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.nudOPCUAKeepAliveInterval = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.pwTextBox = new System.Windows.Forms.TextBox();
            this.nudOPCUAUpdateRate = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.userTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnUserPwd = new System.Windows.Forms.RadioButton();
            this.btnUserAnon = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGetEndpoints = new System.Windows.Forms.Button();
            this.endpointListView = new System.Windows.Forms.ListView();
            this.Endpoints = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.discoveryTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOPCUAKeepAliveInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOPCUAUpdateRate)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(8, 7);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(787, 574);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "OPCUA";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.browseControl);
            this.groupBox3.Controls.Add(this.attributeListControl);
            this.groupBox3.Location = new System.Drawing.Point(5, 308);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Size = new System.Drawing.Size(763, 257);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Endpoint Attributes";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 20);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 17);
            this.label4.TabIndex = 30;
            this.label4.Text = "Nodes";
            // 
            // browseControl
            // 
            this.browseControl.Enabled = false;
            this.browseControl.Location = new System.Drawing.Point(11, 41);
            this.browseControl.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.browseControl.Name = "browseControl";
            this.browseControl.RebrowseOnNodeExpande = false;
            this.browseControl.Server = null;
            this.browseControl.Size = new System.Drawing.Size(365, 206);
            this.browseControl.TabIndex = 1;
            this.browseControl.SelectionChanged += new Siemens.OpcUA.Client.BrowseControl.SelectionChangedEventHandler(this.browseControl_SelectionChanged);
            // 
            // attributeListControl
            // 
            this.attributeListControl.Enabled = false;
            this.attributeListControl.Location = new System.Drawing.Point(383, 14);
            this.attributeListControl.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.attributeListControl.Name = "attributeListControl";
            this.attributeListControl.Server = uaClientHelperAPI1;
            this.attributeListControl.Size = new System.Drawing.Size(373, 234);
            this.attributeListControl.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btnGetEndpoints);
            this.groupBox2.Controls.Add(this.endpointListView);
            this.groupBox2.Controls.Add(this.discoveryTextBox);
            this.groupBox2.Location = new System.Drawing.Point(5, 21);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(763, 281);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Connecting Parameters";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.nudOPCUAKeepAliveInterval);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.pwTextBox);
            this.groupBox4.Controls.Add(this.nudOPCUAUpdateRate);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.btnConnect);
            this.groupBox4.Controls.Add(this.userTextBox);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.btnApply);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.btnCancel);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.btnUserPwd);
            this.groupBox4.Controls.Add(this.btnUserAnon);
            this.groupBox4.Location = new System.Drawing.Point(11, 164);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox4.Size = new System.Drawing.Size(747, 111);
            this.groupBox4.TabIndex = 27;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Connection Parameters";
            // 
            // nudOPCUAKeepAliveInterval
            // 
            this.nudOPCUAKeepAliveInterval.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudOPCUAKeepAliveInterval.Location = new System.Drawing.Point(499, 23);
            this.nudOPCUAKeepAliveInterval.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudOPCUAKeepAliveInterval.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nudOPCUAKeepAliveInterval.Name = "nudOPCUAKeepAliveInterval";
            this.nudOPCUAKeepAliveInterval.Size = new System.Drawing.Size(205, 22);
            this.nudOPCUAKeepAliveInterval.TabIndex = 33;
            this.nudOPCUAKeepAliveInterval.ThousandsSeparator = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(707, 25);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 17);
            this.label7.TabIndex = 31;
            this.label7.Text = "ms";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(369, 25);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(121, 17);
            this.label8.TabIndex = 32;
            this.label8.Text = "KeepAliveInterval:";
            // 
            // pwTextBox
            // 
            this.pwTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pwTextBox.Location = new System.Drawing.Point(244, 82);
            this.pwTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pwTextBox.Name = "pwTextBox";
            this.pwTextBox.PasswordChar = '*';
            this.pwTextBox.Size = new System.Drawing.Size(255, 22);
            this.pwTextBox.TabIndex = 20;
            this.pwTextBox.UseSystemPasswordChar = true;
            // 
            // nudOPCUAUpdateRate
            // 
            this.nudOPCUAUpdateRate.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudOPCUAUpdateRate.Location = new System.Drawing.Point(107, 22);
            this.nudOPCUAUpdateRate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nudOPCUAUpdateRate.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nudOPCUAUpdateRate.Name = "nudOPCUAUpdateRate";
            this.nudOPCUAUpdateRate.Size = new System.Drawing.Size(203, 22);
            this.nudOPCUAUpdateRate.TabIndex = 30;
            this.nudOPCUAUpdateRate.ThousandsSeparator = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(317, 25);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 17);
            this.label5.TabIndex = 28;
            this.label5.Text = "ms";
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnect.Location = new System.Drawing.Point(501, 53);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(239, 27);
            this.btnConnect.TabIndex = 26;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // userTextBox
            // 
            this.userTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.userTextBox.Location = new System.Drawing.Point(244, 55);
            this.userTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.Size = new System.Drawing.Size(255, 22);
            this.userTextBox.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 25);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 17);
            this.label6.TabIndex = 29;
            this.label6.Text = "Update Rate:";
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(501, 80);
            this.btnApply.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(119, 28);
            this.btnApply.TabIndex = 7;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(149, 86);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 17);
            this.label3.TabIndex = 29;
            this.label3.Text = "Password";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(621, 80);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(119, 28);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(163, 58);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 17);
            this.label2.TabIndex = 28;
            this.label2.Text = "Login";
            // 
            // btnUserPwd
            // 
            this.btnUserPwd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnUserPwd.AutoSize = true;
            this.btnUserPwd.Location = new System.Drawing.Point(11, 84);
            this.btnUserPwd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUserPwd.Name = "btnUserPwd";
            this.btnUserPwd.Size = new System.Drawing.Size(124, 21);
            this.btnUserPwd.TabIndex = 18;
            this.btnUserPwd.Text = "User/Password";
            this.btnUserPwd.UseVisualStyleBackColor = true;
            this.btnUserPwd.CheckedChanged += new System.EventHandler(this.btnUserPwd_CheckedChanged);
            // 
            // btnUserAnon
            // 
            this.btnUserAnon.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnUserAnon.AutoSize = true;
            this.btnUserAnon.Checked = true;
            this.btnUserAnon.Location = new System.Drawing.Point(11, 57);
            this.btnUserAnon.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUserAnon.Name = "btnUserAnon";
            this.btnUserAnon.Size = new System.Drawing.Size(103, 21);
            this.btnUserAnon.TabIndex = 17;
            this.btnUserAnon.TabStop = true;
            this.btnUserAnon.Text = "Anonymous";
            this.btnUserAnon.UseVisualStyleBackColor = true;
            this.btnUserAnon.CheckedChanged += new System.EventHandler(this.btnUserAnon_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Node IP";
            // 
            // btnGetEndpoints
            // 
            this.btnGetEndpoints.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetEndpoints.Location = new System.Drawing.Point(637, 18);
            this.btnGetEndpoints.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGetEndpoints.Name = "btnGetEndpoints";
            this.btnGetEndpoints.Size = new System.Drawing.Size(119, 28);
            this.btnGetEndpoints.TabIndex = 25;
            this.btnGetEndpoints.Text = "Get Endpoints";
            this.btnGetEndpoints.UseVisualStyleBackColor = true;
            this.btnGetEndpoints.Click += new System.EventHandler(this.btnGetEndpoints_Click);
            // 
            // endpointListView
            // 
            this.endpointListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.endpointListView.BackColor = System.Drawing.SystemColors.Window;
            this.endpointListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Endpoints});
            this.endpointListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.endpointListView.HideSelection = false;
            this.endpointListView.Location = new System.Drawing.Point(11, 52);
            this.endpointListView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.endpointListView.MultiSelect = false;
            this.endpointListView.Name = "endpointListView";
            this.endpointListView.Size = new System.Drawing.Size(745, 105);
            this.endpointListView.TabIndex = 24;
            this.endpointListView.UseCompatibleStateImageBehavior = false;
            this.endpointListView.View = System.Windows.Forms.View.Details;
            // 
            // Endpoints
            // 
            this.Endpoints.Text = "Found Endpoints";
            this.Endpoints.Width = 900;
            // 
            // discoveryTextBox
            // 
            this.discoveryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.discoveryTextBox.Location = new System.Drawing.Point(75, 22);
            this.discoveryTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.discoveryTextBox.Name = "discoveryTextBox";
            this.discoveryTextBox.Size = new System.Drawing.Size(555, 22);
            this.discoveryTextBox.TabIndex = 23;
            // 
            // formDefineOPCUA
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(801, 583);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formDefineOPCUA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Define OPC UA";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDefineOPCUA_FormClosing);
            this.Load += new System.EventHandler(this.formDefineOPCUA_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOPCUAKeepAliveInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOPCUAUpdateRate)).EndInit();
            this.ResumeLayout(false);

        }

       

        private GroupBox groupBox1;
        private Button btnCancel;
        private Button btnApply;
        private GroupBox groupBox3;
        private GroupBox groupBox2;
        private Siemens.OpcUA.Client.BrowseControl browseControl;
        private Siemens.OpcUA.Client.AttributeListControl attributeListControl;
        private ListView endpointListView;
        private ColumnHeader Endpoints;
        private TextBox discoveryTextBox;
        private Button btnConnect;
        private Button btnGetEndpoints;
        private Label label1;
        private GroupBox groupBox4;
        private TextBox pwTextBox;
        private RadioButton btnUserAnon;
        private RadioButton btnUserPwd;
        private TextBox userTextBox;
        private Label label4;
        private NumericUpDown nudOPCUAUpdateRate;
        private Label label5;
        private Label label6;
        private Label label3;
        private Label label2;
        private NumericUpDown nudOPCUAKeepAliveInterval;
        private Label label7;
        private Label label8;
    }
}