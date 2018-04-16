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
using System.Collections;

namespace NewWindows
{
    public partial class FrmImages : Form
    {
        public FrmImages()
        {
            InitializeComponent();
        }

        public string FileName;
        Dictionary<int, FileInfo> dic;
        Image image;
        int newCount = 0;
        private void FrmImages_Load(object sender, EventArgs e)
        {
            image = new Bitmap(FileName);  //System.Drawing命名空间，图片地址
            this.pictureBox1.Image = image;
            int index =  FileName.LastIndexOf(@"\");
            string NewFileName = FileName.Substring(0,index);
            DirectoryInfo dir = new DirectoryInfo(NewFileName);
            dic = new Dictionary<int, FileInfo>();
            int count = 0;
            foreach (FileInfo f in dir.GetFiles())
            {
                if (f.Extension.ToLower() == ".jpg" || f.Extension.ToLower() == ".gif" || f.Extension.ToLower() == ".png" || f.Extension.ToLower() == ".ico")
                {
                    count++;
                    dic.Add(count,f);
                }
            }
            newCount = dic.Count;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (dic.Count > 1)
            {
                if (newCount<=1)
                {
                    newCount = dic.Count;
                }
                else
                {
                    newCount--;
                }
                image = new Bitmap(dic[newCount].FullName);
                this.pictureBox1.Image = image;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dic.Count > 1)
            {
                if (newCount >= dic.Count)
                {
                    newCount = 1;
                }
                else
                {
                    newCount++;
                }
                image = new Bitmap(dic[newCount].FullName);
                this.pictureBox1.Image = image;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }
    }
}
