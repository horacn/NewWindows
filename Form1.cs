using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;   // dos命令

namespace NewWindows
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Fiast();
        }
        public void Fiast()
        {
            this.treeView1.Nodes.Clear();
            DirectoryInfo dir = new DirectoryInfo(@"D:\");
            TreeNode tn = new TreeNode(dir.Name);
            this.treeView1.Nodes.Add(tn);
            GetDPan(dir, tn);
            this.treeView1.Nodes[0].Expand();   // 展开当前子节点
        }
        public void GetDPan(DirectoryInfo dir ,TreeNode tn)
        {
            //获取文件夹
            foreach (DirectoryInfo  d in dir.GetDirectories())
            {
                //获取文件夹属性
                FileAttributes att = File.GetAttributes(d.FullName);
                //判断文件夹是否是系统文件
                if ((att & FileAttributes.System) == FileAttributes.System)
                {
                    continue;  // 跳出本次
                }
                TreeNode tn1 = new TreeNode(d.Name);
                tn1.Tag = d;
                tn.Nodes.Add(tn1);
                GetDPan(d,tn1);   // 递归
            }
            //获取文件
            foreach (FileInfo f in dir.GetFiles())
            {
                //获取文件属性
                FileAttributes att = File.GetAttributes(f.FullName);
                //判断文件是否是系统文件
                if ((att & FileAttributes.System) == FileAttributes.System)
                {
                    continue;  // 跳出本次
                }
                TreeNode tn1 = new TreeNode(f.Name);
                tn1.Tag = f;
                tn.Nodes.Add(tn1);
            }
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            FileInfo fs = this.treeView1.SelectedNode.Tag as FileInfo;
            DirectoryInfo dir = this.treeView1.SelectedNode.Tag as DirectoryInfo;
            ShowListView(dir);
            //如果选中的是文件
            if (fs != null)
            {
                GetFileInfoForLIstView(fs);
            }
        }
        //显示ListView的方法
        public void ShowListView(DirectoryInfo dir)  
        {
            this.listView1.Items.Clear();  // 清空

           
            if (dir == null)
                return;
            //遍历文件夹
            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                ListViewItem li = new ListViewItem(d.Name);
                li.SubItems.AddRange(new string[] { "", "文件夹", d.FullName });
                li.Tag = d;//tag绑定文件夹
                this.listView1.Items.Add(li);
            }
            //遍历文件
            foreach (FileInfo f in dir.GetFiles())
            {
                GetFileInfoForLIstView(f);
            }
        }
        //显示文件的方法
        public void GetFileInfoForLIstView(FileInfo f)
        {
            string name = f.Name;
            int index =  name.LastIndexOf(".");
            string newName = name.Substring(0,index);
            ListViewItem li = new ListViewItem(newName);
            //判断文件大小给予相应的单位kb、mb、gb等
            if (f.Length / 1024 / 1024 / 1024 >= 1)
            {
                li.SubItems.AddRange(new string[] { (f.Length / 1024 / 1024 / 1024).ToString() + " GB", f.Extension.Substring(f.Extension.IndexOf(".")+1), f.FullName });
            }
            else if (f.Length / 1024 / 1024 >= 1)
            {
                li.SubItems.AddRange(new string[] { (f.Length / 1024 / 1024).ToString() + " MB", f.Extension.Substring(f.Extension.IndexOf(".")+1), f.FullName });
            }
            else
            {
                li.SubItems.AddRange(new string[] { (f.Length / 1024).ToString() + " KB", f.Extension.Substring(f.Extension.IndexOf(".")+1), f.FullName });
            }
            li.Tag = f;  //tag绑定文件
            this.listView1.Items.Add(li);
        }
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            DirFileName = this.listView1.SelectedItems[0].SubItems[3].Text;
            DirectoryInfo dir = new DirectoryInfo(DirFileName + @"\");
            NewFileInfo = this.listView1.SelectedItems[0].Tag as FileInfo;
            //打开文件夹
            if (this.listView1.SelectedItems[0].SubItems[2].Text.ToString() == "文件夹")
            {
                ShowListView(dir);
            }
            else
            {
                //打开文件
                if (NewFileInfo.Extension == ".txt")//记事本文件
                {
                    
                    //ReadText rt = new ReadText();  //打开窗体
                    //FileStream fs = new FileStream(NewFileInfo.FullName,FileMode.Open);
                    //StreamReader sr = new StreamReader(fs,Encoding.Default);
                    //rt.txt = sr.ReadToEnd();
                    //rt.FullName = NewFileInfo.FullName;
                    //sr.Close();
                    //fs.Close();
                    //rt.Show(); 
                    Process p = new Process();  // 使用Dos命令
                    p.StartInfo.FileName = "notepad.exe";  // 执行的命令
                    p.StartInfo.Arguments = NewFileInfo.FullName;  //打开的文件位置
                    p.Start();       //执行
                }
                else if (NewFileInfo.Extension == ".doc" || NewFileInfo.Extension == ".docx")//打开word
                {
                    Process p = new Process();
                    p.StartInfo.FileName = "winword.exe";
                    p.StartInfo.Arguments = NewFileInfo.FullName;
                    p.Start();
                }
                else if (NewFileInfo.Extension == ".xls" || NewFileInfo.Extension == ".xlsx")//打开excel
                {
                    Process p = new Process();
                    p.StartInfo.FileName = "excel.exe";
                    p.StartInfo.Arguments = NewFileInfo.FullName;
                    p.Start();
                }
                else if (NewFileInfo.Extension == ".ppt" || NewFileInfo.Extension == ".pptx")//打开ppt
                {
                    Process p = new Process();
                    p.StartInfo.FileName = "powerpnt.exe";
                    p.StartInfo.Arguments = NewFileInfo.FullName;
                    p.Start();
                }
                else if (NewFileInfo.Extension == ".vsd")//打开visio
                {
                    Process p = new Process();
                    p.StartInfo.FileName = "visio";
                    p.StartInfo.Arguments = NewFileInfo.FullName;
                    p.Start();
                }
                    //打开图片
                else if (NewFileInfo.Extension.ToLower() == ".jpg" || NewFileInfo.Extension.ToLower() == ".gif" || NewFileInfo.Extension.ToLower() == ".png" || NewFileInfo.Extension.ToLower() == ".ico")
                {
                    //Process p = new Process();
                    //p.StartInfo.FileName = "mspaint";
                    //p.StartInfo.Arguments = NewFileInfo.FullName;
                    //p.Start();
                    FrmImages fi = new FrmImages();
                    fi.FileName = NewFileInfo.FullName;
                    fi.Show();
                }
            }
        }
        string DirFileName = null;
        bool IsSelected = true;
        private void 返回上一级ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = DirFileName.LastIndexOf(@"\");
            string ParmentFileName = DirFileName.Substring(0, index);
            int indexs = ParmentFileName.LastIndexOf(@"\");
            string ParmentFileNames = ParmentFileName.Substring(0, indexs+1);
            if (IsSelected == false)
            {
                ParmentFileNames = ParmentFileName;
            }
            DirectoryInfo dir = new DirectoryInfo(ParmentFileNames);
            ShowListView(dir);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (DirFileName == null)
            {
                this.tsmiBack.Enabled = false;
            }
            else
            {
                this.tsmiBack.Enabled = true;
            }

            if (this.listView1.SelectedItems.Count != 0)
            {
                DirFileName = this.listView1.SelectedItems[0].SubItems[3].Text;
                IsSelected = true;
                复制ToolStripMenuItem.Enabled = true;
                删除ToolStripMenuItem.Enabled = true;
                移动ToolStripMenuItem.Enabled = true;
                NewFileInfo = this.listView1.SelectedItems[0].Tag as FileInfo;
                NewDir = this.listView1.SelectedItems[0].Tag as DirectoryInfo;
                //FileName = this.listView1.SelectedItems[0].SubItems[3].Text;
            }
            else
            {
                IsSelected = false;
                复制ToolStripMenuItem.Enabled = false;
                删除ToolStripMenuItem.Enabled = false;
                移动ToolStripMenuItem.Enabled = false;
            }
        }
        //string FileName = null;
        FileInfo NewFileInfo;
        DirectoryInfo NewDir;
        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fbd.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            if (this.listView1.SelectedItems[0].SubItems[2].Text.ToString() == "文件夹")
            {
                string toFileNames = this.fbd.SelectedPath + @"\" + this.listView1.SelectedItems[0].SubItems[0].Text;
                MessageBox.Show("目前无法实现文件夹复制！");
            }
            else
            {
                string toFileName = this.fbd.SelectedPath + @"\" + this.listView1.SelectedItems[0].SubItems[0].Text + "." + this.listView1.SelectedItems[0].SubItems[2].Text;
                //File.Copy(FileName,toFileName);  //第一中做法，用File静态类
                NewFileInfo.CopyTo(toFileName);
            }
            Fiast();  //重新显示treeview
        }
       
        private void 移动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fbd.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            if (this.listView1.SelectedItems[0].SubItems[2].Text.ToString() == "文件夹")
            {
                string toFileNames = this.fbd.SelectedPath + @"\" + this.listView1.SelectedItems[0].SubItems[0].Text;
                NewDir.MoveTo(toFileNames);
            }
            else
            {
                string toFileName = this.fbd.SelectedPath + @"\" + this.listView1.SelectedItems[0].SubItems[0].Text + "." + this.listView1.SelectedItems[0].SubItems[2].Text;
                //File.Move(FileName, toFileName);//第一中做法，用File静态类
                NewFileInfo.MoveTo(toFileName);
            }
            Fiast();  //重新显示treeview
            this.listView1.SelectedItems[0].Remove();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定删除吗？", "删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                return;
            }
            if (this.listView1.SelectedItems[0].SubItems[2].Text.ToString() == "文件夹")
            {
                NewDir.Delete(true);   //表示删除文件夹包括所有子文件及子文件夹
            }
            else
            {
                //File.Delete(FileName);//第一中做法，用File静态类
                NewFileInfo.Delete();
            }
            Fiast();  //重新显示treeview
            this.listView1.SelectedItems[0].Remove();
        }
    }
}
