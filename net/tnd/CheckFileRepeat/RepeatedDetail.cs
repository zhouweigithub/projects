using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace CheckFileRepeat
{
    public partial class RepeatedDetail : Form
    {
        private readonly string fileName;
        private List<string> sources;

        public RepeatedDetail(string fileName)
        {
            InitializeComponent();
            this.fileName = fileName;

            //双击打开文件
            treeView1.NodeMouseDoubleClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    BLL.DataFileView(e.Node.Tag.ToString());
                }
            };
        }

        private void RepeatedDetail_Load(object sender, EventArgs e)
        {
            sources = BLL.GetFilePaths(fileName);
            sources.Sort();
            foreach (string item in sources)
            {
                FileInfo info = new FileInfo(item);
                long length = info.Length / 1024;
                if (length == 0) length = 1;

                TreeNode node = treeView1.Nodes.Add(item + "      " + length.ToString("#,#") + " kb");
                node.Tag = item;
            }
            label1.Text = fileName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (string item in sources)
            {
                BLL.DataFileView(item);
            }
        }
    }
}
