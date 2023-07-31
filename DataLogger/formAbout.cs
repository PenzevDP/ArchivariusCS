using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using DataManager;
//using SQLDataSources;

namespace Archivarius
{
    public partial class formAbout : Form
    {
        /// <summary>
        /// Gets the assembly copyright.
        /// </summary>
        /// <value>The assembly copyright.</value>
        private string AssemblyCopyright
        {
            get
           {
               // Get all Copyright attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                // If there aren't any Copyright attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Copyright attribute, return its value
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }
        
        public formAbout()
            
        {
            NLogger.logger.Trace("Service. formAbout has initialized");
            InitializeComponent();
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            Text += " " + Application.ProductName;
            labelVersion.Text += " " + Application.ProductVersion;
            labelCopyright.Text = AssemblyCopyright;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            NLogger.logger.Trace("Service. formAbout has Closed");
            Close();
         
        }

        private void linkWWW_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel link = sender as LinkLabel;

            try
            {
                link.LinkVisited = true;
                System.Diagnostics.Process.Start(link.Text);
            }
            catch
            {

            }
        }

        private void linkMail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel link = sender as LinkLabel;

            try
            {
                link.LinkVisited = true;
                System.Diagnostics.Process.Start("mailto:" + link.Text);                
            }
            catch
            {

            }
        }
    }
}
