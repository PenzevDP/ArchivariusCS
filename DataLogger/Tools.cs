using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace Tools
{
    public delegate void SetEnableStripItemCallback(Control control, ToolStripItem item, bool value);
    public delegate void SetImageStripItemCallback(Control control, ToolStripItem item, Image value);
    public delegate void SetTextStripItemCallback(Control control, ToolStripItem item, string value);
    public delegate void SetVisibleStripItemCallback(Control control, ToolStripItem item, bool value);
    public delegate void SetEnableControlCallback(Control control, bool value);
    public delegate void SetAllowUserToAddRowsDataGridViewCallback(DataGridView dataGridView, bool value);    
    public delegate void SetReadOnlyDataGridViewCallback(DataGridView dataGridView, bool value);
    public delegate void CancelEditDataGridViewCallback(DataGridView dataGridView);
    public delegate void SetTextControlCallback(Control control, string value);

    public static class SafeThread
    {
        public static void SetEnableStripItem(Control control, ToolStripItem item, bool value)
        {
            if (control.InvokeRequired)
            {
                SetEnableStripItemCallback callback = new SetEnableStripItemCallback(SetEnableStripItem);
                control.Invoke(callback, control, item, value);
            }
            else
            {
                item.Enabled = value;
            }
        }
        public static void SetImageStripItem(Control control, ToolStripItem item, Image value)
        {
            if (control.InvokeRequired)
            {
                SetImageStripItemCallback callback = new SetImageStripItemCallback(SetImageStripItem);
                control.Invoke(callback, control, item, value);
            }
            else
            {
                item.Image = value;
            }
        }
        public static void SetTextStripItem(Control control, ToolStripItem item, string value)
        {
            if (control.InvokeRequired)
            {
                SetTextStripItemCallback callback = new SetTextStripItemCallback(SetTextStripItem);
                control.Invoke(callback, control, item, value);
            }
            else
            {
                item.Text = value;
            }
        }
        public static void SetVisibleStripItem(Control control, ToolStripItem item, bool value)
        {
            if (control.InvokeRequired)
            {
                SetVisibleStripItemCallback callback = new SetVisibleStripItemCallback(SetVisibleStripItem);
                control.Invoke(callback, control, item, value);
            }
            else
            {
                item.Visible = value;
            }
        }
        public static void SetEnableControl(Control control, bool value)
        {
            if (control.InvokeRequired)
            {
                SetEnableControlCallback callback = new SetEnableControlCallback(SetEnableControl);
                control.Invoke(callback, control, value);
            }
            else
            {
                control.Enabled = value;                
            }
        }
        public static void SetAllowUserToAddRowsDataGridView(DataGridView dataGridView, bool value)
        {
            if (dataGridView.InvokeRequired)
            {
                SetAllowUserToAddRowsDataGridViewCallback callback = new SetAllowUserToAddRowsDataGridViewCallback(SetAllowUserToAddRowsDataGridView);
                dataGridView.Invoke(callback, dataGridView, value);
            }
            else
            {
                dataGridView.AllowUserToAddRows = value;
            }
        }
        public static void SetReadOnlyDataGridView(DataGridView dataGridView, bool value)
        {        
            if (dataGridView.InvokeRequired)
            {
                SetReadOnlyDataGridViewCallback callback = new SetReadOnlyDataGridViewCallback(SetReadOnlyDataGridView);
                dataGridView.Invoke(callback, dataGridView, value);
            }
            else
            {
                dataGridView.ReadOnly = value;
            }
        }
        public static void CancelEditDataGridView(DataGridView dataGridView)
        {
            if (dataGridView.InvokeRequired)
            {
                CancelEditDataGridViewCallback callback = new CancelEditDataGridViewCallback(CancelEditDataGridView);
                dataGridView.Invoke(callback, dataGridView);
            }
            else
            {
                dataGridView.CancelEdit();
            }
        }
        public static void SetTextControl(Control control, string value)
        {
            if (control.InvokeRequired)
            {
                SetTextControlCallback callback = new SetTextControlCallback(SetTextControl);
                control.Invoke(callback, control, value);
            }
            else
            {
                control.Text = value;
            }
        }
    }

    public class SingleEventLogArgs : EventArgs
    {
        private string message;

        public SingleEventLogArgs(string message)
        {
            this.message = message;
        }
        public string Message
        {
            get
            {
                return message;
            }
        }
    }
    public delegate void SingleEventLogHandler(object sender, SingleEventLogArgs e);

    public class SingleEventLog
    {
        private string _prevMessage = "";
        private EventLog _eventLog = null;

        public SingleEventLog(string source)
        {
            _eventLog = new EventLog();
            _eventLog.Source = source;
           
        }

        public void WriteEntry(string message)
        {
            if (string.Compare(_prevMessage, message, true) != 0)
            {
                try
                {
                    _eventLog.WriteEntry(message);
                }
                catch
                {

                }
                _prevMessage = message;
                if (NewMessage != null) NewMessage(this, new SingleEventLogArgs(message));
            }
        }

        public event SingleEventLogHandler NewMessage;
    }
    
    public class WaitCursor : IDisposable
    {
        Cursor m_cursorOld;
        bool _disposedValue = false; // To detect redundant call 

        /// <summary>
        /// .cteur
        /// </summary>
        public WaitCursor()
        {
            m_cursorOld = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
                Cursor.Current = m_cursorOld;
            _disposedValue = true;
        }
    }
}