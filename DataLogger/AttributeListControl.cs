using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Opc.Ua;
using Siemens.UAClientHelper;

namespace Siemens.OpcUA.Client
{
    public partial class AttributeListControl : UserControl
    {
        /// <summary>
        /// Event handler for the event that the status label of the main form has to be updated.
        /// </summary>
        public delegate void UpdateStatusLabelEventHandler(string strMessage, bool bSuccess);
        /// <summary>
        /// Use the delegate as event.
        /// </summary>
        public event UpdateStatusLabelEventHandler UpdateStatusLabel = null;
        /// <summary>
        /// An exception was thrown.
        /// </summary>
        public void OnUpdateStatusLabel(string strMessage, bool bSuccess)
        {
            if (UpdateStatusLabel != null)
            {
                UpdateStatusLabel(strMessage, bSuccess);
            }
        }

        #region Construction
        public AttributeListControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Fields
        /// <summary>
        /// Provides access to OPC UA server.
        /// </summary>
        private UAClientHelperAPI m_Server = new UAClientHelperAPI();
        /// <summary>
        /// Keeps current value to write.
        /// </summary>
        private WriteValue m_CurrentWriteValue;
        /// <summary>
        /// Keeps current value.
        /// </summary>
        private Object m_CurrentValue;
        /// <summary>
        /// Keeps the name of the node to write.
        /// </summary>
        private string m_CurrentWriteNodeName;
        /// <summary> 
        /// Keeps a hash table for attribute names. 
        /// </summary>
        private Hashtable m_hashAttributeNames = null;
        #endregion

        #region Properties
        public UAClientHelperAPI Server
        {
            get { return m_Server; }
            set { m_Server = value; }
        }
        #endregion

        #region Calls to ClientWrapper API
        /// <summary>
        /// Helper function for reading attributes.
        /// </summary>
        /// <param name="treeNodeToRead"></param>
        /// <returns></returns>
        public int ReadAttributes(TreeNode treeNodeToRead)
        {
            ReadValueIdCollection nodesToRead;
            DataValueCollection results;
            DiagnosticInfoCollection diag;
            
            ReferenceDescription refDescr = (ReferenceDescription)treeNodeToRead.Tag;
            if (refDescr == null)
            {
                return -1;
            }

            // Create a read request.
            buildAttributeList(refDescr, out nodesToRead);

            try
            {
                // Clear list view.
                this.lvAttributes.Items.Clear();

                m_Server.Session.Read(null, 0, Opc.Ua.TimestampsToReturn.Neither, nodesToRead, out results, out diag);

                // Show results in the listview.
                updateAttributeList(nodesToRead, results);
            }
            catch (ServiceResultException e)
            {
                // Update status label.
                OnUpdateStatusLabel("An exception occured while reading: " + e.Message, false);
                return -1;
            }
            catch (Exception e)
            {
                // Update status label.
                OnUpdateStatusLabel("An exception occured while reading: " + e.Message, false);
                return -1;
            }

            // Update status label.
            OnUpdateStatusLabel("Read succeeded for Node \"" + refDescr.DisplayName + "\".", true);
            return 0;
        }

        /// <summary>
        /// Update the attribute listview.
        /// </summary>
        /// <param name="nodesToRead"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        private void updateAttributeList(ReadValueIdCollection nodesToRead, DataValueCollection results)
        {
            if (nodesToRead.Count != results.Count)
            {
                // Error case.
                return;
            }

            try
            {
                for (int i = 0; i < nodesToRead.Count - 1; i++)
                {
                    string attributeName = (string)nodesToRead[i].Handle;
                    string attributeValue;
                    string attributeStatus;

                    // Add the attribute name / value to the list view.
                    ListViewItem item = new ListViewItem(attributeName);

                    // Add value.
                    if (results[i].WrappedValue.Value != null)
                    {
                        attributeValue = results[i].WrappedValue.Value.ToString();
                    }
                    else
                    {
                        attributeValue = "null";
                    }
                    item.SubItems.Add(attributeValue);

                    if (nodesToRead[i].AttributeId == Attributes.Value)
                    {
                        m_CurrentWriteValue	= new WriteValue();
                        m_CurrentWriteValue.AttributeId = Attributes.Value;
                        m_CurrentWriteValue.NodeId = nodesToRead[i].NodeId;
                        m_CurrentWriteNodeName = nodesToRead[i].NodeId.ToString();
                        m_CurrentValue = results[i].Value;
                    }

                    // Add status.
                    attributeStatus = StatusCode.LookupSymbolicId(results[i].StatusCode.Code);
                    if (StatusCode.IsBad(results[i].StatusCode))
                    {
                        item.SubItems[1].Text = (String)attributeStatus;
                        item.SubItems[1].ForeColor = Color.Red;
                    }

                    // Add item to the listview.
                    this.lvAttributes.Items.Add(item);

                    // Set column width.
                    this.lvAttributes.Columns[0].Width = 150;
                    this.lvAttributes.Columns[1].Width = 250;
                }
            }
            catch (Exception e)
            {
                // Update status label.
                OnUpdateStatusLabel("Error while processing read results: " + e.Message, false);
            }
        }
        #endregion
                
        #region Private Methods
        /// <summary>
        /// Create read request.
        /// </summary>
        /// <param name="refDescription"></param>
        /// <param name="nodesToRead"></param>
        private void buildAttributeList(ReferenceDescription refDescription, out ReadValueIdCollection nodesToRead)
        {
            // Build list of attributes to read.
            nodesToRead = new ReadValueIdCollection();

            // Add default attributes (for all nodeclasses)
            addAttribute((NodeId)refDescription.NodeId, Attributes.NodeId, nodesToRead);
            addAttribute((NodeId)refDescription.NodeId, Attributes.NodeClass, nodesToRead);
            addAttribute((NodeId)refDescription.NodeId, Attributes.BrowseName, nodesToRead);
            addAttribute((NodeId)refDescription.NodeId, Attributes.DisplayName, nodesToRead);
            addAttribute((NodeId)refDescription.NodeId, Attributes.Description, nodesToRead);
            addAttribute((NodeId)refDescription.NodeId, Attributes.WriteMask, nodesToRead);
            addAttribute((NodeId)refDescription.NodeId, Attributes.UserWriteMask, nodesToRead);

            // Add nodeclass specific attributes
            switch (refDescription.NodeClass)
            {
                case NodeClass.Object:
                    addAttribute((NodeId)refDescription.NodeId, Attributes.EventNotifier, nodesToRead);
                    break;
                case NodeClass.Variable:
                    addAttribute((NodeId)refDescription.NodeId, Attributes.Value, nodesToRead);
                    addAttribute((NodeId)refDescription.NodeId, Attributes.DataType, nodesToRead);
                    addAttribute((NodeId)refDescription.NodeId, Attributes.ValueRank, nodesToRead);
                    addAttribute((NodeId)refDescription.NodeId, Attributes.ArrayDimensions, nodesToRead);
                    addAttribute((NodeId)refDescription.NodeId, Attributes.AccessLevel, nodesToRead);
                    addAttribute((NodeId)refDescription.NodeId, Attributes.UserAccessLevel, nodesToRead);
                    addAttribute((NodeId)refDescription.NodeId, Attributes.MinimumSamplingInterval, nodesToRead);
                    addAttribute((NodeId)refDescription.NodeId, Attributes.Historizing, nodesToRead);
                    break;
                case NodeClass.Method:
                    addAttribute((NodeId)refDescription.NodeId, Attributes.Executable, nodesToRead);
                    addAttribute((NodeId)refDescription.NodeId, Attributes.UserExecutable, nodesToRead);
                    break;
                case NodeClass.ObjectType:
                    addAttribute((NodeId)refDescription.NodeId, Attributes.IsAbstract, nodesToRead);
                    break;
                case NodeClass.VariableType:
                    addAttribute((NodeId)refDescription.NodeId, Attributes.Value, nodesToRead);
                    addAttribute((NodeId)refDescription.NodeId, Attributes.DataType, nodesToRead);
                    addAttribute((NodeId)refDescription.NodeId, Attributes.ValueRank, nodesToRead);
                    addAttribute((NodeId)refDescription.NodeId, Attributes.ArrayDimensions, nodesToRead);
                    addAttribute((NodeId)refDescription.NodeId, Attributes.IsAbstract, nodesToRead);
                    break;
                case NodeClass.ReferenceType:
                    addAttribute((NodeId)refDescription.NodeId, Attributes.IsAbstract, nodesToRead);
                    addAttribute((NodeId)refDescription.NodeId, Attributes.Symmetric, nodesToRead);
                    addAttribute((NodeId)refDescription.NodeId, Attributes.InverseName, nodesToRead);
                    break;
                case NodeClass.DataType:
                    addAttribute((NodeId)refDescription.NodeId, Attributes.IsAbstract, nodesToRead);
                    break;
                case NodeClass.View:
                    addAttribute((NodeId)refDescription.NodeId, Attributes.ContainsNoLoops, nodesToRead);
                    addAttribute((NodeId)refDescription.NodeId, Attributes.EventNotifier, nodesToRead);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Add attribute for a particular node to ReadValueCollection
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeId"></param>
        /// <param name="nodesToRead"></param>
        private void addAttribute(NodeId node, uint attributeId, ReadValueIdCollection nodesToRead)
        {
            // Get NodeId from tree node.
            ReadValueId attributeToRead = new ReadValueId();
            attributeToRead.NodeId = node;
            attributeToRead.AttributeId = attributeId;

            // Populate hashtable if called for the first time
            if (m_hashAttributeNames == null)
            {
                m_hashAttributeNames = new Hashtable();

                m_hashAttributeNames.Add(Attributes.AccessLevel, "AccessLevel");
                m_hashAttributeNames.Add(Attributes.ArrayDimensions, "ArrayDimensions");
                m_hashAttributeNames.Add(Attributes.BrowseName, "BrowseName");
                m_hashAttributeNames.Add(Attributes.ContainsNoLoops, "ContainsNoLoops");
                m_hashAttributeNames.Add(Attributes.DataType, "DataType");
                m_hashAttributeNames.Add(Attributes.Description, "Description");
                m_hashAttributeNames.Add(Attributes.DisplayName, "DisplayName");
                m_hashAttributeNames.Add(Attributes.EventNotifier, "EventNotifier");
                m_hashAttributeNames.Add(Attributes.Executable, "Executable");
                m_hashAttributeNames.Add(Attributes.Historizing, "Historizing");
                m_hashAttributeNames.Add(Attributes.InverseName, "InverseName");
                m_hashAttributeNames.Add(Attributes.IsAbstract, "IsAbstract");
                m_hashAttributeNames.Add(Attributes.MinimumSamplingInterval, "MinimumSamplingInterval");
                m_hashAttributeNames.Add(Attributes.NodeClass, "NodeClass");
                m_hashAttributeNames.Add(Attributes.NodeId, "NodeId");
                m_hashAttributeNames.Add(Attributes.Symmetric, "Symmetric");
                m_hashAttributeNames.Add(Attributes.UserAccessLevel, "UserAccessLevel");
                m_hashAttributeNames.Add(Attributes.UserExecutable, "UserExecutable");
                m_hashAttributeNames.Add(Attributes.UserWriteMask, "UserWriteMask");
                m_hashAttributeNames.Add(Attributes.Value, "Value");
                m_hashAttributeNames.Add(Attributes.ValueRank, "ValueRank");
                m_hashAttributeNames.Add(Attributes.WriteMask, "WriteMask");
            }

            string ret = (string)m_hashAttributeNames[attributeId];

            attributeToRead.Handle = ret;
            nodesToRead.Add(attributeToRead);
        }

        /// <summary>
        /// Display current attribute values.
        /// </summary>
        /// <param name="nodeToRead"></param>
        /// <param name="attrIds"></param>
        /// <param name="results"></param>
        /// /// <param name="response"></param>
        private void updateAttributes(
            NodeId nodeToRead,
            UInt32Collection attrIds,
            DataValueCollection results,
            ResponseHeader response)
        {
            if (attrIds.Count != results.Count)
            {
                // Error case.
                return;
            }

            try
            {
                for (int i = 0; i < attrIds.Count; i++)
                {
                    string attributeName = (string)attrIds[i].ToString();
                    string attributeValue = results[i].ToString();
                    string attributeStatus;

                    // Add the attribute name / value to the list view.
                    ListViewItem item = new ListViewItem(attributeName);

                    // Add the value
                    item.SubItems.Add(attributeValue);

                    if (attrIds[i] == Attributes.Value)
                    {
                        m_CurrentWriteValue = new WriteValue();
                        m_CurrentWriteValue.AttributeId = Attributes.Value;
                        m_CurrentWriteValue.NodeId = nodeToRead;
                        m_CurrentWriteNodeName = nodeToRead.ToString();
                        m_CurrentValue = results[i].Value;
                    }

                    // Add status. 
                    attributeStatus = StatusCode.LookupSymbolicId(results[i].StatusCode.Code);
                    if (StatusCode.IsBad(results[i].StatusCode))
                    {
                        item.SubItems[0].Text = (String)attributeStatus;
                        item.SubItems[0].ForeColor = Color.Red;
                    }

                    // Add item to listview.
                    this.lvAttributes.Items.Add(item);

                    // Fit the width of the nodeid column to the size of the header.
                    this.lvAttributes.Columns[0].Width = -2;
                }
            }
            catch (Exception e)
            {
                // Update status label.
                OnUpdateStatusLabel("Error while processing read results: " + e.Message, false);
            }
        }
        #endregion
    }
}
