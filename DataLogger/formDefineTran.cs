using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataManager;
using Tools;
using Settings;
using Opc.Ua.Client;
using Opc.Ua;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Siemens.UAClientHelper;
using Siemens.OpcUA;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Channels;

namespace Archivarius
{
    public partial class formDefineTran : Form
    {
        private DataSet copyTransaction;
        private DataSet copyTransactionOPCUA;

        private string ns  ;
        private string dbuaname  ;
        private string arrayname ;

        private string discoveryIP = "";
        private UAClientHelperAPI clientAPI = new UAClientHelperAPI();
        private bool serverconnected = false;

        public formDefineTran()
        {
            InitializeComponent();
        }

        private void UpdateConfigState(ConfigState state)
        {
            if (state == ConfigState.Stopped)
            {
                SafeThread.SetEnableControl(btnApply, true);
                SafeThread.SetReadOnlyDataGridView(dataGridTrn, false);
                SafeThread.SetReadOnlyDataGridView(dataGridtrnOPC, false);
                SafeThread.SetAllowUserToAddRowsDataGridView(dataGridTrn, false);
                SafeThread.SetAllowUserToAddRowsDataGridView(dataGridtrnOPC, false);
                SafeThread.SetVisibleStripItem(navigatorTran, navigatorAddNewItem, true);
                SafeThread.SetVisibleStripItem(navigatorTran, navigatorDeleteItem, true);
                SafeThread.SetVisibleStripItem(navigatorTranOPC, navigatorAddNewItemOPC, true);
                SafeThread.SetVisibleStripItem(navigatorTranOPC, navigatorDeleteItemOPC, true);
            }
            else
            {                
                SafeThread.SetEnableControl(btnApply, false);
                SafeThread.CancelEditDataGridView(dataGridTrn);
                SafeThread.CancelEditDataGridView(dataGridtrnOPC);
                SafeThread.SetReadOnlyDataGridView(dataGridTrn, true);
                SafeThread.SetReadOnlyDataGridView(dataGridtrnOPC, true);
                SafeThread.SetAllowUserToAddRowsDataGridView(dataGridTrn, false);
                SafeThread.SetAllowUserToAddRowsDataGridView(dataGridtrnOPC, false);
                SafeThread.SetVisibleStripItem(navigatorTran, navigatorAddNewItem, false);
                SafeThread.SetVisibleStripItem(navigatorTran, navigatorDeleteItem, false);
                SafeThread.SetVisibleStripItem(navigatorTranOPC, navigatorAddNewItemOPC, false);
                SafeThread.SetVisibleStripItem(navigatorTranOPC, navigatorDeleteItemOPC, false);
            }
        }

        private void frmDefineTrn_Load(object sender, EventArgs e)
        {
            copyTransaction = Config.Sets.TransactionBase.Copy();
            bindingTran.DataSource = copyTransaction.Tables["TransactionTable"];
            dataGridTrn.DataSource = bindingTran;

            copyTransactionOPCUA = Config.Sets.TransactionBaseOPCUA.Copy();
            bindingTranOPC.DataSource = copyTransactionOPCUA.Tables["TransactionTable"];
            dataGridtrnOPC.DataSource = bindingTranOPC;
            


            navigatorTran.BindingSource = bindingTran;
            navigatorTranOPC.BindingSource = bindingTranOPC;
            UpdateConfigState(Config.State);
            Config.StateChange += ConfigStateChange;


          //  NLogger.logger.Warn(Config.Sets.Primary_OPCUA_EndpointURL.ToString());

            if (Config.Sets.Primary_OPCUA_EndpointURL!="")
            {
                using (new WaitCursor())
                {
                    if (serverconnected)
                    {
                        if (Disconnect() == 0)
                        {
                            serverconnected = false;

                            browseControl.Enabled = false;
                            attributeListControl.Enabled = false;
                            //clear endpoints and attributes
                            browseControl.BrowseTree.BeginUpdate();
                            browseControl.BrowseTree.Nodes.Clear();
                            browseControl.BrowseTree.EndUpdate();
                            attributeListControl.AttributeList.Items.Clear();

                        }
                    }
                    else
                    {

                        if (Connect(Config.Sets.Primary_OPCUA_EndpointURL, Config.Sets.Primary_OPCUA_EndpointSecurityPolicyUri, (MessageSecurityMode)Config.Sets.Primary_OPCUA_EndpointSecurityMode, Config.Sets.Primary_OPCUA_LoginMode, Config.Sets.Primary_OPCUA_User, Config.Sets.Primary_OPCUA_Pass) == 0)
                        {
                            browseControl.Server = clientAPI;
                            attributeListControl.Server = clientAPI;
                            browseControl.Browse(null);
                            serverconnected = true;

                            browseControl.Enabled = true;
                            attributeListControl.Enabled = true;
                        }
                        else
                        {
                            clientAPI.KeepAliveNotification -= new KeepAliveEventHandler(clientAPI_KeepAlive);
                            clientAPI.CertificateValidationNotification -= new CertificateValidationEventHandler(clientAPI_CertificateEvent);

                        }

                    }
                }
            }
        }
        private void browseControl_SelectionChanged(TreeNode selectedNode)
        {
            
            NLogger.logger.Fatal(dataGridtrnOPC.RowCount.ToString());
            attributeListControl.ReadAttributes(selectedNode);
            ReferenceDescription reference = (ReferenceDescription)selectedNode.Tag;
            string nodeid = reference.NodeId.ToString();

            
            char ch1 = '=';
            char ch2 = ';';
            int indexOfChar1 = nodeid.IndexOf(ch1);
            int indexOfChar2 = nodeid.IndexOf(ch2);
            int length = indexOfChar2 - indexOfChar1 - 1 ;
            if (length > 0) {ns = nodeid.Substring(indexOfChar1 + 1, length); }
            
            if (indexOfChar2 > 0) { nodeid = nodeid.Substring(indexOfChar2); }
            

           
            indexOfChar1 = nodeid.IndexOf(ch1);
            nodeid = nodeid.Substring(indexOfChar1+1);

            if (!nodeid.Contains("["))
            {
                
                 dbuaname = nodeid;
                textNode.Text = nodeid;

            }
            else
            {

                ch1 = '[';
                indexOfChar1 = nodeid.IndexOf(ch1);
                nodeid = nodeid.Substring(0, indexOfChar1);

                
                ch1 = '.';
                indexOfChar1 = nodeid.LastIndexOf(ch1);
                dbuaname = nodeid.Substring(0,indexOfChar1);
                textNode.Text = dbuaname;
                nodeid = nodeid.Substring(indexOfChar1+1);

                 arrayname = nodeid;
                

            }
           // NLogger.logger.Error("ns# = " + ns + " ; dbuaname=" + dbuaname + " ; array name=" + arrayname);

        }

        private void ConfigStateChange(object sender, ConfigStateEventArgs e)
        {
           // UpdateConfigState(e.State);
        }

        private int Connect(string url, string secPolicy, MessageSecurityMode MsgSecMode, bool UserAuth, string userName, string Pass)
        {
            int result = -1;
            try
            {
                //Hook up to KeepAlive event
                clientAPI.KeepAliveNotification += new KeepAliveEventHandler(clientAPI_KeepAlive);
                clientAPI.CertificateValidationNotification += new CertificateValidationEventHandler(clientAPI_CertificateEvent);

                //clientAPI.Connect((EndpointDescription)endpointListView.FocusedItem.Tag, btnUserPwd.Checked, userTextBox.Text, pwTextBox.Text);
                clientAPI.Connect(url, secPolicy, MsgSecMode, UserAuth, userName, Pass);

                result = 0;

            }
            catch (Exception ex)
            {
                NLogger.logger.Error("Define OpcUa. Setting Up connection error:{error}", ex.Message);
                Config.Log.WriteEntry("Define OpcUa. Setting Up connection error: " + ex.Message);

            }

            return result;
        }

        private int Disconnect()
        {
            int result = -1;
            try
            {
                clientAPI.KeepAliveNotification -= new KeepAliveEventHandler(clientAPI_KeepAlive);
                clientAPI.CertificateValidationNotification -= new CertificateValidationEventHandler(clientAPI_CertificateEvent);
                clientAPI.Disconnect();
                result = 0;
            }
            catch (Exception ex)
            {
                NLogger.logger.Error("Define OpcUa. Breaking Up connection error:{error}", ex.Message);
                Config.Log.WriteEntry("Define OpcUa. Breaking Up connection error: " + ex.Message);

            }


            return result;

        }


        private void btnApply_Click(object sender, EventArgs e)
        {
            Config.Sets.TransactionBase = copyTransaction.Copy();
            Config.Sets.TransactionBaseOPCUA = copyTransactionOPCUA.Copy();
            Config.Save();
            this.Close();
        }

        private void dataGridTrn_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //MessageBox.Show(e.Exception.Message, Application.ProductName);
            NLogger.logger.Fatal(e.Exception.Message);
            e.Cancel = true;       
        }

        private void dataGridTrnOPC_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //MessageBox.Show(e.Exception.Message, Application.ProductName);
            NLogger.logger.Fatal(e.Exception.Message);
            e.Cancel = true;
        }

        private void dataGridTrnOPC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }




        private void OPCUA_transaction_Click(object sender, EventArgs e)
        {

        }

        private void navigatorMoveFirstItem_Click(object sender, EventArgs e)
        {

        }
        private void navigatorMoveNextItem_Click(object sender, EventArgs e)
        {

        }
        private void navigatorMovePreviousItem_Click(object sender, EventArgs e)
        {

        }
        private void navigatorMoveLastItem_Click(object sender, EventArgs e)
        {

        }

        private void navigatorAddNewItem_Click(object sender, EventArgs e)
        {

        }
        private void navigatorAddNewItemOPC_Click(object sender, EventArgs e)
        {

        }

        private void navigatorDeleteItem_Click(object sender, EventArgs e)
        {

        }
        private void navigatorDeleteItemOPC_Click(object sender, EventArgs e)
        {

        }






        private void button1_Click_1(object sender, EventArgs e)
        {
            if (Tab.SelectedTab.Name == "OPCUA_transaction")
            {
                if (ns != "") 
                { 
                    if (dataGridtrnOPC.CurrentRow != null)
                    {
                        dataGridtrnOPC.CurrentRow.Cells["ns#"].Value = ns;
                    }
                    else
                    {
                        
                        dataGridtrnOPC.Rows[1].Cells["ns#"].Value = ns;
                    }
                    
                
                }
                if (dbuaname != "") 
                {
                    if (dataGridtrnOPC.CurrentRow != null)
                    {
                        dataGridtrnOPC.CurrentRow.Cells["DBUA Name"].Value = dbuaname;
                    }
                    else
                    {
                        dataGridtrnOPC.Rows[1].Cells["DBUA Name"].Value = dbuaname;
                    }
                }
                if (arrayname != "")
                {
                    if (dataGridtrnOPC.CurrentRow != null)
                    {
                        dataGridtrnOPC.CurrentRow.Cells["ArrayUA Name"].Value = arrayname;                
                    }
                    else
                    {
                        dataGridtrnOPC.Rows[1].Cells["ArrayUA Name"].Value = arrayname;
                    }
                }

                if (dataGridtrnOPC.CurrentRow != null)
                {
                    NLogger.logger.Fatal("Здесь был не NULL");
                    NLogger.logger.Fatal(dataGridtrnOPC.CurrentRow.Cells["Transaction Name"].Value.ToString());
                    if (dataGridtrnOPC.CurrentRow.Cells["Transaction Name"].Value.ToString() == "")
                    {
                        dataGridtrnOPC.CurrentRow.Cells["Transaction Name"].Value = "OPC_transaction_" + (dataGridtrnOPC.CurrentRow.Index + 1).ToString();

                    }
                }
                else
                {
                    NLogger.logger.Fatal("Здесь был NULL");
                    dataGridtrnOPC.Rows[1].Cells["Transaction Name"].Value = "OPC_transaction_" + (dataGridtrnOPC.Rows[0].Index + 1).ToString();

                    
                }

            }
            else if (Tab.SelectedTab.Name == "DB_transaction")
            {
                if (ns != "")
                {
                    if (dataGridTrn.CurrentRow != null)
                    {
                        dataGridTrn.CurrentRow.Cells["ns#"].Value = ns;
                    }
                    else
                    {
                        dataGridTrn.Rows[0].Cells["ns#"].Value = ns;
                    }


                }
                if (dbuaname != "")
                {
                    if (dataGridTrn.CurrentRow != null)
                    {
                        dataGridTrn.CurrentRow.Cells["DBUA Name"].Value = dbuaname;
                    }
                    else
                    {
                        dataGridTrn.Rows[0].Cells["DBUA Name"].Value = dbuaname;
                    }
                }

                if (arrayname != "")
                {
                    if (dataGridTrn.CurrentRow != null)
                    {
                        dataGridTrn.CurrentRow.Cells["ArrayUA Name"].Value = arrayname;
                    }
                    else
                    {
                        dataGridTrn.Rows[0].Cells["ArrayUA Name"].Value = arrayname;
                    }
                }

                if (dataGridTrn.CurrentRow != null)
                {
                    if (dataGridTrn.CurrentRow.Cells["Transaction Name"].Value.ToString() == "")
                    {
                        dataGridTrn.CurrentRow.Cells["Transaction Name"].Value = "DB_transaction_" + (dataGridTrn.CurrentRow.Index + 1).ToString();

                    }
                    if (dataGridTrn.CurrentRow.Cells["SizeUA Name"] == null)
                    {
                        dataGridTrn.CurrentRow.Cells["SizeUA Name"].Value = "Size";
                    }
                    if (dataGridTrn.CurrentRow.Cells["CounterUA Name"] == null)
                    {
                        dataGridTrn.CurrentRow.Cells["CounterUA Name"].Value = "Count";
                    }
                }
                else
                {
                    if (dataGridTrn.Rows[0].Cells["Transaction Name"].Value.ToString() == "")
                    {
                        dataGridTrn.Rows[0].Cells["Transaction Name"].Value = "DB_transaction_" + (dataGridtrnOPC.Rows[0].Index + 1).ToString();

                    }
                    if (dataGridTrn.Rows[0].Cells["SizeUA Name"] == null)
                    {
                        dataGridTrn.Rows[0].Cells["SizeUA Name"].Value = "Size";
                    }
                    if (dataGridTrn.Rows[0].Cells["CounterUA Name"] == null)
                    {
                        dataGridTrn.Rows[0].Cells["CounterUA Name"].Value = "Count";
                    }
                }

            }
            groupBox3.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = false;
        }

        private void dataGridtrnOPC_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
           


                if (groupBox3.Visible) { groupBox3.Visible = false; }
                if (e.ColumnIndex > 0 & e.ColumnIndex < 4 & Config.Sets.Primary_OPCUA_EndpointURL != "")
                {
                    int indexX = dataGridtrnOPC.CurrentCell.RowIndex;
                    int indexY = dataGridtrnOPC.CurrentCell.ColumnIndex;
                    Point Location = new Point(20 + indexX * 50, 80 + dataGridtrnOPC.CurrentRow.Height);
                    groupBox3.Location = Location;


                    groupBox3.Visible = true;
                }

            
        
        }

        private void clientAPI_KeepAlive(Session sender, KeepAliveEventArgs e)
        {
            // Connection handling not implemented
            ;
        }

        private void clientAPI_CertificateEvent(CertificateValidator cert, CertificateValidationEventArgs e)
        {
            //Handle certificate here
            //To accept a certificate manually move it to the root folder (Start > mmc.exe > add snap-in > certificates)
            //Or handle via UAClientCertForm
            using (new WaitCursor())
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new CertificateValidationEventHandler(clientAPI_CertificateEvent), cert, e);
                    return;
                }

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


                    else
                    {
                        if (!e.Accept)
                        {
                            try
                            {
                                store.Open(OpenFlags.ReadWrite);
                                store.Add(e.Certificate);
                                store.Close();
                                MessageBox.Show("New trusted certificate was created and added to certificate store. For continue please reconnect to server.", "Added Certificate!", MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                            }
                            catch
                            {
                                MessageBox.Show("Error adding trusted certificate to certificate store", "Add Certificate Error!", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                            }
                        }
                    }
                }
                catch
                {
                    ;
                }
            }
        }

        private void browseControl_Load(object sender, EventArgs e)
        {

        }

        private void dataGridTrn_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            
            if (groupBox3.Visible) { groupBox3.Visible = false; }
            if (e.ColumnIndex > 0 & e.ColumnIndex < 4 & Config.Sets.Primary_OPCUA_EndpointURL != "")
            {

                //int indexX = dataGridTrn.CurrentCell.RowIndex;
               // int indexY = dataGridTrn.CurrentCell.ColumnIndex;
                //Point Location = new Point(20 + indexX * 50, 80 + dataGridTrn.CurrentRow.Height);
               // groupBox3.Location = Location;            
                groupBox3.Visible = true;
            }
        }

       

        private void dataGridTrn_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           NLogger.logger.Fatal(dataGridtrnOPC.RowCount.ToString());

        }


        private void checkTransactionParams(int type)
        {
            DataGridView dgv;
            BindingNavigator nav;
            ToolStripButton btn;

            bool paramsIsNull = false;
            if (type ==1)
            {
                dgv = this.dataGridTrn;
                nav = this.navigatorTran;
                btn = this.navigatorAddNewItem;
              
            }
            else
            {
                
                dgv = this.dataGridtrnOPC;
                nav = this.navigatorTranOPC;
                btn = this.navigatorAddNewItemOPC;
            }
            foreach (DataGridViewRow rw in dgv.Rows)
            {
                for (int i = 0; i < rw.Cells.Count; i++)
                {
                    if (rw.Cells[i].Value == null || rw.Cells[i].Value == DBNull.Value || String.IsNullOrWhiteSpace(rw.Cells[i].Value.ToString()))
                    {
                        paramsIsNull = true;
                        // NLogger.logger.Fatal(rw.Cells[i].Value.ToString() + "стр - " + rw + "/ стлб - " + i);
                    }
                    
                    NLogger.logger.Fatal("Это трагедия"+ paramsIsNull.ToString());
                }

                Int16 val;
                bool isInt = Int16.TryParse(rw.Cells[1].Value.ToString(), out val);
                
                if (!isInt  || (isInt & val < 0))
                {
                    paramsIsNull = true;
                }
            }
            NLogger.logger.Fatal("Это pzdch" + paramsIsNull.ToString());
            if (paramsIsNull)
            {
                SafeThread.SetVisibleStripItem(nav, btn, false);
                SafeThread.SetEnableControl(btnApply, false);

            }
            else
            {
                SafeThread.SetVisibleStripItem(nav, btn, true);
                SafeThread.SetEnableControl(btnApply, true);
            }



        }

        private void dataGridTrn_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            checkTransactionParams(1);
        }
        private void dataGridTrnOPC_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            checkTransactionParams(2);
        }

        private void dataGridTrn_SelectionChanged(object sender, EventArgs e)
        {
            checkTransactionParams(1);
        }
        private void dataGridTrnOPC_SelectionChanged(object sender, EventArgs e)
        {
            checkTransactionParams(2);
        }
    }

}
