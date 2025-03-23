namespace BilderalbumCS
{
    partial class FormBrowser
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.listView1 = new System.Windows.Forms.ListView();
            this.ctxMenuListView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.ctxMenuTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btOpenInBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btRenameDir = new System.Windows.Forms.ToolStripMenuItem();
            this.btDeleteDir = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.viewMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.prevPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.sortNameAscendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortNameDescendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortDateDescendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortDateAscendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripPages = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ThreadImageList = new System.ComponentModel.BackgroundWorker();
            this.BtNewBrowser = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.BtMoveFile = new System.Windows.Forms.ToolStripDropDownButton();
            this.BtCreateDir = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.ctxMenuListView.SuspendLayout();
            this.ctxMenuTreeView.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listView1);
            this.splitContainer1.Size = new System.Drawing.Size(757, 551);
            this.splitContainer1.SplitterDistance = 252;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(252, 551);
            this.treeView1.TabIndex = 0;
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // listView1
            // 
            this.listView1.AllowDrop = true;
            this.listView1.ContextMenuStrip = this.ctxMenuListView;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.HideSelection = false;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(501, 551);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.ListDragTarget_DragDrop);
            this.listView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.ListDragTarget_DragEnter);
            this.listView1.DragOver += new System.Windows.Forms.DragEventHandler(this.ListDragTarget_DragOver);
            this.listView1.DragLeave += new System.EventHandler(this.ListDragTarget_DragLeave);
            this.listView1.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.ListDragSource_GiveFeedback);
            this.listView1.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.ListDragSource_QueryContinueDrag);
            this.listView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyUp);
            this.listView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListDragSource_MouseDown);
            this.listView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ListDragSource_MouseMove);
            this.listView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ListDragSource_MouseUp);
            // 
            // ctxMenuListView
            // 
            this.ctxMenuListView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripSeparator7,
            this.toolStripMenuItem5});
            this.ctxMenuListView.Name = "ctxMenuTreeView";
            this.ctxMenuListView.Size = new System.Drawing.Size(197, 76);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(196, 22);
            this.toolStripMenuItem3.Text = "rename file";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.btRenameFile_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(196, 22);
            this.toolStripMenuItem4.Text = "delete file";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.BtDeleteFile_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(193, 6);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(196, 22);
            this.toolStripMenuItem5.Text = "Copy path to clipboard";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.btCreateLink_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(150, 150);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ctxMenuTreeView
            // 
            this.ctxMenuTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btOpenInBrowser,
            this.toolStripSeparator3,
            this.btRenameDir,
            this.btDeleteDir,
            this.toolStripSeparator5,
            this.toolStripMenuItem2});
            this.ctxMenuTreeView.Name = "ctxMenuTreeView";
            this.ctxMenuTreeView.Size = new System.Drawing.Size(185, 104);
            // 
            // btOpenInBrowser
            // 
            this.btOpenInBrowser.Name = "btOpenInBrowser";
            this.btOpenInBrowser.Size = new System.Drawing.Size(184, 22);
            this.btOpenInBrowser.Text = "open in new browser";
            this.btOpenInBrowser.Click += new System.EventHandler(this.btOpenInBrowser_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(181, 6);
            // 
            // btRenameDir
            // 
            this.btRenameDir.Enabled = false;
            this.btRenameDir.Name = "btRenameDir";
            this.btRenameDir.Size = new System.Drawing.Size(184, 22);
            this.btRenameDir.Text = "rename dir";
            this.btRenameDir.Click += new System.EventHandler(this.btRenameDir_Click);
            // 
            // btDeleteDir
            // 
            this.btDeleteDir.Enabled = false;
            this.btDeleteDir.Name = "btDeleteDir";
            this.btDeleteDir.Size = new System.Drawing.Size(184, 22);
            this.btDeleteDir.Text = "delete dir";
            this.btDeleteDir.Click += new System.EventHandler(this.btDeleteDir_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(181, 6);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem2.Text = "create thumbnails";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.btCreateThumbs_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(757, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip";
            this.menuStrip.Visible = false;
            // 
            // viewMenu
            // 
            this.viewMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.prevPageToolStripMenuItem,
            this.nextPageToolStripMenuItem,
            this.toolStripSeparator2,
            this.sortNameAscendingToolStripMenuItem,
            this.sortNameDescendingToolStripMenuItem,
            this.sortDateDescendingToolStripMenuItem,
            this.sortDateAscendingToolStripMenuItem});
            this.viewMenu.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.viewMenu.Name = "viewMenu";
            this.viewMenu.Size = new System.Drawing.Size(59, 20);
            this.viewMenu.Text = "&Ansicht";
            // 
            // prevPageToolStripMenuItem
            // 
            this.prevPageToolStripMenuItem.Name = "prevPageToolStripMenuItem";
            this.prevPageToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.prevPageToolStripMenuItem.Text = "Prev Page";
            this.prevPageToolStripMenuItem.Click += new System.EventHandler(this.prevPageToolStripMenuItem_Click);
            // 
            // nextPageToolStripMenuItem
            // 
            this.nextPageToolStripMenuItem.Name = "nextPageToolStripMenuItem";
            this.nextPageToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.nextPageToolStripMenuItem.Text = "Next Page";
            this.nextPageToolStripMenuItem.Click += new System.EventHandler(this.nextPageToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(191, 6);
            // 
            // sortNameAscendingToolStripMenuItem
            // 
            this.sortNameAscendingToolStripMenuItem.Name = "sortNameAscendingToolStripMenuItem";
            this.sortNameAscendingToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.sortNameAscendingToolStripMenuItem.Text = "Sort Name ascending";
            this.sortNameAscendingToolStripMenuItem.Click += new System.EventHandler(this.sortNameAscendingToolStripMenuItem_Click);
            // 
            // sortNameDescendingToolStripMenuItem
            // 
            this.sortNameDescendingToolStripMenuItem.Name = "sortNameDescendingToolStripMenuItem";
            this.sortNameDescendingToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.sortNameDescendingToolStripMenuItem.Text = "Sort Name descending";
            this.sortNameDescendingToolStripMenuItem.Click += new System.EventHandler(this.sortNameAscendingToolStripMenuItem_Click);
            // 
            // sortDateDescendingToolStripMenuItem
            // 
            this.sortDateDescendingToolStripMenuItem.Name = "sortDateDescendingToolStripMenuItem";
            this.sortDateDescendingToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.sortDateDescendingToolStripMenuItem.Text = "Sort Date descending";
            this.sortDateDescendingToolStripMenuItem.Click += new System.EventHandler(this.sortNameAscendingToolStripMenuItem_Click);
            // 
            // sortDateAscendingToolStripMenuItem
            // 
            this.sortDateAscendingToolStripMenuItem.Name = "sortDateAscendingToolStripMenuItem";
            this.sortDateAscendingToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.sortDateAscendingToolStripMenuItem.Text = "Sort Date ascending";
            this.sortDateAscendingToolStripMenuItem.Click += new System.EventHandler(this.sortNameAscendingToolStripMenuItem_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtNewBrowser,
            this.toolStripSeparator4,
            this.toolStripButton1,
            this.toolStripPages,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.toolStripButton3,
            this.BtMoveFile,
            this.BtCreateDir,
            this.toolStripButton4});
            this.toolStrip.Location = new System.Drawing.Point(3, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(345, 25);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "ToolStrip";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripPages
            // 
            this.toolStripPages.Name = "toolStripPages";
            this.toolStripPages.Size = new System.Drawing.Size(121, 25);
            this.toolStripPages.SelectedIndexChanged += new System.EventHandler(this.toolStripPages_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(757, 551);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(757, 598);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip);
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(757, 22);
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "StatusStrip";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Status";
            // 
            // ThreadImageList
            // 
            this.ThreadImageList.WorkerReportsProgress = true;
            this.ThreadImageList.WorkerSupportsCancellation = true;
            this.ThreadImageList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ThreadImageList_DoWork);
            this.ThreadImageList.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.ThreadImageList_ProgressChanged);
            this.ThreadImageList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ThreadImageList_RunWorkerCompleted);
            // 
            // BtNewBrowser
            // 
            this.BtNewBrowser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtNewBrowser.Image = global::BilderalbumCS.Properties.Resources.browser;
            this.BtNewBrowser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtNewBrowser.Name = "BtNewBrowser";
            this.BtNewBrowser.Size = new System.Drawing.Size(23, 22);
            this.BtNewBrowser.Text = "new browser";
            this.BtNewBrowser.Click += new System.EventHandler(this.btOpenInBrowser_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::BilderalbumCS.Properties.Resources.leftpage;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.prevPageToolStripMenuItem_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::BilderalbumCS.Properties.Resources.rightpage;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.nextPageToolStripMenuItem_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::BilderalbumCS.Properties.Resources.remove;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "Delete File (CTRL+D)";
            this.toolStripButton3.Click += new System.EventHandler(this.BtDeleteFile_Click);
            // 
            // BtMoveFile
            // 
            this.BtMoveFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtMoveFile.Image = global::BilderalbumCS.Properties.Resources.MoveFile;
            this.BtMoveFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtMoveFile.Name = "BtMoveFile";
            this.BtMoveFile.Size = new System.Drawing.Size(29, 22);
            this.BtMoveFile.Text = "Move File (Ctrl+M)";
            this.BtMoveFile.DropDownOpening += new System.EventHandler(this.BtMoveFile_DropDownOpening);
            this.BtMoveFile.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.BtMoveFile_DropDownItemClicked);
            // 
            // BtCreateDir
            // 
            this.BtCreateDir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtCreateDir.Image = global::BilderalbumCS.Properties.Resources.CreateDir;
            this.BtCreateDir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtCreateDir.Name = "BtCreateDir";
            this.BtCreateDir.Size = new System.Drawing.Size(23, 22);
            this.BtCreateDir.Text = "Create Dir";
            this.BtCreateDir.Click += new System.EventHandler(this.BtCreateDir_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::BilderalbumCS.Properties.Resources.DeleteDir;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "Delete Dir";
            this.toolStripButton4.Click += new System.EventHandler(this.btDeleteDir_Click);
            // 
            // FormBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 598);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FormBrowser";
            this.Text = "FormBrowser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBrowser_FormClosing);
            this.Load += new System.EventHandler(this.FormBrowser_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ctxMenuListView.ResumeLayout(false);
            this.ctxMenuTreeView.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem viewMenu;
        private System.Windows.Forms.ToolStripMenuItem prevPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nextPageToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripComboBox toolStripPages;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripDropDownButton BtMoveFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.ComponentModel.BackgroundWorker ThreadImageList;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem sortNameAscendingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortNameDescendingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortDateDescendingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortDateAscendingToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton BtCreateDir;
        private System.Windows.Forms.ContextMenuStrip ctxMenuTreeView;
        private System.Windows.Forms.ToolStripMenuItem btRenameDir;
        private System.Windows.Forms.ToolStripMenuItem btDeleteDir;
        private System.Windows.Forms.ToolStripMenuItem btOpenInBrowser;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton BtNewBrowser;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ContextMenuStrip ctxMenuListView;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
    }
}