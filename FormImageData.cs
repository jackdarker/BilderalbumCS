using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;

namespace BilderalbumCS
{
    public partial class FormImageData : Form
    {
        //private OleDbDataAdapter adapter = new OleDbDataAdapter();
        protected DBConnection m_ConnHelper = new DBConnection();
        //private BindingSource bindingSource1 = new BindingSource();
        DAImageData m_DAImageData = null;
        private FileInfo m_File;
        public Rectangle WindowSize
        {
            get
            {
                if (Maximized)
                    return this.RestoreBounds;
                else
                    return this.Bounds;
            }
            set
            {
                this.SetBounds(value.Left, value.Top, value.Width, value.Height);
            }
        }
        public bool Maximized
        {
            get
            {
                return (this.WindowState == FormWindowState.Maximized);
            }
            set
            {
                if (value)
                {
                    this.WindowState = FormWindowState.Maximized;
                }
            }
        }
        public FormImageData()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateDB();
        }
        public void ShowFile(FileInfo File)
        {
            m_File = File;
            RunQuery();
        }
        private void DeleteFromDB()
        {
            try
            {
                m_DAImageData.Delete();
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.ToString() + " in ");
            }
        }
        private void UpdateDB()
        {
           /* try
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
            */
            try
            {
                m_DAImageData.Update();
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.ToString() + " in ");
            }
        }
        protected void RunQuery()
        {
            try
            {
                lblNew.Text = "";
                if (m_DAImageData.SelectByName(m_File))
                {
                    lblNew.Text = "*";
                }

                txtName.DataBindings.Clear();
                txtName.DataBindings.Add("Text", m_DAImageData.GetBindingSource(),
                    "FileName", false, DataSourceUpdateMode.OnPropertyChanged);
                txtDescription.DataBindings.Clear();
                txtDescription.DataBindings.Add("Text", m_DAImageData.GetBindingSource(),
                    "Description", false, DataSourceUpdateMode.OnPropertyChanged);
                numericUpDown1.DataBindings.Clear();
                numericUpDown1.DataBindings.Add("Value", m_DAImageData.GetBindingSource(),
                    "Rating", false, DataSourceUpdateMode.OnPropertyChanged);
                dateTimePicker1.DataBindings.Clear();
                dateTimePicker1.DataBindings.Add("Text", m_DAImageData.GetBindingSource(),
                    "DateAdded", false, DataSourceUpdateMode.OnPropertyChanged);
                
            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message, "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                System.Threading.Thread.CurrentThread.Abort();
            }
        }
        /*private DataTable GetData(string sqlCommand)
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
        }*/
        public void SetConnString(DBConnection Conn)
        {
            //m_ConnHelper.SetCfgData(ConnString);
            m_DAImageData = new DAImageData();
            m_DAImageData.SetConnection(Conn.GetConnection());//m_ConnHelper.GetConnection());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_DAImageData.Delete();
        }

    }
    public class DataAdapterBase
    {
        protected BindingSource m_BindingSource;
        protected OleDbConnection m_Conn;
        protected OleDbDataAdapter m_Adapter;
        protected DataTable m_Table;
        protected OleDbCommandBuilder m_Builder;

        public DataAdapterBase()
        {
            m_Adapter = new OleDbDataAdapter();
            m_Table = new DataTable();
            m_Builder = new OleDbCommandBuilder(m_Adapter);
            m_BindingSource = new BindingSource();

            GetTable().Locale = System.Globalization.CultureInfo.InvariantCulture; //??
            GetBindingSource().DataSource = GetTable();
        }

        public void SetConnection(OleDbConnection Conn)
        {
            m_Conn = null;
            m_Conn = Conn;
        }
        public OleDbConnection GetConnection()
        {
            return m_Conn;
        }
        public OleDbDataAdapter GetAdapter()
        {
            return m_Adapter;
        }
        public BindingSource GetBindingSource()
        {
            return m_BindingSource;
        }
        public DataTable GetTable()
        {
            return m_Table;
        }
        virtual public void PrepareStatements()
        {

        }
        virtual public void SelectByID(int ID)
        {
            //??try
         /*   {
                OleDbCommand SelCommand = new OleDbCommand("select ID,Auftrag,AuftragName,Anlage,AnlagenName,AnlagenTeil from Projekte where ID=@ID",
                    GetConnection());
                SelCommand.Parameters.Add("@ID", OleDbType.Integer);
                GetAdapter().SelectCommand = SelCommand;

                m_Table.Clear();
                m_Adapter.SelectCommand.Parameters["@ID"].Value = ID;
                m_Adapter.Fill(m_Table);
            }*/

        }
        virtual public void CreateNewRecord()
        {        }
        virtual public void Delete()
        {
            m_Adapter.DeleteCommand = m_Builder.GetDeleteCommand();
            m_BindingSource.EndEdit();
            foreach (DataRow Row in m_Table.Rows)
            {
                Row.Delete();
            }
            DataTable TableChanges = m_Table.GetChanges();  
            if (TableChanges != null) m_Adapter.Update(TableChanges);
        }

        //zum holen der Identity nach erstellen neuer DS
        protected int GetIdentity()
        {
            int Identity = 0;
            OleDbCommand SelCommand = new OleDbCommand("select @@identity",
               GetConnection());
            GetAdapter().SelectCommand = SelCommand;
            DataTable TempTable = new DataTable();
            m_Adapter.Fill(TempTable);
            Identity = TempTable.Rows[0].Field<int>(0);
            return Identity;
        }
        public virtual void Update()
        {
            try
            {
                m_Adapter.UpdateCommand = m_Builder.GetUpdateCommand();//??doesnt work with variable field length?
                m_BindingSource.EndEdit();
                DataTable TableChanges = m_Table.GetChanges();  //
                if (TableChanges != null) m_Adapter.Update(TableChanges);
            }
            catch (OleDbException ex)
            {
                throw (ex);
            }
        }
    }
    public class DAImageData : DataAdapterBase
    {
        public DAImageData()
            : base()
        { }
        public bool SelectByName(FileInfo File)
        {
            //??try
            {
                bool IsNewEntry = false;
                OleDbCommand SelCommand = new OleDbCommand(
                    "select IDImage,Rating,FileName,FilePath,DateAdded,Description from Images where FileName=@Name",
                    GetConnection());
                SelCommand.Parameters.Add("@Name", OleDbType.VarWChar);
                GetAdapter().SelectCommand = SelCommand;

                m_Table.Clear();
                m_Adapter.SelectCommand.Parameters["@Name"].Value = File.Name;
                m_Adapter.Fill(m_Table);
                if (m_Table.Rows.Count == 0)
                {
                    CreateNewRecord(File);
                    IsNewEntry = true;
                }
                return IsNewEntry;
            }
        }
        public override void Delete()
        {
            OleDbCommand Command = new OleDbCommand(
                "delete from Images where FileName=@FileName",
                GetConnection());
            Command.Parameters.Add("@FileName", OleDbType.VarWChar, 255, "FileName");   //??
            GetAdapter().DeleteCommand = Command;
            m_BindingSource.EndEdit();
            foreach (DataRow Row in m_Table.Rows)
            {
                Row.Delete();
            }
            DataTable TableChanges = m_Table.GetChanges();
            if (TableChanges != null) m_Adapter.Update(TableChanges);
        }
        public override void Update()
        {
            {//??Commandbuilder doesnt work properly, so building command manually
                OleDbCommand Command = new OleDbCommand(
                    "update Images SET Rating=@Rating, Description=@Description where FileName=@FileName",
                    GetConnection());
                Command.Parameters.Add("@Rating", OleDbType.Integer,4,"Rating");
                Command.Parameters.Add("@Description", OleDbType.VarWChar, 255, "Description");   
                Command.Parameters.Add("@FileName", OleDbType.VarWChar, 255, "FileName");   
                GetAdapter().UpdateCommand = Command;
                m_BindingSource.EndEdit();
                DataTable TableChanges = m_Table.GetChanges();  
                if (TableChanges != null) m_Adapter.Update(TableChanges);
            }
        }
        public int CreateNewRecord(FileInfo File)
        {
            int Identity = -1;
            OleDbCommand SelCommand = new OleDbCommand(
                "select IDImage,Rating,FileName,FilePath,DateAdded,Description from Images where IDImage=-1",
                GetConnection());
            GetAdapter().SelectCommand = SelCommand;
            m_Adapter.InsertCommand = m_Builder.GetInsertCommand();
            m_Table.Clear();
            m_Adapter.Fill(m_Table);  //um Tabellenschema zu holen, es werden 0 Zeilen geliefert
            DataRow NewRow = m_Table.NewRow();
            NewRow.SetField<DateTime>("DateAdded",DateTime.Now );
            NewRow.SetField<int>("Rating", 0);
            NewRow.SetField<string>("FileName", File.Name);
            NewRow.SetField<string>("FilePath", File.FullName);
            m_Table.Rows.Add(NewRow);
            //m_Adapter.Update(m_Table); //
            //Identity = GetIdentity();
            return Identity;
        }
    }



}
