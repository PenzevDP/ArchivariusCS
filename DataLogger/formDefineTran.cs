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

namespace Archivarius
{
    public partial class formDefineTran : Form
    {
        private DataSet copyTransaction;
        private DataSet copyTransactionOPCUA;


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
            NLogger.logger.Trace(copyTransactionOPCUA.Tables["TransactionTable"].Columns[9].ColumnName.ToString());


            navigatorTran.BindingSource = bindingTran;
            navigatorTranOPC.BindingSource = bindingTranOPC;
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
    }
}
