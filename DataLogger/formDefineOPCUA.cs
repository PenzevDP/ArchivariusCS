using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools;
using Siemens.UAClientHelper;
using Siemens.OpcUA;
using Opc.Ua;
using DataManager;
using Opc.Ua.Client;
using System.Security.Cryptography.X509Certificates;


namespace DataLogger
{
    public partial class formDefineOPCUA : Form
    {
        public formDefineOPCUA()
        {
            NLogger.logger.Trace("Service. formDefineOPCUA has initialized");
            InitializeComponent();

        }


        private string discoveryIP = "";
        private UAClientHelperAPI clientAPI = new UAClientHelperAPI();
        private bool serverconnected = false;
       
        private void formDefineOPCUA_Load(object sender, EventArgs e)
        {
            discoveryTextBox.Text = Config.Sets.Primary_OPCUA_EndpointURL;
            nudOPCUAUpdateRate.Value = Config.Sets.UpdateRate;
            nudOPCUAKeepAliveInterval.Value = Config.Sets.KeepAliveInterval;
            endpointListView.Items.Clear();

            browseControl.BrowseTree.BeginUpdate();
            browseControl.BrowseTree.Nodes.Clear();
            browseControl.BrowseTree.EndUpdate();
            attributeListControl.AttributeList.Items.Clear();

            btnUserAnon.Checked = !Config.Sets.Primary_OPCUA_LoginMode;
            btnUserPwd.Checked = Config.Sets.Primary_OPCUA_LoginMode;



            if (btnUserPwd.Checked)
            {
                userTextBox.Text = Config.Sets.Primary_OPCUA_User;
                pwTextBox.Text = Config.Sets.Primary_OPCUA_Pass;
                SafeThread.SetEnableControl(userTextBox, true);
                SafeThread.SetEnableControl(pwTextBox, true);
            }
            else
            {
                userTextBox.Text = "";
                pwTextBox.Text = "";
                SafeThread.SetEnableControl(userTextBox, false);
                SafeThread.SetEnableControl(pwTextBox, false);
            }

            UpdateConfigState(Config.State);
            Config.StateChange += ConfigStateChange;
        }


        private void UpdateConfigState(ConfigState state)
        {
            if (state == ConfigState.Stopped)
            {
                SafeThread.SetEnableControl(discoveryTextBox, true);
                SafeThread.SetEnableControl(endpointListView, true);
                SafeThread.SetEnableControl(nudOPCUAUpdateRate, true);
                SafeThread.SetEnableControl(nudOPCUAKeepAliveInterval, true);
                SafeThread.SetEnableControl(btnApply, true);
                SafeThread.SetEnableControl(btnGetEndpoints, true);
                SafeThread.SetEnableControl(btnUserAnon, true);
                SafeThread.SetEnableControl(btnUserPwd, true);

            }
            else
            {
                SafeThread.SetEnableControl(discoveryTextBox, false);
                SafeThread.SetEnableControl(endpointListView, false);
                SafeThread.SetEnableControl(nudOPCUAUpdateRate, false);
                SafeThread.SetEnableControl(nudOPCUAKeepAliveInterval, false);
                SafeThread.SetEnableControl(btnApply, false);
                SafeThread.SetEnableControl(btnGetEndpoints, false);
                SafeThread.SetEnableControl(btnUserAnon, false);
                SafeThread.SetEnableControl(btnUserPwd, false);
            }
        }

        private void ConfigStateChange(object sender, ConfigStateEventArgs e)
        {
            UpdateConfigState(e.State);
        }






        private void btnGetEndpoints_Click(object sender, EventArgs e)

        {
            NLogger.logger.Trace("Service. formDefineOPCUA button GET ENDPOINTS click");
            using (new WaitCursor())
            {
                try
                {
                    //former correct discoveryIP
                    if (discoveryTextBox.Text == "")
                    {
                        discoveryIP = "opc.tcp://localhost:4840";
                    }
                    else
                    {
                        if (discoveryTextBox.Text.Contains("opc.tcp://"))
                        {
                            int n = discoveryTextBox.Text.IndexOf("opc.tcp://");
                            if (n != -1)
                                discoveryIP = discoveryTextBox.Text.Remove(n, "opc.tcp://".Length);
                        }
                        else
                        {
                            discoveryIP = discoveryTextBox.Text;
                        }

                        char seperator = ':';
                        string[] strPortCheck = discoveryIP.Split(seperator);

                        if (strPortCheck.Length > 1)
                        {

                            discoveryIP = "opc.tcp://" + discoveryIP;

                        }
                        else
                        {
                            discoveryIP = "opc.tcp://" + discoveryIP + ":4840";
                        }
                    }
                    discoveryTextBox.Text = discoveryIP;

                    //disconnecting from existing servers
                    try
                    {
                        if (clientAPI.Session != null && !clientAPI.Session.Disposed)
                            if (clientAPI.Session.Connected)
                                clientAPI.Disconnect();
                    }
                    catch (Exception ex)
                    {
                        Config.Log.WriteEntry("Define OpcUa. Closing connection on exit error: " + ex.Message);

                    }
                    //clear endpoints and attributes
                    endpointListView.Items.Clear();
                    browseControl.BrowseTree.BeginUpdate();
                    browseControl.BrowseTree.Nodes.Clear();
                    browseControl.BrowseTree.EndUpdate();
                    attributeListControl.AttributeList.Items.Clear();

                    //getendpoints
                    if (discoveryTextBox.Text != "")
                    {

                        try

                        {
                            ApplicationDescriptionCollection AppDescColl = clientAPI.FindServers(discoveryTextBox.Text);

                            foreach (ApplicationDescription AppDescr in AppDescColl)
                            {
                                try
                                {
                                    foreach (string url in AppDescr.DiscoveryUrls)
                                    {
                                        EndpointDescriptionCollection EndPointDescColl = clientAPI.GetEndpoints(url);
                                        foreach (EndpointDescription EndPointDesc in EndPointDescColl)
                                        {
                                            endpointListView.Items.Add(new OpcUaEndpointWrapper(EndPointDesc).ToString()).Tag = EndPointDesc;
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    NLogger.logger.Error("Define OpcUa. Get Endpoint Description error:{error}" , ex.Message);
                                    Config.Log.WriteEntry("Define OpcUa. Get Endpoint Description error: " + ex.Message);

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            NLogger.logger.Error("Define OpcUa. Find servers error:{error}", ex.Message);
                            Config.Log.WriteEntry("Define OpcUa. Find servers error: " + ex.Message);

                        }
                    }
                    else
                    {

                    }


                }
                catch (Exception ex)
                {
                    NLogger.logger.Error("Define OpcUa. Get endpoint error:{error}", ex.Message);
                    Config.Log.WriteEntry("Define OpcUa. Get endpoint error: " + ex.Message);


                }

            }

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

            using (new WaitCursor())
            {
                if (serverconnected)
                {
                    if (Disconnect() == 0)
                    {
                        serverconnected = false;
                        btnConnect.Text = "Connect";
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
                    if (endpointListView.FocusedItem == null)
                    { MessageBox.Show("Select the endpoint", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                    else
                    {
                        if (Connect(((EndpointDescription)endpointListView.FocusedItem.Tag).EndpointUrl, ((EndpointDescription)endpointListView.FocusedItem.Tag).SecurityPolicyUri, ((EndpointDescription)endpointListView.FocusedItem.Tag).SecurityMode, btnUserPwd.Checked, userTextBox.Text, pwTextBox.Text) == 0)
                        {
                            browseControl.Server = clientAPI;
                            attributeListControl.Server = clientAPI;
                            browseControl.Browse(null);
                            serverconnected = true;
                            btnConnect.Text = "Disconnect";
                            browseControl.Enabled = true;
                            attributeListControl.Enabled = true;
                        }
                        else
                        {
                            clientAPI.KeepAliveNotification -= new KeepAliveEventHandler(clientAPI_KeepAlive);
                            clientAPI.CertificateValidationNotification -= new CertificateValidationEventHandler(clientAPI_CertificateEvent);
                            btnConnect.Text = "Reconnect";
                        }
                    }
                }
            }
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

        private void browseControl_SelectionChanged(TreeNode selectedNode)
        {
            attributeListControl.ReadAttributes(selectedNode);
        }



        private void btnUserAnon_CheckedChanged(object sender, EventArgs e)
        {
            if (btnUserAnon.Checked)
            {
                btnUserPwd.Checked = false;
                userTextBox.Enabled = false;
                pwTextBox.Enabled = false;
            }
        }

        private void btnUserPwd_CheckedChanged(object sender, EventArgs e)
        {
            if (btnUserPwd.Checked)
            {
                btnUserAnon.Checked = false;
                userTextBox.Enabled = true;
                pwTextBox.Enabled = true;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (endpointListView.FocusedItem == null || discoveryTextBox.Text == "")
            { MessageBox.Show("Select the endpoint and enter Node IP", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            else
            {

                Config.Sets.Primary_OPCUA_EndpointURL = ((EndpointDescription)endpointListView.FocusedItem.Tag).EndpointUrl;
                Config.Sets.Primary_OPCUA_EndpointSecurityPolicyUri = ((EndpointDescription)endpointListView.FocusedItem.Tag).SecurityPolicyUri;
                Config.Sets.Primary_OPCUA_EndpointSecurityMode = (int)((EndpointDescription)endpointListView.FocusedItem.Tag).SecurityMode;


                if (userTextBox.Text != "")
                {
                    Config.Sets.Primary_OPCUA_User = userTextBox.Text;
                    Config.Sets.Primary_OPCUA_Pass = pwTextBox.Text;
                }
                else
                {
                    Config.Sets.Primary_OPCUA_User = "";
                    Config.Sets.Primary_OPCUA_Pass = "";
                }
                Config.Sets.Primary_OPCUA_LoginMode = btnUserPwd.Checked;
                Config.Sets.UpdateRate = (int)nudOPCUAUpdateRate.Value;
                Config.Sets.KeepAliveInterval = (int)nudOPCUAKeepAliveInterval.Value;
                Config.Save();
                this.Close();
            }

        }



        private void FormDefineOPCUA_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (clientAPI.Session != null && !clientAPI.Session.Disposed)
                    if (clientAPI.Session.Connected)
                        Disconnect();
            }
            catch (Exception ex)
            {
                NLogger.logger.Error("Define OpcUa. Closing connection on exit error:{error}", ex.Message);
                Config.Log.WriteEntry("Define OpcUa. Closing connection on exit error: " + ex.Message);

            }

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


        private void clientAPI_KeepAlive(Session sender, KeepAliveEventArgs e)
        {
            // Connection handling not implemented
            ;
        }


    }
}
