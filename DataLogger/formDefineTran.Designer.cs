namespace DataLogger
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formDefineTran));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.dataGridTrn = new System.Windows.Forms.DataGridView();
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
            this.bindingTran = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTrn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navigatorTran)).BeginInit();
            this.navigatorTran.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingTran)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(841, 498);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(721, 498);
            this.btnApply.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(100, 28);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
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
            this.dataGridTrn.Location = new System.Drawing.Point(3, 34);
            this.dataGridTrn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridTrn.Name = "dataGridTrn";
            this.dataGridTrn.RowHeadersWidth = 51;
            this.dataGridTrn.Size = new System.Drawing.Size(965, 449);
            this.dataGridTrn.TabIndex = 5;
            this.dataGridTrn.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridTrn_DataError);
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
            this.navigatorTran.Location = new System.Drawing.Point(0, 0);
            this.navigatorTran.MoveFirstItem = this.navigatorMoveFirstItem;
            this.navigatorTran.MoveLastItem = this.navigatorMoveLastItem;
            this.navigatorTran.MoveNextItem = this.navigatorMoveNextItem;
            this.navigatorTran.MovePreviousItem = this.navigatorMovePreviousItem;
            this.navigatorTran.Name = "navigatorTran";
            this.navigatorTran.PositionItem = this.navigatorPositionItem;
            this.navigatorTran.Size = new System.Drawing.Size(971, 27);
            this.navigatorTran.TabIndex = 6;
            // 
            // navigatorAddNewItem
            // 
            this.navigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.navigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("navigatorAddNewItem.Image")));
            this.navigatorAddNewItem.Name = "navigatorAddNewItem";
            this.navigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.navigatorAddNewItem.Size = new System.Drawing.Size(29, 24);
            this.navigatorAddNewItem.Text = "Add new";
            // 
            // navigatorCountItem
            // 
            this.navigatorCountItem.Name = "navigatorCountItem";
            this.navigatorCountItem.Size = new System.Drawing.Size(55, 24);
            this.navigatorCountItem.Text = "для {0}";
            this.navigatorCountItem.ToolTipText = "Total number of items";
            // 
            // navigatorDeleteItem
            // 
            this.navigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.navigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("navigatorDeleteItem.Image")));
            this.navigatorDeleteItem.Name = "navigatorDeleteItem";
            this.navigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.navigatorDeleteItem.Size = new System.Drawing.Size(29, 24);
            this.navigatorDeleteItem.Text = "Delete";
            // 
            // navigatorMoveFirstItem
            // 
            this.navigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.navigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("navigatorMoveFirstItem.Image")));
            this.navigatorMoveFirstItem.Name = "navigatorMoveFirstItem";
            this.navigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.navigatorMoveFirstItem.Size = new System.Drawing.Size(29, 24);
            this.navigatorMoveFirstItem.Text = "Move first";
            // 
            // navigatorMovePreviousItem
            // 
            this.navigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.navigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("navigatorMovePreviousItem.Image")));
            this.navigatorMovePreviousItem.Name = "navigatorMovePreviousItem";
            this.navigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.navigatorMovePreviousItem.Size = new System.Drawing.Size(29, 24);
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
            this.navigatorPositionItem.Name = "navigatorPositionItem";
            this.navigatorPositionItem.Size = new System.Drawing.Size(65, 27);
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
            this.navigatorMoveNextItem.Size = new System.Drawing.Size(29, 24);
            this.navigatorMoveNextItem.Text = "Move next";
            // 
            // navigatorMoveLastItem
            // 
            this.navigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.navigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("navigatorMoveLastItem.Image")));
            this.navigatorMoveLastItem.Name = "navigatorMoveLastItem";
            this.navigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.navigatorMoveLastItem.Size = new System.Drawing.Size(29, 24);
            this.navigatorMoveLastItem.Text = "Move last";
            // 
            // navigatorSeparator2
            // 
            this.navigatorSeparator2.Name = "navigatorSeparator2";
            this.navigatorSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // formDefineTran
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(971, 534);
            this.Controls.Add(this.navigatorTran);
            this.Controls.Add(this.dataGridTrn);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimizeBox = false;
            this.Name = "formDefineTran";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Define Transactions";
            this.Load += new System.EventHandler(this.frmDefineTrn_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTrn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navigatorTran)).EndInit();
            this.navigatorTran.ResumeLayout(false);
            this.navigatorTran.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingTran)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.DataGridView dataGridTrn;
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
        private System.Windows.Forms.BindingSource bindingTran;

    }
}