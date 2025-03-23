using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

namespace BilderalbumCS
{
    public partial class DBQuery : Form
    {
        [Serializable]
        public class DBQueryData
        {
            private string m_Query;
            public string Query
            {
                get
                {
                    return m_Query;
                }
                set
                {
                    m_Query = value;
                    Modified = true;
                }
            }
            private DBConnection.DBConnectionData m_Conn;
            public DBConnection.DBConnectionData ConnString
            {
                get
                {
                    return m_Conn;
                }
                set
                {
                    m_Conn = value;
                    Modified = true;
                }
            }
            [NonSerialized]
            public bool Modified;
        }

        private BindingSource bindingSource1 = new BindingSource();
        private OleDbDataAdapter adapter = new OleDbDataAdapter();
        private DBQueryData m_Data = new DBQueryData();
        private string m_FileName;
        private DBConnection m_ConnHelper = new DBConnection();

        public DBQuery()
        {
            InitializeComponent();
            bindingNavigator1.BindingSource = bindingSource1;
            this.textBox1.DataBindings.Add("Text", m_Data, "Query");  //??funktioniert nicht in beide Richtungen
            SetQuery("");
        }
        public void SetQuery(string Qry)
        {
            m_Data.Query = Qry;
            this.textBox1.Text = m_Data.Query;
        }
        public void SetConnString(DBConnection.DBConnectionData ConnString)
        {
            m_Data.ConnString = ConnString;
            m_ConnHelper.SetCfgData(m_Data.ConnString);
        }
        private DataTable GetData(string sqlCommand)
        {
            DataTable table = new DataTable();
            try
            {
                //string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=L:\\Projects\\wxWindows_Projects\\Bilderalbum\\Bilderalbum.mdb;Persist Security Info=True";

                if (!m_ConnHelper.ConnectionOK())
                {
                    m_ConnHelper.ConnectToDB();
                };
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                OleDbCommand command = new OleDbCommand(sqlCommand, m_ConnHelper.GetConnection());
                //OleDbCommand UpdCmd = new OleDbCommand("UPDATE `Images` SET `FileName` = ?, `FilePath` = ?, `Source` = ?, `DateAdded` = ?, `Rating` = ?, `Description` = ? WHERE ((`IDImage` = ?) AND ((? = 1 AND `FileName` IS NULL) OR (`FileName` = ?)) AND ((? = 1 AND `FilePath` IS NULL) OR (`FilePath` = ?)) AND ((? = 1 AND `Source` IS NULL) OR (`Source` = ?)) AND ((? = 1 AND `DateAdded` IS NULL) OR (`DateAdded` = ?)) AND ((? = 1 AND `Rating` IS NULL) OR (`Rating` = ?)) AND ((? = 1 AND `Description` IS NULL) OR (`Description` = ?)))", northwindConnection);
                //adapter = new OleDbDataAdapter();
                adapter.SelectCommand = command;

                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                //OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(adapter);

                /*adapter.UpdateCommand = UpdCmd;
                adapter.UpdateCommand.Parameters.Add("@FileName",
                OleDbType.VarChar, 255, "FileName");
                adapter.UpdateCommand.Parameters.Add("@Source",
                    OleDbType.VarChar, 255, "Source");
                */


                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                adapter.Fill(table);
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return table;
        }
        private void SaveToFile()
        {
            Stream _stream = null;
            try
            {
                _stream = new FileStream(m_FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                SoapFormatter formatter = new SoapFormatter();
                formatter.Serialize(_stream, m_Data);
                m_Data.Modified = false;
            }
            catch (SerializationException e)
            {
                throw (e);
            }
            finally
            {
                if (_stream != null) _stream.Close();
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_FileName == "")
            {
                SaveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                SaveToFile();
            }
        }
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Textdateien (*.txt)|*.txt|Alle Dateien (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                m_FileName = saveFileDialog.FileName;
                SaveToFile();
            };

        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                bindingSource1.DataSource = GetData(this.textBox1.Text);
                dataGridView1.DataSource = bindingSource1;
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message, "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                System.Threading.Thread.CurrentThread.Abort();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            adapter.Update((DataTable)bindingSource1.DataSource);

        }
        public void LoadFile()
        {
            Stream _stream = null;
            try
            {
                OpenFileDialog saveFileDialog = new OpenFileDialog();
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                saveFileDialog.Filter = "Textdateien (*.txt)|*.txt|Alle Dateien (*.*)|*.*";
                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    m_FileName = saveFileDialog.FileName;
                    _stream = new FileStream(m_FileName, FileMode.Open, FileAccess.Read, FileShare.None);
                    SoapFormatter formatter = new SoapFormatter();

                    DBQueryData Data = (DBQueryData)formatter.Deserialize(_stream);
                    SetConnString(Data.ConnString);
                    SetQuery(Data.Query);
                }
            }
            catch (SerializationException e)
            {
                throw (e);
            }

            finally
            {
                _stream.Close();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            SetQuery("??");
        }
        private void connectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_ConnHelper.ShowDialog(this) == DialogResult.OK)
            {
                m_Data.ConnString = m_ConnHelper.GetCfgData(); //copy to local for file save
            };

        }
        private void DBQuery_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_Data.Modified && e.CloseReason == CloseReason.UserClosing)
            {
                if (DialogResult.Cancel == MessageBox.Show(this, "Abfrage modifiziert. Schließen ohne speichern?", "Schließen ohne speichern", MessageBoxButtons.OKCancel))
                {
                    e.Cancel = true;
                }
                else
                {
                    DialogResult = DialogResult.OK;
                }
                return;
            }

            if (!m_Data.Modified)
            {
                DialogResult = DialogResult.OK;
                return;
            }
        }
        private void DBQuery_FormClosed(object sender, FormClosedEventArgs e)
        {
            ;
        }
        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                try
                {
                    // Add the selection to the clipboard.
                    Clipboard.SetDataObject(
                        this.dataGridView1.GetClipboardContent());
                }
                catch (System.Runtime.InteropServices.ExternalException)
                {
                    MessageBox.Show("The Clipboard could not be accessed. Please try again.");
                }
            }

        }

        private void BtViewItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                string _File = "";
                foreach (DataGridViewCell Cell in dataGridView1.CurrentRow.Cells)
                {
                    if (Cell.OwningColumn.DataPropertyName == "FilePath")
                    {
                        _File = Cell.Value.ToString();
                    };
                }
                if (_File != "")
                {
                    if (File.Exists(_File))
                    {
                        FormViewer Viewer = ((MDIParent1)this.MdiParent).GetActiveViewer();
                        if (Viewer == null)
                        {
                            ((MDIParent1)this.MdiParent).CreateNewViewer();
                            Viewer = ((MDIParent1)this.MdiParent).GetActiveViewer();
                        }
                        FileInfo ItemFile = new FileInfo(_File);
                        Viewer.AddFile(ItemFile);
                    }
                }
            }
        }

    }



}
