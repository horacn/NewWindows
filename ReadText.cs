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

namespace NewWindows
{
    public partial class ReadText : Form
    {
        public ReadText()
        {
            InitializeComponent();
        }
        public string txt = null;
        public string FullName;
        private void ReadText_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = txt;
        }

        private void ReadText_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.textBox1.Text == txt)
            {
                return;
            }
           DialogResult result =  MessageBox.Show("是否将更改保存到 "+this.FullName+"？", "记事本", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
           if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            else if (result == DialogResult.Yes)
            {
                FileStream fs = new FileStream(FullName,FileMode.Create);
                StreamWriter sw = new StreamWriter(fs,Encoding.Default);
                sw.Write(this.textBox1.Text);
                sw.Close();
                fs.Close();
            }
        }
    }
}
