using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Collections;

namespace BilderalbumCS
{
    /// <summary>
    /// Form for Browsing file directory
    /// </summary>
    public partial class FormBrowser : Form
    {
        private const int THUMBNAILSIZE = 150;
        private const int ITEMSPERPAGE = 40;

        public Rectangle WindowSize {
            get {
                if (Maximized)
                    return this.RestoreBounds;
                else
                    return this.Bounds;
            }
            set {
                this.SetBounds(value.Left,value.Top,value.Width, value.Height);
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
        private enum SortOrder
        {
            NameAsc     = 1,
            NameDesc    = 2,
            DateAsc     = 3,
            DateDesc    = 4
        }
        private SortOrder m_SortOrder = SortOrder.NameAsc;
        private int m_CurrentPage = 0;
        private ListViewItem m_SelectedItem = null;
        private int m_FilesInDirectory = 0;
        private DirectoryInfo m_CurrentDirectory = null;
        private TreeNode m_ContextTreeNode = null;  //stores context menu node
        private int m_PreviousPage = -1;
        private DirectoryInfo m_PreviousDirectory = null;
        private int indexOfItemUnderMouseToDrag;
        private int indexOfItemUnderMouseToDrop;
        private Rectangle dragBoxFromMouseDown;
        private Point screenOffset;

        /// ///////////////////////////////////////////////////////////
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
        /// ///////////////////////////////////////////////////////////
        public FormBrowser()
        {
            InitializeComponent();
            
        }
        public DirectoryInfo GetCurrentDirectory()
        {
            return m_CurrentDirectory;
        }
        //Sets the file dir programatically
        public TreeNode BrowseToDirectory(DirectoryInfo Dir)
        {
            TreeNode SelNode=null;
            treeView1.CollapseAll();
            if (Dir!=null && Directory.Exists(Dir.FullName))
            {
                SelNode = BrowseToDirectoryRecurse(Dir);
                treeView1.SelectedNode = SelNode;
            }
            return (SelNode);
        }
        private TreeNode BrowseToDirectoryRecurse(DirectoryInfo Dir)
        {
            TreeNode ParentNode = null;
            if (Dir.Root.FullName == Dir.FullName)
            {
                ParentNode = treeView1.Nodes[0];
                //AppendChildDirectories(ParentNode);
            }
            else
            {
                ParentNode = BrowseToDirectoryRecurse(Dir.Parent);
                
            }
            //AppendChildDirectories(ParentNode.Nodes[Dir.Name]);
            ParentNode.Expand();
            ParentNode = ParentNode.Nodes[Dir.Name];
            return ParentNode;

        }
        private void FormBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            ThreadImageList.CancelAsync();
            while (ThreadImageList.IsBusy)
            {//wait for Thread abort
                Application.DoEvents();
                Thread.Sleep(20);
            };
        }
        private void FormBrowser_Load(object sender, EventArgs e)
        {
            TreeNode rootNode = new TreeNode("Root");
            rootNode.Tag = null;
            rootNode.Nodes.Add(new TreeNode());
            this.treeView1.Nodes.Add(rootNode);
            sortNameAscendingToolStripMenuItem_Click(null, null); //init sortorder checkmark
            Font DefaultFont= this.treeView1.Font;
            DefaultFont= new Font( DefaultFont.Name, DefaultFont.Size+2f,DefaultFont.Style, DefaultFont.Unit);
            this.treeView1.Font = ConnString.BrowserFont;
        }
        private void AppendChildDirectories(TreeNode DirNode)
        {
            DirNode.Nodes.Clear();
            DirectoryInfo[] childDirectories;
            if (DirNode.Tag != null)
            {
                try
                {
                    DirectoryInfo RootDirectory = (DirectoryInfo)DirNode.Tag;   //?? fails if dir is CD-Rom and not ready
                    childDirectories = RootDirectory.GetDirectories("*", SearchOption.TopDirectoryOnly);
                }
                catch (Exception ex)
                {
                    childDirectories = new DirectoryInfo[0];
                }
            }
            else
            {   //if Node is ROOT, adding Drives
                string[] Drives = Environment.GetLogicalDrives();
                childDirectories = new DirectoryInfo[Drives.Length];
                int i=0;
                for(i=0 ;i < Drives.Length;i++)
                {
                    childDirectories[i]= new DirectoryInfo(Drives[i]);
                }
            }
            foreach (DirectoryInfo childDirectory in childDirectories)
            {
                TreeNode childNode = new TreeNode(childDirectory.Name);
                childNode.Name = childDirectory.Name;
                childNode.Tag = childDirectory;
                childNode.ContextMenuStrip = ctxMenuTreeView;
                //??IOException abfangen if (childDirectory.GetDirectories("*", SearchOption.TopDirectoryOnly).Length > 0)
                {
                    childNode.Nodes.Add(new TreeNode());//create dummy node to enable expand
                };
                DirNode.Nodes.Add(childNode);
            }

        }
        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            AppendChildDirectories(e.Node);
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            m_CurrentPage = 0;
            m_CurrentDirectory = (DirectoryInfo)e.Node.Tag;
            this.Text = (Text.IndexOf('@') >= 0) ? Text.Remove(Text.IndexOf('@')-1): Text;   //show directory in header
            this.Text = Text +" @" + ((m_CurrentDirectory!=null) ?m_CurrentDirectory.FullName : "Root" );
            int _SelectedPage = toolStripPages.SelectedIndex;
            RefreshPageDisplay(m_CurrentPage);
            m_ContextTreeNode = e.Node; //remember node for context menu
           // PopulateImageList(m_CurrentDirectory, m_CurrentPage);
        }
        private void ListDragSource_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            ListViewItem Item = listView1.GetItemAt(e.X, e.Y);
            indexOfItemUnderMouseToDrag = -1;
            if (Item!=null) {
                indexOfItemUnderMouseToDrag = Item.Index;
                // Remember the point where the mouse down occurred. The DragSize indicates
                // the size that the mouse can move before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)), dragSize);
            } else {
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
            }
        }
        private void ListDragSource_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Reset the drag rectangle when the mouse button is raised.
            dragBoxFromMouseDown = Rectangle.Empty;
        }
        private void ListDragSource_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {

                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {

                    // Create custom cursors for the drag-and-drop operation.
                    try
                    {
                        // MyNormalCursor = new Cursor("3dwarro.cur");
                        // MyNoDropCursor = new Cursor("3dwno.cur");

                    }
                    catch
                    {
                        // An error occurred while attempting to load the cursors, so use
                        // standard cursors.
                    }
                    finally
                    {

                        // The screenOffset is used to account for any desktop bands 
                        // that may be at the top or left side of the screen when 
                        // determining when to cancel the drag drop operation.
                        screenOffset = SystemInformation.WorkingArea.Location;

                        // Proceed with the drag-and-drop, passing in the list item.                    
                        DragDropEffects dropEffect = listView1.DoDragDrop(listView1.Items[indexOfItemUnderMouseToDrag], DragDropEffects.All | DragDropEffects.Link);

                        // If the drag operation was a move then remove the item.
                        if (dropEffect == DragDropEffects.Move)
                        {
                            listView1.Items.RemoveAt(indexOfItemUnderMouseToDrag);

                            // Selects the previous item in the list as long as the list has an item.
                            if (indexOfItemUnderMouseToDrag > 0) ;
                            //??   listView1.SelectedIndex = indexOfItemUnderMouseToDrag - 1;

                            else if (listView1.Items.Count > 0) ;
                                // Selects the first item.
                           //??     listView1.SelectedIndex = 0;
                        }

                        // Dispose of the cursors since they are no longer needed.
                      /*  if (MyNormalCursor != null) MyNormalCursor.Dispose();

                        if (MyNoDropCursor != null) MyNoDropCursor.Dispose();*/
                    }
                }
            }
        }
        private void ListDragSource_GiveFeedback(object sender, System.Windows.Forms.GiveFeedbackEventArgs e)
        {
            // Use custom cursors if the check box is checked.
            if (false)
            {

                // Sets the custom cursor based upon the effect.
                e.UseDefaultCursors = false;
                //if ((e.Effect & DragDropEffects.Move) == DragDropEffects.Move)
                    //Cursor.Current = MyNormalCursor;
                //else
                    //Cursor.Current = MyNoDropCursor;
            }

        }
        private void ListDragTarget_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {

            // Determine whether string data exists in the drop data. If not, then
            // the drop effect reflects that the drop cannot occur.
            string[] _formats = e.Data.GetFormats();
        
            //if (!e.Data.GetDataPresent(typeof(System.String)))
            if (!e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            ListViewItem item = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
            if(item.ListView == this.listView1 ){
                //?? Pfad prüfen statt listView
                e.Effect = DragDropEffects.None;
                return;
            }

            // Set the effect based upon the KeyState.
            if ((e.KeyState & (8 + 32)) == (8 + 32) &&
                (e.AllowedEffect & DragDropEffects.Link) == DragDropEffects.Link)
            {
                // KeyState 8 + 32 = CTL + ALT

                // Link drag-and-drop effect.
                e.Effect = DragDropEffects.Link;

            }
            else if ((e.KeyState & 32) == 32 &&
              (e.AllowedEffect & DragDropEffects.Link) == DragDropEffects.Link)
            {

                // ALT KeyState for link.
                e.Effect = DragDropEffects.Link;

            }
            else if ((e.KeyState & 4) == 4 &&
              (e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
            {

                // SHIFT KeyState for move.
                e.Effect = DragDropEffects.Move;

            }
            else if ((e.KeyState & 8) == 8 &&
              (e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {

                // CTL KeyState for copy.
                e.Effect = DragDropEffects.Copy;

            }
            else if ((e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
            {

                // By default, the drop action should be move, if allowed.
                e.Effect = DragDropEffects.Move;

            }
            else
                e.Effect = DragDropEffects.None;

            // Get the index of the item the mouse is below. 

            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point _Pt = PointToClient(new Point(e.X, e.Y));
            ListViewItem _Item = listView1.GetItemAt(_Pt.X,_Pt.Y);
            if (_Item != null) {
                indexOfItemUnderMouseToDrop = _Item.Index;
            } else {
                indexOfItemUnderMouseToDrop = ListBox.NoMatches;
            }
                //??ListDragTarget.IndexFromPoint(ListDragTarget.PointToClient(new Point(e.X, e.Y)));

        }
        private void ListDragTarget_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            // Ensure that the list item index is contained in the data.
            if (e.Data.GetDataPresent(typeof(ListViewItem)))            {
                ListViewItem item = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                if (item.Tag.GetType() == typeof(FileInfo)) {
                    FileInfo File = (FileInfo)item.Tag;
                    // Perform drag-and-drop, depending upon the effect.
                    if (e.Effect == DragDropEffects.Copy ||
                        e.Effect == DragDropEffects.Move) {
                        // Insert the item.
                        //??   if (indexOfItemUnderMouseToDrop != ListBox.NoMatches)
                        //??       ListDragTarget.Items.Insert(indexOfItemUnderMouseToDrop, item);
                        //??else
                        //??ListDragTarget.Items.Add(item);
                            DialogResult Result = MoveOrRenameFile(File, this.GetCurrentDirectory());
                            if (Result == DialogResult.OK || Result == DialogResult.Yes) { } else { e.Effect = DragDropEffects.None; }
                        /*FormFileMove MsgBox = new FormFileMove();
                        MsgBox.DestDirName = this.GetCurrentDirectory().FullName;
                        MsgBox.DestFileName = File.Name;
                        MsgBox.SourceFileName = File;
                        Result = MsgBox.ShowDialog(this);
                        if (Result == DialogResult.OK || Result == DialogResult.Yes) {
                            ((MDIParent1)this.MdiParent).FileDeleting(File);
                            if (Result == DialogResult.OK) {
                                string NewName = this.GetCurrentDirectory().FullName + "\\" + MsgBox.DestFileName;
                                File.MoveTo(NewName);
                            } else {
                                File.Delete();
                            }
                            m_SelectedItem = null;
                            ((MDIParent1)this.MdiParent).RefreshFileList(GetCurrentDirectory());
                            ((MDIParent1)this.MdiParent).RefreshFileList(File.Directory);
                        } else {
                            e.Effect = DragDropEffects.None;
                        }*/
                    }
                }
            }

        }
        private void ListDragSource_QueryContinueDrag(object sender, System.Windows.Forms.QueryContinueDragEventArgs e)
        {
            // Cancel the drag if the mouse moves off the form.
            ListBox lb = sender as ListBox;

            if (lb != null)
            {

                Form f = lb.FindForm();

                // Cancel the drag if the mouse moves off the form. The screenOffset
                // takes into account any desktop bands that may be at the top or left
                // side of the screen.
                if (((Control.MousePosition.X - screenOffset.X) < f.DesktopBounds.Left) ||
                    ((Control.MousePosition.X - screenOffset.X) > f.DesktopBounds.Right) ||
                    ((Control.MousePosition.Y - screenOffset.Y) < f.DesktopBounds.Top) ||
                    ((Control.MousePosition.Y - screenOffset.Y) > f.DesktopBounds.Bottom))
                {

                    //??   e.Action = DragAction.Cancel;
                }
            }
        }
        private void ListDragTarget_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {        }
        private void ListDragTarget_DragLeave(object sender, System.EventArgs e)
        {        }
        private int fileInfoComparer(FileInfo fi1, FileInfo fi2)
        {
            int Result = -1;
            switch (m_SortOrder)
            {
                case SortOrder.NameDesc:
                    Result = fi2.Name.CompareTo(fi1.Name) ;
                    break;
                case SortOrder.DateAsc:
                    if (fi1.LastWriteTime > fi2.LastWriteTime) Result= 1;
                    else if (fi1.LastWriteTime == fi2.LastWriteTime) Result= 0;
                    break;
                case SortOrder.DateDesc:
                    if (fi1.LastWriteTime < fi2.LastWriteTime) Result = 1;
                    else if (fi1.LastWriteTime == fi2.LastWriteTime) Result = 0;
                    break;
                default:
                    Result = fi1.Name.CompareTo(fi2.Name) ;
                    break;
            }
            return Result;
        }
        private FileInfo[] GetImagesInDirectory(DirectoryInfo Dir)
        {
            FileInfo[] AllFiles= new FileInfo[0];
            FileInfo[] Files = new FileInfo[0];
            if (Dir != null)
            {
                try
                {
                    AllFiles = Dir.GetFiles("*", SearchOption.TopDirectoryOnly);
                }
                catch (Exception ex)
                {
                    ;   //IO or AccessException
                }
                Comparison<FileInfo> comp = new Comparison<FileInfo>(fileInfoComparer);
                Array.Sort(AllFiles, comp);
                int[] FileOk = new int[AllFiles.Length];
                int NumberFiles = 0;
                string Extension;
                for (int i = 0; i < AllFiles.Length; i++)
                {
                    FileOk[NumberFiles] = -1;
                    Extension = AllFiles[i].Extension.ToLower();
                    if (Extension == ".jpg" ||
                        Extension == ".jpeg" ||
                        Extension == ".bmp" ||
                        Extension == ".gif" ||
                        Extension == ".tga" ||
                        Extension == ".png")
                    {
                        FileOk[NumberFiles] = i;
                        NumberFiles++;
                    }
                }
                 Files = new FileInfo[NumberFiles];
                for (int i = 0; i < NumberFiles; i++)
                {
                    Files[i] = AllFiles[FileOk[i]];
                }
            }
            return Files;
        }    
        private void PopulateImageList(DirectoryInfo Dir,int Page)
        {
            ThreadImageList.CancelAsync();
            while (ThreadImageList.IsBusy)
            {//wait for Thread abort
                Application.DoEvents();
                Thread.Sleep(20);
            };//be sure thread is stopped or there might be access conflicts in imageList
            this.toolStripStatusLabel.Text = "Creating Thumbnails";
            listView1.Items.Clear();
            if (m_PreviousDirectory != Dir || m_PreviousPage != Page)
            {//dont delete & recreate images if directory not changed
                imageList1.Images.Clear();
            }
            m_PreviousDirectory = Dir;
            m_PreviousPage = Page;

            if (Dir != null)
            {
                FileInfo[] Files = GetImagesInDirectory(Dir);
                for (int i = Page * ITEMSPERPAGE; (i < Files.Length && i < Page * ITEMSPERPAGE + ITEMSPERPAGE); i++)
                {
                    ListViewItem FileItem = new ListViewItem(Files[i].Name);
                    FileItem.Tag = Files[i];
                    FileItem.Name = Files[i].Name;
                    listView1.Items.Add(FileItem);
                }

                //now creating images
                if (ThreadImageList.IsBusy != true)
                {
                    ThreadImageListArgument Arg = new ThreadImageListArgument();
                    Arg.Directory = Dir;
                    Arg.Page = Page;
                    ThreadImageList.RunWorkerAsync(Arg);
                }
                if (m_SelectedItem != null && listView1.Items.ContainsKey(m_SelectedItem.Text))
                {
                    listView1.Items[m_SelectedItem.Text].EnsureVisible(); //TopItem doesnt work in IconView
                }
                /*
                FileInfo[] Files = GetImagesInDirectory(Dir);
                for (int i = Page * ITEMSPERPAGE; (i < Files.Length && i < Page * ITEMSPERPAGE + ITEMSPERPAGE); i++)
                {
                    ListViewItem FileItem = CreateImageItem(Files[i]);
                    if (FileItem != null)
                    {
                        listView1.Items.Add(FileItem);
                    };
                }*/

            }
        }
        /// <summary>
        /// -2 to not change page
        /// will trigger preview creation
        /// </summary>
        /// <param name="ActiveIndex"></param>
        private void RefreshPageDisplay(int ActiveIndex)
        {
            m_FilesInDirectory = GetImagesInDirectory(m_CurrentDirectory).Length; //
            int _SelectedPage = toolStripPages.SelectedIndex;
            int _CurrPageCount = toolStripPages.Items.Count;
            int _NewPageCount = (m_FilesInDirectory / ITEMSPERPAGE)+
                (((m_FilesInDirectory % ITEMSPERPAGE)==0) ? 0 : 1);
            _NewPageCount = Math.Max(1,_NewPageCount);
         
            int _Pages = Math.Max(_CurrPageCount,_NewPageCount);
            bool _ManualUpdate = true;
            for (int i = 0; i < _Pages; i++)
            {
                string _PageText = string.Format("{0:D} of {1:D} ({2:D}Files)", (i + 1), _NewPageCount, m_FilesInDirectory);
                if (i >= _NewPageCount) //remove unused pages
                {
                    toolStripPages.Items.RemoveAt(toolStripPages.Items.Count-1);
                    _ManualUpdate = false;
                }
                else if (i >= _CurrPageCount)  //add Page
                {
                    toolStripPages.Items.Add(_PageText);
                    _ManualUpdate = false;
                }
                else if (toolStripPages.Items[i].ToString() != _PageText) //rename page
                {
                    toolStripPages.Items[i] = _PageText;    //will trigger SelectedIndexChanged-event??
                    _ManualUpdate = false;
                }
            }
            //if Directory changes, select page1, else select active or previous page
            //will trigger preview creation
            if (ActiveIndex < 0)
            {
                toolStripPages.SelectedIndex = Math.Min(_SelectedPage, toolStripPages.Items.Count - 1);
            }
            else
            {
                toolStripPages.SelectedIndex = Math.Min(ActiveIndex, toolStripPages.Items.Count - 1);
            }
            if (_ManualUpdate ) toolStripPages_SelectedIndexChanged(null, null);    //if neither index nor text changed, trigger manually
        }
        public void UpdateImageList()
        {
            RefreshPageDisplay(-2);
            //PopulateImageList(m_CurrentDirectory, m_CurrentPage);//??modify Page if file removed added
        }
        /// <summary>
        /// update Directorytree if directory created/deleted/renamed
        /// </summary>
        /// <param name="Dir"></param>
        public void UpdateFolderTree(DirectoryInfo Dir)
        {
            //if dir or parent in tree  c\path\to\dir   treeView1.Nodes["path"].Nodes["to"]
            DirectoryInfo _Dir;
            ArrayList _path =new ArrayList();
            _path.Add(Dir.Name);
            _Dir = Dir.Parent;
            while(_Dir != null)
            {
                _path.Add(_Dir.Name);
                _Dir = _Dir.Parent;
            }
            _path.Reverse();
            TreeNodeCollection _Nodes = treeView1.Nodes[0].Nodes;  //FirstNode = "Root"
            TreeNode _Nodes2;
            TreeNodeCollection _Nodes3;
            int i,n;
            bool _unfolded = false;
            n = _path.Count - 1; i = -1;
            while(i<n)
            {
                i++;
                _Nodes2 = _Nodes[(string)_path[i]];
                if (_Nodes2 != null)
                {
                    if (i == n) //Dir is shown
                    {   //if parent was unfolded, fold-unfold to refresh
                        BrowseToDirectory(Dir.Parent).Expand();  // updates tree and imagelist and expands updated parent
                    } else _Nodes = _Nodes2.Nodes;
                }
                else break;
            }
        }
        public bool ThumbnailCallback()
        {//callback stub for Thumbnailcreation
            return false;
        }
        /* ??
         * private ListViewItem CreateImageItem(FileInfo File)
        {
            bool IsImage = false;

            ListViewItem ImageItem = new ListViewItem(File.Name);
            ImageItem.Tag = File;
            string Extension =File.Extension.ToLower();
                IsImage = true;
                Image NormalImage= Image.FromFile(File.FullName);
                ImageItem.ImageKey=File.Name;
                imageList1.Images.Add(File.Name, GetScaledImage(NormalImage, THUMBNAILSIZE));
            if (!IsImage)
            {
                ImageItem = null;
            }
            return ImageItem;
        }*/
        private Image GetScaledImage(Image LargeImage, int Size)
        {
            Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
            int ThumbnailSize = Size;
            int Width =0;
            int Height = 0;
            float Ratio = LargeImage.Width / LargeImage.Height;
            if(Ratio>= 1f)
            {
                Width = ThumbnailSize;
                Height = (Width * LargeImage.Height) / LargeImage.Width;
            }
            else
            {
                Height = ThumbnailSize;
                Width = (Height * LargeImage.Width) / LargeImage.Height;
            }
            Bitmap IconBMP = new Bitmap(ThumbnailSize, ThumbnailSize);
            Brush TranspBrush = Brushes.Transparent;//??.White;
            Image IconImage = LargeImage.GetThumbnailImage(Width, Height, myCallback, System.IntPtr.Zero);
            Graphics MyGraphic = Graphics.FromImage(IconBMP);
            MyGraphic.FillRectangle(TranspBrush, new Rectangle(0,0,100,100));
            MyGraphic.DrawImage(LargeImage, 
                new Rectangle((ThumbnailSize - Width) / 2, (ThumbnailSize - Height) / 2, Width, Height));
            Image MyImage = IconBMP;
            return MyImage;
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

           FormViewer Viewer = ((MDIParent1)this.MdiParent).GetActiveViewer();
           if (Viewer == null)
           {
                ((MDIParent1)this.MdiParent).CreateNewViewer();
                Viewer = ((MDIParent1)this.MdiParent).GetActiveViewer();
           }

           FormImageData ImageData = ((MDIParent1)this.MdiParent).GetActiveImageData();
           if (ImageData == null)
           {
               ((MDIParent1)this.MdiParent).CreateNewImageData();
               ImageData = ((MDIParent1)this.MdiParent).GetActiveImageData();
           }

           if (listView1.SelectedItems.Count > 0)
           {
               m_SelectedItem = (ListViewItem)listView1.SelectedItems[0].Clone();//why is Namevalue not cloned?
               Viewer.AddFile((FileInfo)listView1.SelectedItems[0].Tag);
               ImageData.ShowFile((FileInfo)listView1.SelectedItems[0].Tag);
           }
           else 
           {
               m_SelectedItem = null;
           }
           
        }
        private void prevPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((m_CurrentPage>0)) m_CurrentPage--;
            RefreshPageDisplay(m_CurrentPage);
            //this.toolStripPages.SelectedIndex = m_CurrentPage;
            //PopulateImageList(m_CurrentDirectory, m_CurrentPage);
        }
        private void nextPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((m_CurrentPage + 1) * ITEMSPERPAGE < m_FilesInDirectory) m_CurrentPage++;
            RefreshPageDisplay(m_CurrentPage);
            //this.toolStripPages.SelectedIndex = m_CurrentPage;
            //PopulateImageList(m_CurrentDirectory, m_CurrentPage);
        }
        private void toolStripPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_CurrentPage = Math.Max(0, this.toolStripPages.SelectedIndex);
            PopulateImageList(m_CurrentDirectory, m_CurrentPage);
        }
        /// <summary>
        /// delete file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtDeleteFile_Click(object sender, EventArgs e)
        {
            FileInfo SelectedFile = null;
            if (this.listView1.SelectedItems.Count>0 )
            {
                SelectedFile = (FileInfo)listView1.SelectedItems[0].Tag;
                if (MessageBox.Show(this,
                    string.Format("delete file {0:F} ?", SelectedFile.Name),
                    "Are you sure?", MessageBoxButtons.YesNo)
                    == DialogResult.Yes)
                {
                    ((MDIParent1)this.MdiParent).FileDeleting(SelectedFile);
                    m_SelectedItem = null;
                    for (int i = 0; i<listView1.Items.Count; i++)
                    {//avoid jump to start of list after rebuild of list
                        if ((i-1)>=0 && listView1.Items[i].Name == listView1.SelectedItems[0].Text)
                        {
                                m_SelectedItem = (ListViewItem)listView1.Items[i-1].Clone();
                        }
                    }
                    SelectedFile.Delete();             
                   ((MDIParent1)this.MdiParent).RefreshFileList(GetCurrentDirectory());
                }
            }
        }
        private void BtMoveFile_DropDownOpening(object sender, EventArgs e)
        {
            this.BtMoveFile.DropDownItems.Clear();
            Form[] Childs = this.MdiParent.MdiChildren;

            for (int i = 0; i < Childs.GetLength(0); i++)
            {
                if (Childs[i] is FormBrowser && !Childs[i].Equals(this) &&
                    ((FormBrowser)Childs[i]).GetCurrentDirectory()!=null)
                {
                    string DirName = ((FormBrowser)Childs[i]).GetCurrentDirectory().FullName;
                    ToolStripMenuItem DirItem = new ToolStripMenuItem(DirName);
                    this.BtMoveFile.DropDownItems.Add(DirItem);
                };
            }
        }
        /// <summary>
        /// move file to directory, selected in other browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtMoveFile_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            FileInfo SelectedFile = null;
            string Dir =e.ClickedItem.Text;
            DirectoryInfo _Dir = new DirectoryInfo(Dir);
            if (this.listView1.SelectedItems.Count > 0)
            {
                SelectedFile = (FileInfo)listView1.SelectedItems[0].Tag;
                /*bool Exists = File.Exists(Dir + "\\" + SelectedFile.Name);
                if (MessageBox.Show(this,
                    string.Format("move file {2:F} to {0:F}?\n{1:F}",
                    Dir, 
                    (Exists ?"File already exists!":""),
                    SelectedFile.Name),
                        "Are you sure?", MessageBoxButtons.YesNo)
                    == DialogResult.Yes)*/

                MoveOrRenameFile(SelectedFile, _Dir);
             }

        }

        private DialogResult MoveOrRenameFile(FileInfo SelectedFile, DirectoryInfo Dir) { //Todo auch den Pfad in der DB aktualisieren
            FormFileMove MsgBox = new FormFileMove();
            MsgBox.DestFileName = SelectedFile.Name;
            MsgBox.DestDirName = Dir.FullName;
            MsgBox.SourceFileName = SelectedFile;
            DialogResult Result = MsgBox.ShowDialog(this);
            if (Result == DialogResult.OK ||
                 Result == DialogResult.Yes) {
                ((MDIParent1)this.MdiParent).FileDeleting(SelectedFile);
                if (Result == DialogResult.OK) {
                    SelectedFile.MoveTo(MsgBox.DestDirName + "\\" + MsgBox.DestFileName);
                } else {
                    SelectedFile.Delete();
                }
                m_SelectedItem = null;
                if (listView1.SelectedItems.Count > 0) { //Todo das funktioniert nur wenn auch ein Item ausgewählt ist
                    for (int i = 0; i < listView1.Items.Count; i++) {//avoid jump to start of list after rebuild of list
                        if ((i - 1) >= 0 && listView1.Items[i].Name == listView1.SelectedItems[0].Text) {
                            m_SelectedItem = (ListViewItem)listView1.Items[i - 1].Clone();
                        }
                    }
                }
                ((MDIParent1)this.MdiParent).RefreshFileList(GetCurrentDirectory());
                ((MDIParent1)this.MdiParent).RefreshFileList(SelectedFile.Directory);
            };
            return (Result);
        }
        public void SetConnString(DBConnection.DBConnectionData ConnString)
        {
            m_Conn = ConnString;
        }
        private void sortNameAscendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool sortNameAscendingToolItemChecked = false;
            bool sortNameDescendingToolItemChecked = false;
            bool sortDateAscendingToolItemChecked = false;
            bool sortDateDescendingToolItemChecked = false;
            if (sender == null)
            {
                sortNameAscendingToolItemChecked = true;
                m_SortOrder = SortOrder.NameAsc;
            }
            else if (sender.Equals(sortNameAscendingToolStripMenuItem))
            {
                sortNameAscendingToolItemChecked = true;
                m_SortOrder = SortOrder.NameAsc;
            }
            else if (sender.Equals(sortNameDescendingToolStripMenuItem))
            {
                sortNameDescendingToolItemChecked = true;
                m_SortOrder = SortOrder.NameDesc;
            }
            else if (sender.Equals(sortDateAscendingToolStripMenuItem))
            {
                sortDateAscendingToolItemChecked = true;
                m_SortOrder = SortOrder.DateAsc;
            }
            else if (sender.Equals(sortDateDescendingToolStripMenuItem))
            {
                sortDateDescendingToolItemChecked = true;
                m_SortOrder = SortOrder.DateDesc;
            }
            else
            {
                sortNameAscendingToolItemChecked = true;
                m_SortOrder = SortOrder.NameAsc;
            }
            sortNameAscendingToolStripMenuItem.Checked = sortNameAscendingToolItemChecked;
            sortNameDescendingToolStripMenuItem.Checked = sortNameDescendingToolItemChecked;
            sortDateAscendingToolStripMenuItem.Checked = sortDateAscendingToolItemChecked;
            sortDateDescendingToolStripMenuItem.Checked = sortDateDescendingToolItemChecked;
            PopulateImageList(m_CurrentDirectory, m_CurrentPage);
        }
        private void BtCreateDir_Click(object sender, EventArgs e)
        {
            if (m_CurrentDirectory != null && m_CurrentDirectory.Exists)
            {
                FormDirCreate MsgBox = new FormDirCreate();
                MsgBox.RootDirectory = m_CurrentDirectory;
                if (MsgBox.ShowDialog(this) == DialogResult.OK)
                {
                    string NewDirName = MsgBox.RootDirectory.FullName+ "\\" + MsgBox.UserInput;
                    DirectoryInfo NewDir =  Directory.CreateDirectory(NewDirName);
                    AppendChildDirectories(this.treeView1.SelectedNode);
                    BrowseToDirectory(NewDir);
                }
            }
        }
        private void btOpenInBrowser_Click(object sender, EventArgs e)
        {
            //??
            if (m_ContextTreeNode!=null)
            {
                DirectoryInfo _Dir = ((DirectoryInfo)m_ContextTreeNode.Tag);
                (((MDIParent1)this.MdiParent).CreateNewBrowser()).BrowseToDirectory(_Dir);
            }
        }
        private void btCreateThumbs_Click(object sender, EventArgs e) {
            if (m_ContextTreeNode != null) {
                DirectoryInfo _Dir = ((DirectoryInfo)m_ContextTreeNode.Tag);
                FileInfo[] Files = GetImagesInDirectory(_Dir);
                DirectoryInfo NewDir = Directory.CreateDirectory(_Dir.FullName + "\\Thumbs");
                Image ScaledImage;
                FileStream ScaledFile;
                Image _Image;
                for (int i = 0; (i < Files.Length); i++) {
                    ScaledFile = File.Create(NewDir.FullName + "\\" + Files[i].Name);
                    _Image = Image.FromFile(Files[i].FullName);
                    ScaledImage = GetScaledImage(_Image, THUMBNAILSIZE);
                    ScaledImage.Save(ScaledFile, System.Drawing.Imaging.ImageFormat.Png);
                    _Image.Dispose();
                    ScaledImage.Dispose();
                    ScaledFile.Dispose();
                    _Image = null;
                    ScaledFile = null;
                    ScaledImage = null;
                }
            }
        }
        
        private void btRenameDir_Click(object sender, EventArgs e)//TODO
        {//all open images in this dir have to be closed !

        }
        private void btDeleteDir_Click(object sender, EventArgs e)
        {//dialog- "all open images in this dir will be closed" "This directory contains subdirectorys that will be deleted too" "Are you sure to delete fullpath"
            bool bOK = true;
            if (m_CurrentDirectory == null || !m_CurrentDirectory.Exists)
            {
                return;
            }
            //confirm dialog
            FormDirDelete MsgBox = new FormDirDelete();
            MsgBox.RootDirectory = m_CurrentDirectory;
            if (MsgBox.ShowDialog(this) != DialogResult.OK) return;

            //Hack: Viewer has to release image-file BEFORE able to delete  TODO Notify viewers that directory will be deleted and they should remove images from list
            ((MDIParent1)this.MdiParent).RefreshImageList(m_CurrentDirectory);
            //delete content and dir
            Directory.Delete(m_CurrentDirectory.FullName, true);
            //refresh all browsers & viewers
            ((MDIParent1)this.MdiParent).RefreshFolderList(m_CurrentDirectory);
            
        }
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {//workaround to store clicked node for context menu action
            if (e.Button == MouseButtons.Right) //doesnt work for keyboard, using selected node in that case
            {
                m_ContextTreeNode = e.Node;
            };
        }
        #region ThreadImageList
        private void ThreadImageList_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.toolStripProgressBar1.Value =e.ProgressPercentage;
            try
            {
                ThreadImageListArgument Arg = (ThreadImageListArgument)e.UserState;
                if (imageList1.Images.IndexOfKey(Arg.Item) < 0)
                {//not already contained in list
                    imageList1.Images.Add(Arg.Item, Arg.ScaledImage);
                };
                ListViewItem Item = listView1.FindItemWithText(Arg.Item);
                if (Item != null) {
                    Item.ImageKey = Item.Text;
                    this.listView1.Invalidate(Item.Bounds);
                }
                this.Update();
                
            }
            catch(Exception ex)
            {
                ;
            }
        }
        private void ThreadImageList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            this.toolStripProgressBar1.Value = 100;
            this.toolStripStatusLabel.Text = e.Cancelled ? "Canceled" : "Completed";
        }
        private class ThreadImageListArgument
        {
            public ThreadImageListArgument()
            { }
            //input to Thread
            public DirectoryInfo Directory = null;
            public int Page = 0;
            //output from thread
            public Image ScaledImage = null;
            public string Item = "";
        }
        private void ThreadImageList_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            ThreadImageListArgument Arg = e.Argument as ThreadImageListArgument;
            int Page = Arg.Page;
            DirectoryInfo Dir = Arg.Directory;
            FileInfo[] Files = GetImagesInDirectory(Dir);
            int StartOffset = Page * ITEMSPERPAGE;
            int _AlreadyThumbedIndex = -1;
            System.Collections.Specialized.StringCollection _Keys; 
            for (int i = StartOffset; (i < Files.Length && i < Page * ITEMSPERPAGE + ITEMSPERPAGE); i++)
            {
                System.Threading.Thread.Sleep(1);
                Application.DoEvents();
                if ((worker.CancellationPending == true)) {
                    e.Cancel = true;
                    break;
                } else  {  

			//?? in Thumbsdatei prüfen ob Thumbnail vorhanden sonst neu erstellen und speichern
			// datei löschen bei Seiten/Verzeichniswechsel oder Browserende

                    try
                    {
                        _Keys = imageList1.Images.Keys;
                        _AlreadyThumbedIndex =  _Keys.IndexOf(Files[i].Name);
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {//wieso gibts hier manchmal eine exception
                        _AlreadyThumbedIndex = -1;
                    }
                    Image ScaledImage ;
                    if (_AlreadyThumbedIndex >= 0) {
                        ScaledImage = imageList1.Images[_AlreadyThumbedIndex];
                    } else {
                        Image _Image = Image.FromFile(Files[i].FullName);
                        ScaledImage = GetScaledImage(_Image, THUMBNAILSIZE);
                        _Image.Dispose();
                        _Image = null;
                    }
                    ThreadImageListArgument Out = new ThreadImageListArgument();
                    Out.ScaledImage = ScaledImage;
                    Out.Item = Files[i].Name;
                    worker.ReportProgress((((i - StartOffset) * 100) / ITEMSPERPAGE), Out);
                }


            }
        }
        #endregion

        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.D:
                        BtDeleteFile_Click(this, null);
                        break;
                    case Keys.M:
                        BtMoveFile.ShowDropDown();
                        break;
                    default:
                        break;
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }
        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            // Handle FileDrop data.
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Assign the file names to a string array, in 
                // case the user has selected multiple files.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                try
                {
                    // Assign the first image to the picture variable.
                /*    this.picture = Image.FromFile(files[0]);
                    // Set the picture location equal to the drop point.
                    this.pictureLocation = this.PointToClient(new Point(e.X, e.Y));*/
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }

            // Handle Bitmap data.
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                try
                {
                    // Create an Image and assign it to the picture variable.
               /*    this.picture = (Image)e.Data.GetData(DataFormats.Bitmap);
                    // Set the picture location equal to the drop point.
                    this.pictureLocation = this.PointToClient(new Point(e.X, e.Y));*/
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }
        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            // If the data is a file or a bitmap, display the copy cursor.
            if (e.Data.GetDataPresent(DataFormats.Bitmap) ||
               e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void btCreateLink_Click(object sender, EventArgs e) {
            FileInfo SelectedFile = null;
            if (this.listView1.SelectedItems.Count > 0) {
                SelectedFile = (FileInfo)listView1.SelectedItems[0].Tag;
                Clipboard.SetDataObject(SelectedFile.FullName);

            }
        }

        private void btRenameFile_Click(object sender, EventArgs e) {
            MoveOrRenameFile((FileInfo)listView1.SelectedItems[0].Tag, this.GetCurrentDirectory());
        }

    }
   
}
