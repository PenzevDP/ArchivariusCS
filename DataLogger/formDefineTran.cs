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

namespace DataLogger
{
    public partial class formDefineTran : Form
    {
        private DataSet copyTransaction;

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
                SafeThread.SetAllowUserToAddRowsDataGridView(dataGridTrn, true);
                SafeThread.SetVisibleStripItem(navigatorTran, navigatorAddNewItem, true);
                SafeThread.SetVisibleStripItem(navigatorTran, navigatorDeleteItem, true);
            }
            else
            {                
                SafeThread.SetEnableControl(btnApply, false);
                SafeThread.CancelEditDataGridView(dataGridTrn);
                SafeThread.SetReadOnlyDataGridView(dataGridTrn, true);
                SafeThread.SetAllowUserToAddRowsDataGridView(dataGridTrn, false);
                SafeThread.SetVisibleStripItem(navigatorTran, navigatorAddNewItem, false);
                SafeThread.SetVisibleStripItem(navigatorTran, navigatorDeleteItem, false);
            }
        }

        private void frmDefineTrn_Load(object sender, EventArgs e)
        {
            copyTransaction = Config.Sets.TransactionBase.Copy();
            bindingTran.DataSource = copyTransaction.Tables["TransactionTable"];
            dataGridTrn.DataSource = bindingTran;
            navigatorTran.BindingSource = bindingTran;
            UpdateConfigState(Config.State);
            Config.StateChange += ConfigStateChange;
        }

        private void ConfigStateChange(object sender, ConfigStateEventArgs e)
        {
            UpdateConfigState(e.State);
        }


        private void btnApply_Click(object sender, EventArgs e)
        {
            Config.Sets.TransactionBase = copyTransaction.Copy();
            Config.Save();
            this.Close();
        }

        private void dataGridTrn_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, Application.ProductName);
            e.Cancel = true;       
        }
    }
}
