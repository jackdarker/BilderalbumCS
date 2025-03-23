using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BilderalbumCS
{
    public abstract partial class FormUserInput : Form
    {
        private string m_UserInput = "";
        public string UserInput
        {
            get { return m_UserInput; }
            set
            {
                m_UserInput = value;
                DoValidation();
            }
        }
        private string m_UserOutput = "";
        public string UserOutput
        {
            get { return m_UserOutput; }
            set
            {
                m_UserOutput = value;
            }
        }
        public FormUserInput()
        {
            InitializeComponent();
            button1.Text = "OK";
            button2.Text = "Cancel";
            button3.Text = "";
            button3.Enabled = false;
        }
        protected virtual bool OnValidateInput()
        {
            return true;
        }
        public void EnableButton3(bool Enable, string ButtonTxt)
        {
            button3.Enabled=Enabled;
            if(ButtonTxt!="") button3.Text= ButtonTxt;
        }
        private void DoValidation()
        {
            bool Result = OnValidateInput();
            button1.Enabled = Result;
            txtMessage.Text = UserOutput;
        }
        private void Form_Shown(object sender, EventArgs e)
        {
            txtUserInput.Text = UserInput;
            DoValidation();
        }

        private void txtInput_Leave(object sender, EventArgs e)
        {
            m_UserInput = txtUserInput.Text;
            DoValidation();
        }

        private void txtUserInput_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Return ||
                e.KeyData == Keys.Enter)
            {
                txtInput_Leave(sender, null);
            }
        }

        private void txtUserInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x0D || e.KeyChar == 0x0A) e.Handled = true;
        }
    }
    public class FormFileMove : FormUserInput
    {
        private FileInfo m_SourceFileName = null;
        public FileInfo SourceFileName
        {
            set { m_SourceFileName = value; }
            get { return m_SourceFileName; }
        }
        private string m_DestFileName = null;
        public string DestFileName
        {
            set 
            { 
                m_DestFileName = value;
                UserInput = value;
            }
            get { return m_DestFileName; }
        }
        private string m_DestDirName = null;
        public string DestDirName
        {
            set { m_DestDirName = value; }
            get { return m_DestDirName; }
        }
        public FormFileMove()
            : base()
        {
            EnableButton3(true, "Delete file");
        }
        private bool DestFileExists()
        {
            return File.Exists(DestDirName + "\\" + DestFileName);
        }
        protected override bool OnValidateInput()
        {
            bool Result = false;
            if (UserInput != "")
            {
                m_DestFileName = UserInput;
                UserOutput = string.Format("move file {2:F} to {0:F}?\r\n\r\n{1:F}",
                DestDirName,
                (DestFileExists() ? "File already exists!" : ""),
                DestFileName);
                Result = !DestFileExists();
            }
            else
            {
                UserOutput = string.Format("Enter file name");
                Result = false;
            }
            return Result;
        }
    }
    public class FormDirCreate : FormUserInput
    {
        private DirectoryInfo m_RootDirectory = null;
        public DirectoryInfo RootDirectory
        {
            set { m_RootDirectory = value; }
            get { return m_RootDirectory; }
        }
        public FormDirCreate() : base()
        {
        }
        private bool DestDirExists()
        {
            return Directory.Exists(RootDirectory.FullName + "\\" + UserInput);
        }
        protected override bool OnValidateInput()
        {
            bool Result = false;
            if (UserInput != "")
            {
                Result = !DestDirExists();
                UserOutput = string.Format("create subdirectory {0:F} in {1:F}?\r\n\r\n{2:F}",
                UserInput, RootDirectory.FullName,
                (DestDirExists() ? "Directory already exists!" : ""));
            }
            else
            {
                UserOutput = string.Format("Enter directory name");
            }
            return Result;
        }
    }
    public class FormDirDelete : FormUserInput
    {
        private DirectoryInfo m_RootDirectory = null;
        public DirectoryInfo RootDirectory
        {
            set { m_RootDirectory = value; }
            get { return m_RootDirectory; }
        }
        public FormDirDelete() : base()
        {
        }
        private bool DestDirExists()
        {
            return Directory.Exists(RootDirectory.FullName);
        }
        protected override bool OnValidateInput()
        {
            bool Result = false;
            Result = DestDirExists();
            //TODO  check for subdiretories check access to each file
            UserOutput = string.Format("delete directory\r\n {0:F} ?\r\n", RootDirectory.FullName);
            UserOutput += CheckAffectedFiles();

            return Result;
        }
        private string CheckAffectedFiles()
        {
            //collect all files&dirs within rootdir
            DirectoryInfo _Dir = RootDirectory;
            ArrayList _AllDirs = new ArrayList();
            ArrayList _AllFiles = new ArrayList();
            int _DirIndex = 0;
            int _FileIndex = 0;
            int NumberFiles = 0;
            int NumberDirs = 1;
            _AllDirs.Add(RootDirectory);
            while (_DirIndex < _AllDirs.Count)
            {
                _Dir = (DirectoryInfo)_AllDirs[_DirIndex];
                FileInfo[] _Files=_Dir.GetFiles("*", SearchOption.TopDirectoryOnly);
                NumberFiles += _Files.Length;
                _AllFiles.AddRange(_Files);
                //Note: SearchOption.AllDirectories creates endless loop if directory contains links to parent directory
                DirectoryInfo[] _Dirs = _Dir.GetDirectories("*", SearchOption.TopDirectoryOnly);
                NumberDirs += _Dirs.Length;
                _AllDirs.AddRange(_Dirs);
                _DirIndex++;
            }
            bool bFilesLocked = false;
            /*while (_FileIndex < _AllFiles.Count)
            {
            }*/
            string Msg = "it contains\r\n";
            if(NumberDirs>1) Msg+= string.Format("{0:D} subdirectories and ", NumberDirs-1);
            Msg += string.Format(" {0:D} files !", NumberFiles);
            return (Msg);
        }
        // Return true if the file is locked for the indicated access.
        private bool FileIsLocked(string filename, FileAccess file_access)
        {
            // Try to open the file with the indicated access.
            try
            {
                FileStream fs =
                    new FileStream(filename, FileMode.Open, file_access);
                fs.Close();
                return false;
            }
            catch (IOException)
            {
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
