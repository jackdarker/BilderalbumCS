using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;

namespace BilderalbumCS
{
    public partial class FormQuery : Form
    {
        [Serializable]
        public class FormData //: ISerializable
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
                }
            }
            public FormData() { }

            [NonSerialized]
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
                }
            }
        }
        private BindingSource bindingSource1 = new BindingSource();
        private OleDbDataAdapter adapter = new OleDbDataAdapter();
        protected DBConnection m_ConnHelper = new DBConnection();
        FormData m_Data = new FormData();
        public FormQuery()
        {
            InitializeComponent();
            bindingNavigator1.BindingSource = bindingSource1;
            this.textBox1.DataBindings.Add("Text", m_Data, "Query");  //??funktioniert nicht in beide Richtungen
            SetQuery("");
        }
        protected void SetQuery(string Qry)
        {
            m_Data.Query = Qry;
            this.textBox1.Text = m_Data.Query;
        }
        protected void RunQuery()
        {
            try
            {
                bindingSource1.DataSource = GetData(m_Data.Query);
                dataGridView1.DataSource = bindingSource1;
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message, "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                System.Threading.Thread.CurrentThread.Abort();
            }
        }
        private DataTable GetData(string sqlCommand)
        {
            DataTable table = new DataTable();
            try
            {
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
                adapter.SelectCommand = command;
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                adapter.Fill(table);
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return table;
        }
        public void SetConnString(DBConnection.DBConnectionData ConnString)
        {
            m_Data.ConnString = ConnString;
            m_ConnHelper.SetCfgData(m_Data.ConnString);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
