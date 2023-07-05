namespace DataLogger
{
    partial class formAbout
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
            this.btnOK = new System.Windows.Forms.Button();
            this.labelVersion = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.labelCopyright = new System.Windows.Forms.Label();
            this.labelRight = new System.Windows.Forms.Label();
            this.linkWWW = new System.Windows.Forms.LinkLabel();
            this.linkMail = new System.Windows.Forms.LinkLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(135, 121);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(108, 9);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(42, 13);
            this.labelVersion.TabIndex = 6;
            this.labelVersion.Text = "Version";
            this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::DataLogger.Properties.Resources.RAUFFE_LOGO;
            this.pictureBox.Location = new System.Drawing.Point(5, 22);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(97, 58);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 7;
            this.pictureBox.TabStop = false;
            // 
            // labelCopyright
            // 
            this.labelCopyright.AutoSize = true;
            this.labelCopyright.Location = new System.Drawing.Point(108, 32);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(51, 13);
            this.labelCopyright.TabIndex = 8;
            this.labelCopyright.Text = "Copyright";
            this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelRight
            // 
            this.labelRight.AutoSize = true;
            this.labelRight.Location = new System.Drawing.Point(108, 57);
            this.labelRight.Name = "labelRight";
            this.labelRight.Size = new System.Drawing.Size(93, 13);
            this.labelRight.TabIndex = 9;
            this.labelRight.Text = "All rights reserved.";
            this.labelRight.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // linkWWW
            // 
            this.linkWWW.AutoSize = true;
            this.linkWWW.Location = new System.Drawing.Point(108, 84);
            this.linkWWW.Name = "linkWWW";
            this.linkWWW.Size = new System.Drawing.Size(87, 13);
            this.linkWWW.TabIndex = 11;
            this.linkWWW.TabStop = true;
            this.linkWWW.Text = "https://rauffe.ru/";
            this.linkWWW.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkWWW.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkWWW_LinkClicked);
            // 
            // linkMail
            // 
            this.linkMail.AutoSize = true;
            this.linkMail.Location = new System.Drawing.Point(225, 84);
            this.linkMail.Name = "linkMail";
            this.linkMail.Size = new System.Drawing.Size(64, 13);
            this.linkMail.TabIndex = 12;
            this.linkMail.TabStop = true;
            this.linkMail.Text = "info@k-4.su";
            this.linkMail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkMail_LinkClicked);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Location = new System.Drawing.Point(5, 110);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(334, 5);
            this.panel1.TabIndex = 13;
            // 
            // formAbout
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 155);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.linkMail);
            this.Controls.Add(this.linkWWW);
            this.Controls.Add(this.labelRight);
            this.Controls.Add(this.labelCopyright);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.Load += new System.EventHandler(this.frmAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label labelCopyright;
        private System.Windows.Forms.Label labelRight;
        private System.Windows.Forms.LinkLabel linkWWW;
        private System.Windows.Forms.LinkLabel linkMail;
        private System.Windows.Forms.Panel panel1;
    }
}