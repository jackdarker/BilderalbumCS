using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Windows.Forms;

namespace BilderalbumCS
{
    /// <summary>
    /// the main form (MDI style)
    /// </summary>
    public partial class MDIParent1 : Form
    {
        class WndInfo : JKFlow.Core.ISerializable
        {
            public string WndClass = "";
            public Rectangle WndBounds = new Rectangle(10,10,200,200);
            public Boolean Maximized = false;
            public Object UserData = null;
            public void WriteToSerializer(JKFlow.Core.SerializerBase Stream)
            {
                Stream.WriteData("WndClass", WndClass);
                Stream.WriteData("WndBounds", WndBounds);
                Stream.WriteData("Maximized", (Maximized?1:0));
                Stream.WriteData("UserData", UserData.ToString());
            }
            public void ReadFromSerializer(JKFlow.Core.SerializerBase Stream)
            {
                WndClass = Stream.ReadAsString("WndClass");
                WndBounds = Stream.ReadAsRect("WndBounds");
                Maximized = (Stream.ReadAsInt("Maximized")>0);
                UserData = Stream.ReadAsString("UserData");
            }
        }
        private int childFormNumber = 0;
        private DBConnection FormConn = new DBConnection();
        private class FormData : JKFlow.Core.ISerializable
        {
            public Rectangle FormViewerBounds = new Rectangle(100,100,400,400);
            public bool FormViewerMaximized = true;
            public Rectangle MainBounds = new Rectangle(100, 100, 400, 400);
            public bool MainMaximized = true;
            public List<WndInfo> SubWndData = new List<WndInfo>();
            public void WriteToSerializer(JKFlow.Core.SerializerBase Stream)
            {
                Stream.WriteData("FormViewerBounds", FormViewerBounds);
                Stream.WriteData("MainBounds", MainBounds);
                Stream.WriteData("MainMaximized", (MainMaximized ? 1 : 0));
                Stream.WriteData("FormViewerMaximized", (FormViewerMaximized ? 1 : 0));
                List<WndInfo>.Enumerator Iterator= SubWndData.GetEnumerator();
                string Node = "SubWndData";
                while (Iterator.MoveNext())
                {
                    Stream.WriteElementStart(Node);
                    Iterator.Current.WriteToSerializer(Stream);
                    Stream.WriteElementEnd(Node);
                }
            }
            public void ReadFromSerializer(JKFlow.Core.SerializerBase Stream)
            {
                FormViewerBounds = Stream.ReadAsRect("FormViewerBounds");
                MainBounds = Stream.ReadAsRect("MainBounds");
                MainMaximized = (Stream.ReadAsInt("MainMaximized") > 0);
                FormViewerMaximized = (Stream.ReadAsInt("FormViewerMaximized") > 0);
                string NodeGroup;
                int StartNodeLevel = 0, CurrNodeLevel = 0;
                do
                {
                    NodeGroup = Stream.GetNodeName();
                    CurrNodeLevel = Stream.GetNodeLevel();
                    if (CurrNodeLevel < StartNodeLevel) { break; }
                    if (Stream.GetNodeType() != JKFlow.Core.SerializerBase.NodeType.NodeEnd)
                    {
                        if (NodeGroup == "SubWndData")
                        {
                            WndInfo tmp = new WndInfo();
                            tmp.ReadFromSerializer(Stream);
                            SubWndData.Add(tmp);
                        }
                        else if (NodeGroup == JKFlow.Core.SerializerXML.FieldName.SerializerDocName.ToString())
                        {
                            if (NodeGroup != "App")
                                throw new Exception(JKFlow.Core.SerializerXML.FieldName.SerializerDocName.ToString() + " unknown");
                        }
                    }

                } while (Stream.ReadNext());
            }
        }
        private FormData m_AppData= new FormData();

        public MDIParent1()
        {
            
            InitializeComponent();
            menuStrip.MdiWindowListItem = windowsMenu;
        }
        private void ShowNewForm(object sender, EventArgs e)
        {
            CreateNewBrowser();
        }
        private string GetIniFileName()
        {
            string File = Application.StartupPath + Application.ProductName + ".cfg";
            return File;
        }
        /* ?? delete
        private void SaveToFile()
        {
            Stream _stream = null;
            try
            {
                _stream = new FileStream(GetIniFileName(), FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                SoapFormatter formatter = new SoapFormatter();
                DBConnection.DBConnectionData Data = FormConn.GetCfgData();
                formatter.Serialize(_stream, Data);
                formatter.Serialize(_stream, FormViewerBounds);
                formatter.Serialize(_stream, FormViewerMaximized);
                formatter.Serialize(_stream, MainBounds);
                formatter.Serialize(_stream, MainMaximized);
                formatter.Serialize(_stream, SubWndData.GetLength(0));
                for (int i = 0; i<SubWndData.GetLength(0); i++)
                {
                    formatter.Serialize(_stream,SubWndData[i].WndClass);
                    formatter.Serialize(_stream, SubWndData[i].Maximized);
                    formatter.Serialize(_stream, SubWndData[i].WndBounds);
                    formatter.Serialize(_stream, SubWndData[i].UserData);
                }
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
        private void LoadFile()
        {
            Stream _stream = null;
            try
            {
                _stream = new FileStream(GetIniFileName(), FileMode.Open, FileAccess.Read, FileShare.None);
                SoapFormatter formatter = new SoapFormatter();

                DBConnection.DBConnectionData Data = (DBConnection.DBConnectionData)formatter.Deserialize(_stream);
                FormViewerBounds = (Rectangle)formatter.Deserialize(_stream);
                FormViewerBounds.Intersect(Screen.GetWorkingArea(FormViewerBounds));
                FormViewerMaximized = (bool)formatter.Deserialize(_stream);
                MainBounds = (Rectangle)formatter.Deserialize(_stream);
                MainBounds.Intersect(Screen.GetWorkingArea(MainBounds));
                MainMaximized = (bool)formatter.Deserialize(_stream);
                int CntWnds = (int)formatter.Deserialize(_stream);
                SubWndData = new WndInfo[CntWnds];
                for (int i = 0; i < SubWndData.GetLength(0); i++)
                {
                    WndInfo _wndInfo = new WndInfo();
                    _wndInfo.WndClass = (String)formatter.Deserialize(_stream);
                    _wndInfo.Maximized = (Boolean)formatter.Deserialize(_stream);
                    _wndInfo.WndBounds = (Rectangle)formatter.Deserialize(_stream);
                    _wndInfo.UserData = (Object)formatter.Deserialize(_stream);
                    SubWndData[i] = _wndInfo;
                }
                FormConn.SetCfgData(Data);
                FormConn.ConnectToDB();
                RestoreLastSession();
            }
            catch (SerializationException e)
            {
                throw (e);
            }

            finally
            {
                _stream.Close();
            }
        }*/
        private void SaveToFile()
        {
            JKFlow.Core.SerializerXML _stream = null;
            try
            {
                _stream = new JKFlow.Core.SerializerXML("APP", "1.0.0.0");
                _stream.OpenOutputStream(GetIniFileName());
                _stream.WriteElementStart("Application");
                FormConn.GetCfgData().WriteToSerializer(_stream);
                m_AppData.WriteToSerializer(_stream);
                _stream.WriteElementEnd("Application");
                _stream.CloseOutputStream();
                _stream = null;
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                if (_stream != null) _stream.CloseOutputStream();
            }
        }
        private void LoadFile()
        {
            string DocType = string.Empty;
            JKFlow.Core.SerializerXML _stream = null;
            try
            {
                if (!File.Exists(GetIniFileName()))
                {
                    SaveToFile();
                }
                DBConnection.DBConnectionData Data = new DBConnection.DBConnectionData();
                _stream = new JKFlow.Core.SerializerXML("APP", "1.0.0.0");
                _stream.OpenInputStream(GetIniFileName());
                if (_stream.GetDetectedSerializerName() != "APP")
                    throw new FormatException("");
                string NodeGroup;
                do
                {
                    NodeGroup = _stream.GetNodeName();
                    if (_stream.GetNodeType() != JKFlow.Core.SerializerBase.NodeType.NodeEnd)
                    {
                        if (NodeGroup == "Application")
                        {
                            Data.ReadFromSerializer(_stream);
                            m_AppData.ReadFromSerializer(_stream);
                        }
                    }
                } while (_stream.ReadNext());

                _stream.CloseInputStream();
                _stream = null;
                FormConn.SetCfgData(Data);
                FormConn.ConnectToDB();
                RestoreLastSession();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                if (_stream != null) _stream.CloseInputStream();
            }
        }
        private void RestoreLastSession()
        {
            string UserMsg ="";
            WindowSize = m_AppData.MainBounds;
            Maximized = m_AppData.MainMaximized;
            List<WndInfo>.Enumerator Iterator = m_AppData.SubWndData.GetEnumerator();
            while (Iterator.MoveNext())
            {
                if (Iterator.Current.WndClass == "FormBrowser")
                {
                    UserMsg += "\n" + Iterator.Current.WndClass + " ";
                    if (Iterator.Current.UserData is DirectoryInfo ) 
                        UserMsg += ((DirectoryInfo)Iterator.Current.UserData).FullName.ToString();
                    if ( Iterator.Current.UserData is String)
                        UserMsg += (Iterator.Current.UserData).ToString();
                }
                else if (Iterator.Current.WndClass != "")
                {
                    UserMsg += "\n" + Iterator.Current.WndClass;
                }
            }
            if (UserMsg.Length > 0)
            {
                UserMsg = "Restore following Windows?" + UserMsg;
                if (MessageBox.Show(this, UserMsg,
                        "Restore Session?", MessageBoxButtons.YesNo)
                        == DialogResult.Yes)
                {
                    Iterator = m_AppData.SubWndData.GetEnumerator();
                    while (Iterator.MoveNext())
                    {
                        if (Iterator.Current.WndClass == "FormBrowser")
                        {
                            FormBrowser frm = CreateNewBrowser();
                            frm.WindowSize = Iterator.Current.WndBounds;
                            frm.Maximized = Iterator.Current.Maximized;
                            if (Iterator.Current.UserData is DirectoryInfo)
                            {
                                frm.BrowseToDirectory((DirectoryInfo)Iterator.Current.UserData);
                            }
                            else if (Iterator.Current.UserData is String)
                            {
                                frm.BrowseToDirectory(new DirectoryInfo((String)Iterator.Current.UserData));
                            }
                        }
                        if (Iterator.Current.WndClass == "FormImageData")
                        {
                            FormImageData frm = CreateNewImageData();//will Tile windows vertical??
                            frm.WindowSize = Iterator.Current.WndBounds;
                            frm.Maximized = Iterator.Current.Maximized;
                        }
                    }
                }
            }
            else
            {
                CreateNewBrowser();
            }

        }
        public DBQuery CreateNewQuery(string Qry)
        {
            DBQuery childForm = new DBQuery();
            childForm.MdiParent = this;
            childForm.Text = "Fenster " + childFormNumber++;
            childForm.SetConnString(FormConn.GetCfgData());
            childForm.SetQuery(Qry);
            childForm.WindowState = FormWindowState.Maximized;
            childForm.Show();
            //childForm.BringToFront();
            this.ActivateMdiChild(childForm);
            return childForm;
        }
        public FormBrowser CreateNewBrowser()
        {
            FormBrowser childForm = new FormBrowser();
            childForm.MdiParent = this;
            childForm.Text = "Browser " + childFormNumber++;
            childForm.SetConnString(FormConn.GetCfgData());
            childForm.WindowState = FormWindowState.Normal;
            childForm.Show();
            this.ActivateMdiChild(childForm);
            return childForm;
        }
        public FormBrowser GetBrowser(int FormIndex)
        {
            Form[] Childs = this.MdiChildren;
            FormBrowser Browser = null;
            int Index=-1;
            for (int i = 0; i < Childs.GetLength(0); i++)
            {
                if (Childs[i] is FormBrowser)
                {
                    Index++;
                    if (Index == FormIndex) Browser = (FormBrowser)Childs[i];
                };
            }
            return Browser;
        }
        public FormViewer CreateNewViewer()
        {
            FormViewer childForm = new FormViewer();
            childForm.Owner = this;  //no MDI
            childForm.Text = "Viewer " + childFormNumber++;
            childForm.WindowState = FormWindowState.Normal;
            childForm.FormClosing += new FormClosingEventHandler(OwnedFormClosing);
            childForm.Show();
            childForm.WindowSize = m_AppData.FormViewerBounds;
            if (m_AppData.FormViewerMaximized) childForm.WindowState = FormWindowState.Maximized;
            //this.ActivateMdiChild(childForm);
            return childForm;
        }
        public FormViewer GetActiveViewer()
        {
            Form[] Childs = this.OwnedForms;
            FormViewer Viewer = null;

            for (int i = 0; i < Childs.GetLength(0); i++)
            {
                if (Childs[i] is FormViewer)
                {
                    Viewer = (FormViewer)Childs[i];
                };
            }
            return Viewer;
        }
        public FormImageData CreateNewImageData()
        {

            if ( !FormConn.ConnectionOK() ) FormConn.ConnectToDB();

            FormImageData childForm = new FormImageData();
            childForm.MdiParent = this;
            childForm.Text = "ImageData " + childFormNumber++;
            childForm.WindowState = FormWindowState.Normal;
            childForm.SetConnString(FormConn);
            childForm.Show();
            //??LayoutMdi(MdiLayout.TileVertical);
            //this.ActivateMdiChild(childForm);
            return childForm;
        }
        public FormImageData GetActiveImageData()
        {
            Form[] Childs = this.MdiChildren;
            FormImageData Viewer = null;

            for (int i = 0; i < Childs.GetLength(0); i++)
            {
                if (Childs[i] is FormImageData)
                {
                    Viewer = (FormImageData)Childs[i];
                };
            }
            return Viewer;
        }
        private void OpenFile(object sender, EventArgs e)
        {
            CreateNewQuery("");
            if (this.ActiveMdiChild != null) ((DBQuery)this.ActiveMdiChild).LoadFile();
        }
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //if (this.ActiveMdiChild != null) ((DBQuery)this.ActiveMdiChild).SaveAs();

        }
        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }
        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }
        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }
        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }
        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }
        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }
        private void MDIParent1_Load(object sender, EventArgs e)
        {
            //SaveToFile();   //uncomment to create file
            LoadFile();
            //CreateNewBrowser();
        }
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            FormConn.ShowDialog();
        }
        private void MDIParent1_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool AskUser = false;
            m_AppData.MainBounds = WindowSize;
            m_AppData.MainMaximized = Maximized;
            m_AppData.SubWndData = new List<WndInfo>(0);
            int i=0;
            foreach (Form form in MdiChildren)
            {
                WndInfo _WndInfo = new WndInfo();
                if (form is FormBrowser)
                {
     
                    FormBrowser frm= ((FormBrowser)form);
                    _WndInfo.WndBounds = frm.WindowSize;
                    _WndInfo.Maximized = frm.Maximized;
                    _WndInfo.WndClass = "FormBrowser";
                    if (frm.GetCurrentDirectory() != null) {
                        _WndInfo.UserData = frm.GetCurrentDirectory().FullName;
                    } else {
                        _WndInfo.UserData = "";
                    }
                    
                    if (_WndInfo.UserData == null) _WndInfo.UserData = String.Empty;
                };
                if (form is FormImageData)
                {
                    FormImageData frm = ((FormImageData)form);
                    _WndInfo.WndBounds = frm.WindowSize;
                    _WndInfo.Maximized = frm.Maximized;
                    _WndInfo.WndClass = "FormImageData";
                    _WndInfo.UserData = String.Empty;
                };
                m_AppData.SubWndData.Add(_WndInfo);
                i++;
            }

            if (AskUser)
            {
                if (DialogResult.Cancel == MessageBox.Show(this, "Abfrage modifiziert. Schließen ohne speichern?", "Schließen ohne speichern", MessageBoxButtons.OKCancel))
                {
                    e.Cancel = true;
                }
                else
                {
                    
                }
            }
            SaveToFile();
            //    if (this.ActiveMdiChild != null)
            //    e.Cancel = this.ActiveMdiChild.DialogResult != DialogResult.OK;
            //e.Cancel = false;
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormViewer().Show();
        }
        private void OwnedFormClosing(object sender, FormClosingEventArgs e)
        {
            if (sender is FormViewer)
            {
                m_AppData.FormViewerBounds=((FormViewer)sender).WindowSize;
                m_AppData.FormViewerMaximized = ((FormViewer)sender).Maximized;
            };
        }
        /// <summary>
        /// Called to inform that fileresources are to be freed to be able for file operation
        /// </summary>
        /// <param name="File"></param>
        public void FileDeleting(FileInfo File)
        {
            Form[] Childs = this.OwnedForms;

            for (int i = 0; i < Childs.GetLength(0); i++)
            {
                if (Childs[i] is FormViewer)
                {
                    ((FormViewer)Childs[i]).RemoveFile(File);
                };
                /*if (Childs[i] is FormImageData)
                {
                    ((FormImageData)Childs[i]).RemoveFile(File);
                };*/
            }
        }
        /// <summary>
        /// Called to refresh list of files after Fileoperation
        /// </summary>
        /// <param name="File"></param>
        public void RefreshFileList()
        {
            RefreshFileList(null);
        }
        /// <summary>
        /// Called to refresh list of files after Fileoperation
        /// Only Views refering to the Directory are affected (should reduce load if many views are open)
        /// </summary>
        /// <param name="Dir"></param>
        public void RefreshFileList(DirectoryInfo Dir)
        {
            Form[] Childs = this.MdiChildren;
            FormBrowser Child = null;

            for (int i = 0; i < Childs.GetLength(0); i++)
            {
                if (Childs[i] is FormBrowser)
                {
                    Child =(FormBrowser)Childs[i];
                    if (Dir== null || Child.GetCurrentDirectory().FullName== Dir.FullName)
                    {
                        Child.UpdateImageList();
                    };
                };
            }
        }
        /// <summary>
        /// Called to refresh list of folders after directory-delete/add.
        /// </summary>
        /// <param name="Dir"></param>
        public void RefreshFolderList(DirectoryInfo Dir)
        {
            Form[] Childs = this.MdiChildren;
            FormBrowser Child = null;

            for (int i = 0; i < Childs.GetLength(0); i++)
            {
                if (Childs[i] is FormBrowser)
                {
                    Child = (FormBrowser)Childs[i];
                    Child.UpdateFolderTree(Dir);
                };
            }
        }
        /// <summary>
        /// Called to refresh viewer imagelist because files where changed/deleted or folders renamed/deleted.
        /// </summary>
        /// <param name="Dir">either a directory or filename</param>
        public void RefreshImageList(FileSystemInfo Dir)
        {
            Form[] Childs = this.OwnedForms;
            FormViewer Child = null;

            for (int i = 0; i < Childs.GetLength(0); i++)
            {
                if (Childs[i] is FormViewer)
                {
                    Child = (FormViewer)Childs[i];
                    Child.UpdateImageList(Dir);
                };
            }
        }
        private void neuerBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewBrowser();
        }

        private void neuerViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewViewer();
        }

        private void neuerFinderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewQuery("");
        }
        public Rectangle WindowSize
        {
            get
            {
                if (Maximized)
                    return this.RestoreBounds;
                else
                    return this.DesktopBounds;
            }
            set
            {
                this.SetDesktopBounds(value.Left, value.Top, value.Width, value.Height);
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
    }
}
