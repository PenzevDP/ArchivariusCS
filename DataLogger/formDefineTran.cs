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

namespace Archivarius
{
    public partial class formDefineTran : Form
    {
        private DataSet copyTransaction;
        private DataSet copyTransactionOPCUA;


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
                SafeThread.SetAllowUserToAddRowsDataGridView(dataGridTrn, true);
                SafeThread.SetAllowUserToAddRowsDataGridView(dataGridtrnOPC, true);
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
        private void browseControl_SelectionChanged(TreeNode selectedNode)
        {
            attributeListControl.ReadAttributes(selectedNode);
        }

        private void ConfigStateChange(object sender, ConfigStateEventArgs e)
        {
            UpdateConfigState(e.State);
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
            MessageBox.Show(e.Exception.Message, Application.ProductName);
            e.Cancel = true;       
        }

        private void dataGridTrnOPC_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, Application.ProductName);
            e.Cancel = true;
        }

        private void dataGridTrn_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridtrnOPC_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void OPCUA_transaction_Click(object sender, EventArgs e)
        {

        }

        private void navigatorMoveFirstItem_Click(object sender, EventArgs e)
        {

        }

        private void navigatorAddNewItem_Click(object sender, EventArgs e)
        {

        }

        private void navigatorDeleteItem_Click(object sender, EventArgs e)
        {

        }

     

       
      

        private void button1_Click_1(object sender, EventArgs e)
        {
            groupBox3.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = false;
        }

        private void dataGridtrnOPC_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex != 0)
            {
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




    }

}
