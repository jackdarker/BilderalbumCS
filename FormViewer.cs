using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace BilderalbumCS
{
    public partial class FormViewer : Form
    {
        private class ImageItem : ListViewItem
        {
            public ImageItem(string text)
                : base(text)
            {
                SetZoom(100);
                SetScrollPosition(new Point(0, 0));
            }
            public FileInfo ItemFile;
            public void SetZoom(int Zoom)
            {
                m_Zoom= Zoom;
            }
            public int GetZoom()
            {
                return m_Zoom;
            }
            private int m_Zoom;
            public void SetScrollPosition(Point Pos)
            {
                m_ScrollPosition = Pos;
            }
            public Point GetScrollPosition()
            {
                return m_ScrollPosition;
            }
            private Point m_ScrollPosition;
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
                return (this.WindowState== FormWindowState.Maximized);
            }
            set
            {
                if (value)
                {
                    this.WindowState = FormWindowState.Maximized;
                }
            }
        }
        private ImageItem m_ItemIndex = null;
        private int m_DefaultImageScalingMode = -1;
        private int m_ImageScalingMode = -1;
            //0 = 
            //-1 = Scale to Window
            //-2 = Shrink to Window but dont upscale
            //>0 = zoom in %


        public FormViewer()
        {
            InitializeComponent();
            m_ImageScalingMode = 100;
            timer1.Enabled = false;
            timer1.Tick += new  EventHandler(OnTimedEvent);

        }
        private void Form1_Load(object sender, EventArgs e)
        {        }
        public void AddFile(FileInfo File)
        {
            ListViewItem Item = listView1.FindItemWithText(File.Name);
            if (Item == null)
            {
                //Item = new ListViewItem(File.Name + File.Extension);
                Item = new ImageItem(File.Name);// + File.Extension);
                ((ImageItem)Item).ItemFile= File;
                ((ImageItem)Item).SetZoom(m_DefaultImageScalingMode);
                listView1.Items.Add(Item);
            }
            Item.Focused = true;
            Item.Selected = true;

            ShowSelectedFile();       
        }
        /// <summary>
        /// Remove file from view, but doesnt delete
        /// </summary>
        /// <param name="File"></param>
        public void RemoveFile(FileInfo File)
        {
            ListViewItem Item = listView1.FindItemWithText(File.Name);
            ListViewItem NextItem = null;
            if (Item != null)
            {
                int Index = Item.Index + 1;
                NextItem = Item.ListView.Items[(Index>=Item.ListView.Items.Count)? 
                    Math.Max(Index-2, 0) : Index]; //select following image or previous
                if (Item.Selected)
                {
                    Item.Remove();
                    pictureBox1.ClearImage();
                    m_ItemIndex = null;
                    if (NextItem != null)
                    {
                        NextItem.Focused = true;
                        NextItem.Selected = true;
                    }
                    ShowSelectedFile();
                }
                else Item.Remove();
            }
        }
        /// <summary>
        /// either directory changed or file
        /// </summary>
        /// <param name="info"></param>
        public void UpdateImageList(FileSystemInfo info)
        {

            if ((info.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {   //if dir find all files residing there (also subdirectorys) and remove from list
                for (int i = listView1.Items.Count-1; i >= 0; i--)
                {
                    ImageItem _item=(ImageItem)listView1.Items[i];
                    if (_item.ItemFile.FullName.StartsWith(info.FullName))
                    {
                        RemoveFile(new FileInfo(_item.ItemFile.FullName));
                    }
                }
            }
            else
            {   //if file verify that it exists or remove from list
                RemoveFile(new FileInfo(info.FullName));
            }
        }
        public void RemoveSelectedFile()
        {
            int Index = -1;
            if (listView1.FocusedItem != null)
            {
                Index = listView1.FocusedItem.Index;
                listView1.FocusedItem.Remove();
                pictureBox1.ClearImage();
                m_ItemIndex = null;
                if (Index > 0 & listView1.Items.Count > 0)
                {
                    listView1.Items[Index - 1].Focused = true;
                    listView1.Items[Index - 1].Selected = true;
                }
                if (Index == 0 & listView1.Items.Count > 0)
                {
                    listView1.Items[0].Focused = true;
                    listView1.Items[0].Selected = true;
                }
            }
            ShowSelectedFile();
 
        }
        public void ShowSelectedFile()
        {
            if (m_ItemIndex != null)
            {//save settings before selecting new Item
                m_ItemIndex.SetScrollPosition(pictureBox1.GetScrollPosition());
                m_ItemIndex.SetZoom(pictureBox1.GetScaling());
            };

            if (listView1.FocusedItem != null)
            {
                m_ItemIndex = ((ImageItem)listView1.FocusedItem);
                pictureBox1.ShowImage(m_ItemIndex.ItemFile,
                    m_ItemIndex.GetZoom(),
                    m_ItemIndex.GetScrollPosition());
            }
            else
            {
                m_ItemIndex = null;
                pictureBox1.ClearImage();
            }
            
        }
        private void ScaleImage()
        {
            pictureBox1.SetScaling(m_ImageScalingMode);
        }
        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ShowSelectedFile();
        }
        private void BtRemoveImage_Click(object sender, EventArgs e)
        {
            RemoveSelectedFile();
        }

        private void DisplayNextImage(int Offset)
        {
            int Index = -1;
            if (listView1.FocusedItem != null && listView1.Items.Count > 0)
            {
                Index = listView1.FocusedItem.Index;
                Index = (Index + Offset);
                if (Index < 0)
                {
                    Index = Math.Abs(Index) % listView1.Items.Count;
                    Index = listView1.Items.Count - Index;
                }
                else
                {
                    Index = Index % listView1.Items.Count;
                }

                    listView1.Items[Index].Focused = true;
                    listView1.Items[Index].Selected = true;

                
            }
        }

        /// <summary>
        /// previous Image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtPrevImage_Click(object sender, EventArgs e)
        {
            DisplayNextImage(-1);
            /*int Index = -1;
            if (listView1.FocusedItem !=null )
            {
                Index = listView1.FocusedItem.Index;
                if (Index > 0 && listView1.Items.Count>0)
                {
                    Index--;
                    listView1.Items[Index].Focused = true;
                    listView1.Items[Index].Selected = true;

                }
            }*/
        }
        /// <summary>
        /// next Image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtNextImage_Click(object sender, EventArgs e)
        {
            DisplayNextImage(+1);
            /*int Index = -1;
            if (listView1.FocusedItem != null)
            {
                Index = listView1.FocusedItem.Index;
                if (Index < listView1.Items.Count-1)
                {
                    Index++;
                    listView1.Items[Index].Focused = true;
                    listView1.Items[Index].Selected = true;

                }
            }*/
        }
        private void toolStripZoomWindow_Click(object sender, EventArgs e)
        {
            m_ImageScalingMode = -1;
            m_DefaultImageScalingMode = m_ImageScalingMode;
            ScaleImage();
        }
        private void toolStripZoomNone_Click(object sender, EventArgs e)
        {
            m_ImageScalingMode = 100;
            m_DefaultImageScalingMode = m_ImageScalingMode;
            ScaleImage();
        }
        private void toolStripZoomShrink_Click(object sender, EventArgs e)
        {
            m_ImageScalingMode = -2;
            m_DefaultImageScalingMode = m_ImageScalingMode;
            ScaleImage();
        }
        private void toolStripZoomValue_TextChanged(object sender, EventArgs e)
        {
            int _Scale = 0;
            try 
            {
                _Scale = Convert.ToInt32(toolStripComboBox1.Text);
                _Scale = Math.Min(1000, Math.Max(0, _Scale));
                m_ImageScalingMode = _Scale;
                m_DefaultImageScalingMode = m_ImageScalingMode;
                ScaleImage();
            }
            catch(FormatException ex)
            {
                ;
            }
        }

        private void OnTimedEvent(object source, EventArgs e)
        {
            BtNextImage_Click(source, null);
        }
        
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
            }
            else 
            {
                timer1.Interval = 3000; //??
                timer1.Start();
            }

        }

        private void FormViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            pictureBox1.ClearImage();   //unlock image file
        }
    }
}
