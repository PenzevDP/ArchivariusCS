namespace Archivarius
{
    partial class formDefineTran
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formDefineTran));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.bindingTran = new System.Windows.Forms.BindingSource(this.components);
            this.bindingTranOPC = new System.Windows.Forms.BindingSource(this.components);
            this.Tab = new System.Windows.Forms.TabControl();
            this.DB_transaction = new System.Windows.Forms.TabPage();
            this.navigatorTran = new System.Windows.Forms.BindingNavigator(this.components);
            this.navigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.navigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.navigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.navigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.navigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.navigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.navigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.navigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.navigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.navigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.navigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.dataGridTrn = new System.Windows.Forms.DataGridView();
            this.OPCUA_transaction = new System.Windows.Forms.TabPage();
            this.dataGridtrnOPC = new System.Windows.Forms.DataGridView();
            this.navigatorTranOPC = new System.Windows.Forms.BindingNavigator(this.components);
            this.navigatorAddNewItemOPC = new System.Windows.Forms.ToolStripButton();
            this.navigatorCountItemOPC = new System.Windows.Forms.ToolStripLabel();
            this.navigatorDeleteItemOPC = new System.Windows.Forms.ToolStripButton();
            this.navigatorMoveFirstItemOPC = new System.Windows.Forms.ToolStripButton();
            this.navigatorMovePreviousItemOPC = new System.Windows.Forms.ToolStripButton();
            this.navigatorSeparatorOPC = new System.Windows.Forms.ToolStripSeparator();
            this.navigatorPositionItemOPC = new System.Windows.Forms.ToolStripTextBox();
            this.navigatorSeparator1OPC = new System.Windows.Forms.ToolStripSeparator();
            this.navigatorMoveNextItemOPC = new System.Windows.Forms.ToolStripButton();
            this.navigatorMoveLastItemOPC = new System.Windows.Forms.ToolStripButton();
            this.navigatorSeparator2OPC = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.bindingTran)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingTranOPC)).BeginInit();
            this.Tab.SuspendLayout();
            this.DB_transaction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.navigatorTran)).BeginInit();
            this.navigatorTran.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTrn)).BeginInit();
            this.OPCUA_transaction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridtrnOPC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navigatorTranOPC)).BeginInit();
            this.navigatorTranOPC.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(631, 405);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(541, 405);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // Tab
            // 
            this.Tab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tab.Controls.Add(this.DB_transaction);
            this.Tab.Controls.Add(this.OPCUA_transaction);
            this.Tab.Location = new System.Drawing.Point(0, 0);
            this.Tab.Name = "Tab";
            this.Tab.SelectedIndex = 0;
            this.Tab.Size = new System.Drawing.Size(728, 399);
            this.Tab.TabIndex = 5;
            // 
            // DB_transaction
            // 
            this.DB_transaction.Controls.Add(this.navigatorTran);
            this.DB_transaction.Controls.Add(this.dataGridTrn);
            this.DB_transaction.Location = new System.Drawing.Point(4, 22);
            this.DB_transaction.Name = "DB_transaction";
            this.DB_transaction.Padding = new System.Windows.Forms.Padding(3);
            this.DB_transaction.Size = new System.Drawing.Size(720, 373);
            this.DB_transaction.TabIndex = 0;
            this.DB_transaction.Text = "DB_transaction";
            this.DB_transaction.UseVisualStyleBackColor = true;
            // 
            // navigatorTran
            // 
            this.navigatorTran.AddNewItem = this.navigatorAddNewItem;
            this.navigatorTran.CountItem = this.navigatorCountItem;
            this.navigatorTran.DeleteItem = this.navigatorDeleteItem;
            this.navigatorTran.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.navigatorTran.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.navigatorMoveFirstItem,
            this.navigatorMovePreviousItem,
            this.navigatorSeparator,
            this.navigatorPositionItem,
            this.navigatorCountItem,
            this.navigatorSeparator1,
            this.navigatorMoveNextItem,
            this.navigatorMoveLastItem,
            this.navigatorSeparator2,
            this.navigatorAddNewItem,
            this.navigatorDeleteItem});
            this.navigatorTran.Location = new System.Drawing.Point(3, 3);
            this.navigatorTran.MoveFirstItem = this.navigatorMoveFirstItem;
            this.navigatorTran.MoveLastItem = this.navigatorMoveLastItem;
            this.navigatorTran.MoveNextItem = this.navigatorMoveNextItem;
            this.navigatorTran.MovePreviousItem = this.navigatorMovePreviousItem;
            this.navigatorTran.Name = "navigatorTran";
            this.navigatorTran.PositionItem = this.navigatorPositionItem;
            this.navigatorTran.Size = new System.Drawing.Size(714, 27);
            this.navigatorTran.TabIndex = 8;
            // 
            // navigatorAddNewItem
            // 
            this.navigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.navigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("navigatorAddNewItem.Image")));
            this.navigatorAddNewItem.Name = "navigatorAddNewItem";
            this.navigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.navigatorAddNewItem.Size = new System.Drawing.Size(24, 24);
            this.navigatorAddNewItem.Text = "Add new";
            this.navigatorAddNewItem.Click += new System.EventHandler(this.navigatorAddNewItem_Click);
            // 
            // navigatorCountItem
            // 
            this.navigatorCountItem.Name = "navigatorCountItem";
            this.navigatorCountItem.Size = new System.Drawing.Size(43, 24);
            this.navigatorCountItem.Text = "для {0}";
            this.navigatorCountItem.ToolTipText = "Total number of items";
            // 
            // navigatorDeleteItem
            // 
            this.navigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.navigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("navigatorDeleteItem.Image")));
            this.navigatorDeleteItem.Name = "navigatorDeleteItem";
            this.navigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.navigatorDeleteItem.Size = new System.Drawing.Size(24, 24);
            this.navigatorDeleteItem.Text = "Delete";
            // 
            // navigatorMoveFirstItem
            // 
            this.navigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.navigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("navigatorMoveFirstItem.Image")));
            this.navigatorMoveFirstItem.Name = "navigatorMoveFirstItem";
            this.navigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.navigatorMoveFirstItem.Size = new System.Drawing.Size(24, 24);
            this.navigatorMoveFirstItem.Text = "Move first";
            this.navigatorMoveFirstItem.Click += new System.EventHandler(this.navigatorMoveFirstItem_Click);
            // 
            // navigatorMovePreviousItem
            // 
            this.navigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.navigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("navigatorMovePreviousItem.Image")));
            this.navigatorMovePreviousItem.Name = "navigatorMovePreviousItem";
            this.navigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.navigatorMovePreviousItem.Size = new System.Drawing.Size(24, 24);
            this.navigatorMovePreviousItem.Text = "Move previous";
            // 
            // navigatorSeparator
            // 
            this.navigatorSeparator.Name = "navigatorSeparator";
            this.navigatorSeparator.Size = new System.Drawing.Size(6, 27);
            // 
            // navigatorPositionItem
            // 
            this.navigatorPositionItem.AccessibleName = "Position";
            this.navigatorPositionItem.AutoSize = false;
            this.navigatorPositionItem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.navigatorPositionItem.Name = "navigatorPositionItem";
            this.navigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.navigatorPositionItem.Text = "0";
            this.navigatorPositionItem.ToolTipText = "Current position";
            // 
            // navigatorSeparator1
            // 
            this.navigatorSeparator1.Name = "navigatorSeparator1";
            this.navigatorSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // navigatorMoveNextItem
            // 
            this.navigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.navigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("navigatorMoveNextItem.Image")));
            this.navigatorMoveNextItem.Name = "navigatorMoveNextItem";
            this.navigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.navigatorMoveNextItem.Size = new System.Drawing.Size(24, 24);
            this.navigatorMoveNextItem.Text = "Move next";
            // 
            // navigatorMoveLastItem
            // 
            this.navigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.navigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("navigatorMoveLastItem.Image")));
            this.navigatorMoveLastItem.Name = "navigatorMoveLastItem";
            this.navigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.navigatorMoveLastItem.Size = new System.Drawing.Size(24, 24);
            this.navigatorMoveLastItem.Text = "Move last";
            // 
            // navigatorSeparator2
            // 
            this.navigatorSeparator2.Name = "navigatorSeparator2";
            this.navigatorSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // dataGridTrn
            // 
            this.dataGridTrn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridTrn.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridTrn.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridTrn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridTrn.Location = new System.Drawing.Point(0, 27);
            this.dataGridTrn.Name = "dataGridTrn";
            this.dataGridTrn.RowHeadersWidth = 51;
            this.dataGridTrn.Size = new System.Drawing.Size(726, 345);
            this.dataGridTrn.TabIndex = 7;
            this.dataGridTrn.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridTrn_CellContentClick);
            // 
            // OPCUA_transaction
            // 
            this.OPCUA_transaction.Controls.Add(this.dataGridtrnOPC);
            this.OPCUA_transaction.Controls.Add(this.navigatorTranOPC);
            this.OPCUA_transaction.Location = new System.Drawing.Point(4, 22);
            this.OPCUA_transaction.Name = "OPCUA_transaction";
            this.OPCUA_transaction.Padding = new System.Windows.Forms.Padding(3);
            this.OPCUA_transaction.Size = new System.Drawing.Size(720, 373);
            this.OPCUA_transaction.TabIndex = 1;
            this.OPCUA_transaction.Text = "OPCUA_transaction";
            this.OPCUA_transaction.UseVisualStyleBackColor = true;
            this.OPCUA_transaction.Click += new System.EventHandler(this.OPCUA_transaction_Click);
            // 
            // dataGridtrnOPC
            // 
            this.dataGridtrnOPC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridtrnOPC.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridtrnOPC.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridtrnOPC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridtrnOPC.Location = new System.Drawing.Point(3, 33);
            this.dataGridtrnOPC.Name = "dataGridtrnOPC";
            this.dataGridtrnOPC.RowHeadersWidth = 51;
            this.dataGridtrnOPC.Size = new System.Drawing.Size(714, 337);
            this.dataGridtrnOPC.TabIndex = 9;
            // 
            // navigatorTranOPC
            // 
            this.navigatorTranOPC.AddNewItem = this.navigatorAddNewItemOPC;
            this.navigatorTranOPC.CountItem = this.navigatorCountItemOPC;
            this.navigatorTranOPC.DeleteItem = this.navigatorDeleteItemOPC;
            this.navigatorTranOPC.Dock = System.Windows.Forms.DockStyle.None;
            this.navigatorTranOPC.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.navigatorTranOPC.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.navigatorMoveFirstItemOPC,
            this.navigatorMovePreviousItemOPC,
            this.navigatorSeparatorOPC,
            this.navigatorPositionItemOPC,
            this.navigatorCountItemOPC,
            this.navigatorSeparator1OPC,
            this.navigatorMoveNextItemOPC,
            this.navigatorMoveLastItemOPC,
            this.navigatorSeparator2OPC,
            this.navigatorAddNewItemOPC,
            this.navigatorDeleteItemOPC});
            this.navigatorTranOPC.Location = new System.Drawing.Point(3, 3);
            this.navigatorTranOPC.MoveFirstItem = this.navigatorMoveFirstItemOPC;
            this.navigatorTranOPC.MoveLastItem = this.navigatorMoveLastItemOPC;
            this.navigatorTranOPC.MoveNextItem = this.navigatorMoveNextItemOPC;
            this.navigatorTranOPC.MovePreviousItem = this.navigatorMovePreviousItemOPC;
            this.navigatorTranOPC.Name = "navigatorTranOPC";
            this.navigatorTranOPC.PositionItem = null;
            this.navigatorTranOPC.Size = new System.Drawing.Size(269, 27);
            this.navigatorTranOPC.TabIndex = 8;
            // 
            // navigatorAddNewItemOPC
            // 
            this.navigatorAddNewItemOPC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.navigatorAddNewItemOPC.Image = ((System.Drawing.Image)(resources.GetObject("navigatorAddNewItemOPC.Image")));
            this.navigatorAddNewItemOPC.Name = "navigatorAddNewItemOPC";
            this.navigatorAddNewItemOPC.RightToLeftAutoMirrorImage = true;
            this.navigatorAddNewItemOPC.Size = new System.Drawing.Size(24, 24);
            this.navigatorAddNewItemOPC.Text = "Add new";
            // 
            // navigatorCountItemOPC
            // 
            this.navigatorCountItemOPC.Name = "navigatorCountItemOPC";
            this.navigatorCountItemOPC.Size = new System.Drawing.Size(43, 24);
            this.navigatorCountItemOPC.Text = "для {0}";
            this.navigatorCountItemOPC.ToolTipText = "Total number of items";
            // 
            // navigatorDeleteItemOPC
            // 
            this.navigatorDeleteItemOPC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.navigatorDeleteItemOPC.Image = ((System.Drawing.Image)(resources.GetObject("navigatorDeleteItemOPC.Image")));
            this.navigatorDeleteItemOPC.Name = "navigatorDeleteItemOPC";
            this.navigatorDeleteItemOPC.RightToLeftAutoMirrorImage = true;
            this.navigatorDeleteItemOPC.Size = new System.Drawing.Size(24, 24);
            this.navigatorDeleteItemOPC.Text = "Delete";
            // 
            // navigatorMoveFirstItemOPC
            // 
            this.navigatorMoveFirstItemOPC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.navigatorMoveFirstItemOPC.Image = ((System.Drawing.Image)(resources.GetObject("navigatorMoveFirstItemOPC.Image")));
            this.navigatorMoveFirstItemOPC.Name = "navigatorMoveFirstItemOPC";
            this.navigatorMoveFirstItemOPC.RightToLeftAutoMirrorImage = true;
            this.navigatorMoveFirstItemOPC.Size = new System.Drawing.Size(24, 24);
            this.navigatorMoveFirstItemOPC.Click += new System.EventHandler(this.navigatorMoveFirstItem_Click);
            // 
            // navigatorMovePreviousItemOPC
            // 
            this.navigatorMovePreviousItemOPC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.navigatorMovePreviousItemOPC.Image = ((System.Drawing.Image)(resources.GetObject("navigatorMovePreviousItemOPC.Image")));
            this.navigatorMovePreviousItemOPC.Name = "navigatorMovePreviousItemOPC";
            this.navigatorMovePreviousItemOPC.RightToLeftAutoMirrorImage = true;
            this.navigatorMovePreviousItemOPC.Size = new System.Drawing.Size(24, 24);
            this.navigatorMovePreviousItemOPC.Text = "Move previous";
            // 
            // navigatorSeparatorOPC
            // 
            this.navigatorSeparatorOPC.Name = "navigatorSeparatorOPC";
            this.navigatorSeparatorOPC.Size = new System.Drawing.Size(6, 27);
            // 
            // navigatorPositionItemOPC
            // 
            this.navigatorPositionItemOPC.AccessibleName = "Position";
            this.navigatorPositionItemOPC.AutoSize = false;
            this.navigatorPositionItemOPC.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.navigatorPositionItemOPC.Name = "navigatorPositionItemOPC";
            this.navigatorPositionItemOPC.Size = new System.Drawing.Size(50, 23);
            this.navigatorPositionItemOPC.Text = "0";
            this.navigatorPositionItemOPC.ToolTipText = "Current position";
            // 
            // navigatorSeparator1OPC
            // 
            this.navigatorSeparator1OPC.Name = "navigatorSeparator1OPC";
            this.navigatorSeparator1OPC.Size = new System.Drawing.Size(6, 27);
            // 
            // navigatorMoveNextItemOPC
            // 
            this.navigatorMoveNextItemOPC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.navigatorMoveNextItemOPC.Image = ((System.Drawing.Image)(resources.GetObject("navigatorMoveNextItemOPC.Image")));
            this.navigatorMoveNextItemOPC.Name = "navigatorMoveNextItemOPC";
            this.navigatorMoveNextItemOPC.RightToLeftAutoMirrorImage = true;
            this.navigatorMoveNextItemOPC.Size = new System.Drawing.Size(24, 24);
            this.navigatorMoveNextItemOPC.Text = "Move next";
            // 
            // navigatorMoveLastItemOPC
            // 
            this.navigatorMoveLastItemOPC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.navigatorMoveLastItemOPC.Image = ((System.Drawing.Image)(resources.GetObject("navigatorMoveLastItemOPC.Image")));
            this.navigatorMoveLastItemOPC.Name = "navigatorMoveLastItemOPC";
            this.navigatorMoveLastItemOPC.RightToLeftAutoMirrorImage = true;
            this.navigatorMoveLastItemOPC.Size = new System.Drawing.Size(24, 24);
            this.navigatorMoveLastItemOPC.Text = "Move last";
            // 
            // navigatorSeparator2OPC
            // 
            this.navigatorSeparator2OPC.Name = "navigatorSeparator2OPC";
            this.navigatorSeparator2OPC.Size = new System.Drawing.Size(6, 27);
            // 
            // formDefineTran
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(728, 434);
            this.Controls.Add(this.Tab);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "formDefineTran";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Define Transactions";
            this.Load += new System.EventHandler(this.frmDefineTrn_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingTran)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingTranOPC)).EndInit();
            this.Tab.ResumeLayout(false);
            this.DB_transaction.ResumeLayout(false);
            this.DB_transaction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.navigatorTran)).EndInit();
            this.navigatorTran.ResumeLayout(false);
            this.navigatorTran.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTrn)).EndInit();
            this.OPCUA_transaction.ResumeLayout(false);
            this.OPCUA_transaction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridtrnOPC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navigatorTranOPC)).EndInit();
            this.navigatorTranOPC.ResumeLayout(false);
            this.navigatorTranOPC.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.BindingSource bindingTran;
        private System.Windows.Forms.BindingSource bindingTranOPC;
        private System.Windows.Forms.TabControl Tab;
        private System.Windows.Forms.TabPage DB_transaction;
        private System.Windows.Forms.BindingNavigator navigatorTran;
        private System.Windows.Forms.ToolStripButton navigatorAddNewItem;
        private System.Windows.Forms.ToolStripLabel navigatorCountItem;
        private System.Windows.Forms.ToolStripButton navigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton navigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton navigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator navigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox navigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator navigatorSeparator1;
        private System.Windows.Forms.ToolStripButton navigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton navigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator navigatorSeparator2;
        private System.Windows.Forms.DataGridView dataGridTrn;
        private System.Windows.Forms.TabPage OPCUA_transaction;
        private System.Windows.Forms.BindingNavigator navigatorTranOPC;
        private System.Windows.Forms.DataGridView dataGridtrnOPC;
        private System.Windows.Forms.ToolStripButton navigatorMoveFirstItemOPC;
        private System.Windows.Forms.ToolStripButton navigatorMovePreviousItemOPC;
        private System.Windows.Forms.ToolStripSeparator navigatorSeparatorOPC;
        private System.Windows.Forms.ToolStripTextBox navigatorPositionItemOPC;
        private System.Windows.Forms.ToolStripLabel navigatorCountItemOPC;
        private System.Windows.Forms.ToolStripSeparator navigatorSeparator1OPC;
        private System.Windows.Forms.ToolStripButton navigatorMoveNextItemOPC;
        private System.Windows.Forms.ToolStripButton navigatorMoveLastItemOPC;
        private System.Windows.Forms.ToolStripSeparator navigatorSeparator2OPC;
        private System.Windows.Forms.ToolStripButton navigatorAddNewItemOPC;
        private System.Windows.Forms.ToolStripButton navigatorDeleteItemOPC;
    }
}