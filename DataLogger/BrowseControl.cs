using System;
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
    public partial class BrowseControl : UserControl 
    {
        /// <summary>
        /// Event handler for the event that the selection of the OPC server in the browse tree changed.
        /// </summary>
        public delegate void SelectionChangedEventHandler(TreeNode selectedNode);
        /// <summary>
        /// Use the delegate as event.
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged = null;
        /// <summary>
        /// The selection of the OPC server in the browse tree changed.
        /// </summary>
        public void OnSelectionChanged(TreeNode node)
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(node);
            }
        }

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
        public BrowseControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Fields
        /// <summary>
        /// Provides access to OPC UA server.
        /// </summary>
        private UAClientHelperAPI m_Server;
        /// <summary>
        /// Indicates a rebrowse procedure.
        /// </summary>
        private bool m_RebrowseOnExpandNode = false;
        /// <summary>
        /// Keeps the current tree node.
        /// </summary>
        private TreeNode m_CurrentNode;
        #endregion

        #region Properties
        // Flag to set rebrowse if node is expanded.
        public bool RebrowseOnNodeExpande
        {
            get { return m_RebrowseOnExpandNode; }
            set { m_RebrowseOnExpandNode = value; }
        }
        // Server
        public UAClientHelperAPI Server
        {
            get { return m_Server; }
            set { m_Server = value; }
        }
        #endregion

        #region Calls to ClientAPI
        /// <summary>
        /// Call the Browse service of an UA server.
        /// </summary>
        /// <param name="parentNode">The node of the treeview to browse.</param>
        public int Browse(TreeNode parentNode)
        {
            NodeId nodeToBrowse = null;
            TreeNodeCollection parentCollection = null;

            // Check if we browse from root
            if (parentNode == null)
            {
                nodeToBrowse = new NodeId(Objects.RootFolder, 0);
                // Get all the tree nodes that are assigned to the control
                parentCollection = tvBrowseTree.Nodes;
            }
            else
            {
                // Get the data about the parent tree node
                ReferenceDescription parentRefDescription = (ReferenceDescription)parentNode.Tag;

                if (parentRefDescription == null)
                {
                    return -1;
                }

                nodeToBrowse = (NodeId)parentRefDescription.NodeId;
                parentCollection = parentNode.Nodes;
            }

            ReferenceDescriptionCollection browseResults;
            ReferenceDescription refDesc;
            int result = 0;
            bool bBrowse;

            // Set wait cursor.
            Cursor.Current = Cursors.WaitCursor;

            // Check if we want to browse on the server.
            if (m_RebrowseOnExpandNode)
            {
                bBrowse = true;
            }
            else if (parentNode == null)
            {
                bBrowse = true;
            }
            else if (parentNode.Checked)
            {
                bBrowse = false;
            }
            else
            {
                bBrowse = true;
            }

            // Delete children if required.
            if (bBrowse)
            {
                if (parentNode != null)
                {
                    parentNode.Nodes.Clear();
                }

                try
                {
                    // Call browse service.
                    refDesc = new ReferenceDescription();
                    refDesc.NodeId = nodeToBrowse;
                    browseResults = m_Server.BrowseNode(refDesc);

                    if (browseResults == null)
                    {
                        return -1;
                    }

                    if (result == 0)
                    {
                        // Add children.
                        tvBrowseTree.BeginUpdate();

                        // Mark parent node as browsed.
                        if (parentNode != null)
                        {
                            parentNode.Checked = true;
                        }

                        foreach (ReferenceDescription refDescr in browseResults)
                        {
                            if (refDescr.ReferenceTypeId != ReferenceTypeIds.HasNotifier)
                            {
                                TreeNode node = new TreeNode();
                                node.Text = refDescr.BrowseName.Name.ToString();
                                node.Tag = refDescr;
                                CustomizeTreeNode(ref node);

                                // Add dummy child.
                                TreeNode dummy = new TreeNode("dummy");
                                node.Nodes.Add(dummy);
                                parentCollection.Add(node);
                            }
                        }
                        
                        // Sort the tree nodes of the particular node collection
                        this.SortTreeNode((parentNode == null) ? tvBrowseTree.Nodes : parentNode.Nodes);
                        tvBrowseTree.EndUpdate();

                        // Update status label.
                        OnUpdateStatusLabel("Browse succeeded.", true);
                    }
                    else
                    {
                        // Update status label.
                        OnUpdateStatusLabel("Browse failed. Error: " + result.ToString(), false);
                    }
                }
                catch (Exception e)
                {
                    result = -1;
                    // Update status label.
                    OnUpdateStatusLabel("An exception occured while browsing: " + e.Message, false);
                }
            }

            // Restore default cursor.
            Cursor.Current = Cursors.Default;

            return result;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Set icon for tree node dependent on the type.
        /// </summary>
        /// <param name="node">The current node of the treeview.</param>
        private void CustomizeTreeNode(ref TreeNode node)
        {
            // Get the data about the tree node.
            ReferenceDescription refDescr = (ReferenceDescription)node.Tag;
            
            if (refDescr == null)
            {
                // Error case.
                return;
            }

            // Check for folder.
            if (ExpandedNodeId.ToNodeId(refDescr.TypeDefinition, null) == ObjectTypes.FolderType)
            {
                node.ImageKey = "treefolder";
            }
            // Check for property.
            else if (ExpandedNodeId.ToNodeId(refDescr.ReferenceTypeId, null) == ReferenceTypes.HasProperty)
            {
                node.ImageKey = "property";
            }
            // Check node class.
            else
            {
                switch (refDescr.NodeClass)
                {
                    case NodeClass.Object:
                        node.ImageKey = "object";
                        break;
                    case NodeClass.Variable:
                        node.ImageKey = "variable";
                        break;
                    case NodeClass.Method:
                        node.ImageKey = "method";
                        break;
                    case NodeClass.ObjectType:
                        node.ImageKey = "objecttype";
                        break;
                    case NodeClass.VariableType:
                        node.ImageKey = "variabletype";
                        break;
                    case NodeClass.ReferenceType:
                        node.ImageKey = "reftype";
                        break;
                    case NodeClass.DataType:
                        node.ImageKey = "datatype";
                        break;
                    case NodeClass.View:
                        node.ImageKey = "view";
                        break;
                    default:
                        node.ImageKey = "error";
                        break;
                }
            }
            
            // Set the key of the image.
            node.SelectedImageKey = node.ImageKey;
            node.StateImageKey = node.ImageKey;
        }

        /// <summary>
        /// Sorts all tree nodes in a tree node collection.
        /// </summary>
        /// <param name="nodes">Collection of TreeNodes to be sorted</param>
        private void SortTreeNode(TreeNodeCollection nodes)
        {
            if (nodes.Count > 1)
            {
                TreeNode[] arrNodes = new TreeNode[nodes.Count];

                nodes.CopyTo(arrNodes, 0);
                // Sort the nodes being copied according to (1) node class and (2) text property.
                Array.Sort<TreeNode>(arrNodes, this.CompareTreeNodes);
                // Add the nodes to the collection.
                nodes.Clear();
                nodes.AddRange(arrNodes);
            }
        }

        /// <summary>
        /// Compare method for TreeNode sorting. 
        /// 1st. characteristic: object class (defined by index of image in ImageList).
        /// 2nd. characteristic: Text property of the tree node.
        /// </summary>
        /// <param name="nodeA">tree node to be compared</param>
        /// <param name="nodeB">tree node to be compared</param>
        /// <returns></returns>
        private int CompareTreeNodes(TreeNode nodeA, TreeNode nodeB)
        {
            int imageIndexA = this.tvBrowseTree.ImageList.Images.IndexOfKey(nodeA.ImageKey);
            int imageIndexB = this.tvBrowseTree.ImageList.Images.IndexOfKey(nodeB.ImageKey);
            int compClass = Decimal.Compare(imageIndexA, imageIndexB);

            return (compClass == 0) ? String.Compare(nodeA.Text, nodeB.Text, true) : compClass;
        }
        #endregion

        #region User Actions and Event Handling

        /// <summary>
        /// Event fired before the browse tree will be expanded.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tvBrowseTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // Get current node.
            TreeNode node = e.Node;

            if (node != null)
            {
                // Browse next level.
                Browse(node);
            }
        }

        /// <summary>
        /// Event fired after a node has been selected.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tvBrowseTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Get current node.
            TreeNode node = e.Node;
            // Fire SelectionChanged event.
            OnSelectionChanged(node);
        }

        /// <summary>
        /// Event being fired when an item is dragged over the tree view control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tvBrowseTree_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
        {
            if (((TreeNode)e.Item).Tag.GetType() == typeof(ReferenceDescription))
            {
                // Get the data about the tree node.
                ReferenceDescription reference = (ReferenceDescription)((TreeNode)e.Item).Tag;

                // Allow only variables drag and drop.
                if (reference.NodeId.IsAbsolute || reference.NodeClass != NodeClass.Variable)
                {
                    return;
                }

                // Start the drag and drop action.
                // We have to copy serializable data like strings.
                String sNodeId = reference.NodeId.ToString();

                // The data is copied to the target control.
                DragDropEffects dde = tvBrowseTree.DoDragDrop(sNodeId, DragDropEffects.Copy);
            }
        }

        /// <summary>
        /// Event being fired when an item is being clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tvBrowseTree_MouseDown(object sender, MouseEventArgs e)
        {
            // Context menu(s):
            if (e.Button == MouseButtons.Right)
            {
                // Get item at events position.
                TreeNode node = tvBrowseTree.GetNodeAt(e.X, e.Y);

                // Select this node in the tree view.
                tvBrowseTree.SelectedNode = node;

                // Check if node is valid.
                if (node != null)
                {
                    // Check if node is a method.
                    ReferenceDescription refDescr = node.Tag as ReferenceDescription;

                    if (refDescr != null)
                    {
                        if (refDescr.NodeClass == NodeClass.Variable)
                        {
                            m_CurrentNode = node;
                        }
                        else if (refDescr.NodeClass == NodeClass.Object)
                        {
                            m_CurrentNode = node;
                        }
                        else
                        {
                            m_CurrentNode = node;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
